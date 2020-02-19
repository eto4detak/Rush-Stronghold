using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MouseManager : MonoBehaviour
{
    [SerializeField]
    private TrailRenderer prefabTrail;

    private TrailRenderer trail;
    private bool drag = false;
    private Vector2 startPos;
    private Vector2 endPos;
    private float minY = 0;
    private CharacterManager selected;
    private List<CharacterManager> selectedGroup = new List<CharacterManager>();
    public List<Vector3> mousePath = new List<Vector3>();
    public Text text;

    private int characterLayer;
    private int pathLayer;
    private int noPathLayer;
    private RaycastHit mouseHit;
    private float timeOldClick;
    private float timeCheckDoubleClick = 0.3f;
    private bool isDoubleClick = false;
    private float findRadius = 1.5f;
    private int maxWrongPath = 30;
    #region Singleton
    static protected MouseManager s_Instance;
    static public MouseManager instance { get { return s_Instance; } }
    #endregion

    void Awake()
    {
        #region Singleton
        if (s_Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        s_Instance = this;
        #endregion
    }
    
    private void Start()
    {
        characterLayer = LayerMask.GetMask("Character");
        pathLayer = LayerMask.GetMask("Path") & ~LayerMask.GetMask("Character");
        noPathLayer = LayerMask.GetMask("NoPath");
    }

    void Update()
    {
        //drag path

        ControleDragPath();

        //click to unit
        //CheckLeftClick();
    }



    private void ControleDragPath()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TrySelectUnit();
            if (drag)
            {
                // double click
                if (Time.time < timeOldClick + timeCheckDoubleClick)
                {
                    SelectGroup();
                    return;
                }
                else timeOldClick = Time.time;
                CreateTrail();
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (drag)
            {
                CreatePathCommand();
            }
            drag = false;
            if (trail != null)
            {
                trail.time = trail.time / 2;
                Destroy(trail.gameObject, trail.time);
            }
        }

        if (drag)
        {
            DoMouseHit(noPathLayer);
            if(mouseHit.collider == null)
            {
                DoMouseHit(pathLayer);
                if (trail != null)
                {
                    trail.transform.position = mouseHit.point + new Vector3(0, 0.2f, 0);
                    mousePath.Add(mouseHit.point);
                }
            }
        }
    }


    private void TrySelectUnit()
    {
        startPos = Input.mousePosition;
        DoMouseHit(characterLayer);

        if (mouseHit.collider == null)
        {
            DoMouseHit(-1);
            Collider[] units = Physics.OverlapSphere(mouseHit.point, findRadius, characterLayer);
            if (units.Length > 0)
            {
                selected = UnityExtension.GetClosest(mouseHit.point, PController.instance.playerUnits) as CharacterManager;
            }
            else return;
        }
        else
        {
            selected = mouseHit.collider.GetComponent<CharacterManager>();
        }
        if (selected == null || selected.GetTeam() != Team.Player1) return;
        mousePath.Clear();
        drag = true;
    }

    private void CreateTrail()
    {
        if (trail != null)
        {
            trail.time = trail.time / 2;
            Destroy(trail.gameObject, trail.time);
        }
        trail = Instantiate(prefabTrail, mouseHit.point + new Vector3(0, 0.2f, 0), Quaternion.identity);
    }

    private void SelectGroup()
    {
        selectedGroup = selected.GetClosestGroup(PController.instance.playerUnits);
        for (int i = 0; i < selectedGroup.Count; i++)
        {
            selectedGroup[i].PlaySelected();
        }
    }

    private void CreatePathCommand()
    {
        if (selected == null) return;
        int minGroupPointer = 30;
        int minGroupLenght = 1;
        int minSinglePointer = 10;
        Vector3 startPoint = selected.transform.position;
        bool isMin = mousePath.Count > minGroupPointer || UnityExtension.SumPath(mousePath) > minGroupLenght;

        ImprovePath();
        if (selectedGroup.Count > 1 && isMin)
        {
            for (int i = 0; i < selectedGroup.Count; i++)
            {
                if (selectedGroup[i] == null) continue;
                Vector3 offset = selectedGroup[i].transform.position - startPoint;
                List<Vector3> gPath = new List<Vector3>(mousePath);
                for (int g = 0; g < gPath.Count; g++)
                {
                    gPath[g] += offset;
                }
                selectedGroup[i].SetPathCommand(gPath);
            }
            
        }
        else if(mousePath.Count > minSinglePointer)
        {

            selected.SetPathCommand(mousePath);
        }
        selectedGroup.Clear();
        selected = null;
        mousePath = new List<Vector3>();
    }

    private void DoMouseHit(int layout)
    {
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(mouseRay, out mouseHit, 100, layout);
    }

    private bool IsPointerOverUIObject()
    {
        PointerEventData e = new PointerEventData(EventSystem.current);
        e.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(e, results);
        return results.Count > 0;
    }
    
    private void CheckLeftClick()
    {
        if (IsPointerOverUIObject()) return;
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            CharacterManager unitTarget;
            
            if (Physics.Raycast(mouseRay, out RaycastHit mouseHit, 100, characterLayer))
            {
                //attack
                unitTarget = mouseHit.collider.GetComponent<CharacterManager>();
                if (unitTarget)
                {
                    OnClickRightObject(unitTarget, mouseHit.point);
                    return;
                }

                //move
                Collider filterTerrain = mouseHit.collider.GetComponent(typeof(Collider)) as Collider;
                if (filterTerrain)
                {
                    OnClickRight(mouseHit.point);
                    return;
                }
            }
        }
    }
    private void ImprovePath()
    {
        int indexRemove = -1;
        Vector3 direction = Vector3.zero;
        for (int i = 0; i < maxWrongPath && i < mousePath.Count; i++)
        {
            direction = (mousePath[i] - selected.transform.position);
            direction.y = direction.y > 0 ? direction.y : 0;
            if (direction.magnitude < 0.5f)
            {
                indexRemove = i;
            }
        }
        if (indexRemove > -1)
        {
            mousePath.RemoveRange(0, indexRemove);
        }
    }

    private void OnClickRightObject(CharacterManager target, Vector3 hitPoint)
    {
        CharacterManager selected = PController.instance.GetClosestFreePlayerUnit(hitPoint);
        if (selected != null && target.GetTeam() != selected.GetTeam())
            selected.Command = new AttackCommand(selected, target);
    }

    private void OnClickRight(Vector3 hitPoint)
    {
        CharacterManager selected = PController.instance.GetClosestFreePlayerUnit(hitPoint);
        if(selected != null)
        selected.Command = new RushCommand(selected, hitPoint);
    }
}