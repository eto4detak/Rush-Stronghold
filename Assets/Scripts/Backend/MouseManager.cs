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
    private RaycastHit mouseHit;
    private float timeOldClick;
    private float timeCheckDoubleClick = 0.3f;
    bool isDoubleClick = false;
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
                if (Time.time < timeOldClick + timeCheckDoubleClick)
                {
                    // double click
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
            DoMouseHit(pathLayer);
            trail.transform.position = mouseHit.point;
            mousePath.Add(mouseHit.point);
        }
    }


    private void TrySelectUnit()
    {
        startPos = Input.mousePosition;
        DoMouseHit(characterLayer);

        if (mouseHit.collider == null)
        {
            DoMouseHit(-1);
            Collider[] units = Physics.OverlapSphere(mouseHit.point, 2f, characterLayer);
            if (units.Length > 0) selected = units[0].GetComponent<CharacterManager>();
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
        trail = Instantiate(prefabTrail, mouseHit.point, Quaternion.identity);
    }

    private void SelectGroup()
    {
        selectedGroup = selected.GetClosestGroup(PController.instance.playerUnits);
    }
    private void CreatePathCommand()
    {
        if(selectedGroup.Count > 0 && mousePath.Count > 40)
        {
            for (int i = 0; i < selectedGroup.Count; i++)
            {
                Vector3 offset = selectedGroup[i].transform.position - selected.transform.position ;
                List<Vector3> gPath = new List<Vector3>(mousePath);
                for (int g = 0; g < gPath.Count; g++)
                {
                    gPath[g] += offset;
                }
                selectedGroup[i].command = new RushCommand(selectedGroup[i], gPath);
            }
        }
        else if(mousePath.Count > 10)
        {
            selected.command = new RushCommand(selected, mousePath);
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

    private void OnClickRightObject(CharacterManager target, Vector3 hitPoint)
    {
        CharacterManager selected = PController.instance.GetClosestFreePlayerUnit(hitPoint);
        if (selected != null && target.GetTeam() != selected.GetTeam())
            selected.command = new AttackCommand(selected, target);
    }

    private void OnClickRight(Vector3 hitPoint)
    {
        CharacterManager selected = PController.instance.GetClosestFreePlayerUnit(hitPoint);
        if(selected != null)
        selected.command = new RushCommand(selected, hitPoint);
    }
}