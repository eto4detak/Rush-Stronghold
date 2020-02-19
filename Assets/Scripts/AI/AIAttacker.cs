using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAttacker : AIUnit
{
    public CharacterManager target;
    [HideInInspector]
    public CharacterAttack attacker;

    private float awayTime = 1f;
    private float currentAwayTime;

    private float maxTime = 1f;
    private float currentTime;
    private float travaledDistance;
    private float maxDistance;
    private Vector3 oldPosition;
    private int bypassCounter;
    private bool isStoped;
    private List<Vector3> direccts = new List<Vector3>();
    private bool isMoveAway;
    private CharacterCommand extraCommad;

    protected void Awake()
    {
        base.Awake();
        direccts.Add( new Vector3(0, 0, 1));
        direccts.Add(new Vector3(0, 0, -1));
        direccts.Add( new Vector3(1, 0, 0));
        direccts.Add( new Vector3(1, 0, 1));
        direccts.Add(new Vector3(1, 0, -1));
        direccts.Add(new Vector3(-1, 0, 0));
        direccts.Add( new Vector3(-1, 0, 1));
        direccts.Add(new Vector3(-1, 0, -1));
    }

    private void Start()
    {
        attacker = GetComponent<CharacterAttack>();
    }

    private void Update()
    {
        
        if (isMoveAway)
        {
            currentAwayTime += Time.deltaTime;
            if (currentAwayTime > awayTime)
            {
                currentAwayTime = 0;
                isMoveAway = false;
            }
                return;
        }
        if (bypassCounter > 0)
        {
            bypassCounter = 0;
            MoveAway();
        }
        else
        {
            CalculateMovement();
        }
        oldPosition = owner.transform.position;
    }

    private void CalculateMovement()
    {
        if (!owner.movement.run)
        {
            currentTime = 0;
            travaledDistance = 0;
            maxDistance = 0;
            bypassCounter = 0;
            return;
        }

        currentTime += Time.deltaTime;
        travaledDistance += (owner.transform.position - oldPosition).magnitude;
        maxDistance += owner.movement.MoveVector.magnitude;
        if (currentTime > maxTime)
        {
            if (travaledDistance < maxDistance / 10)
            {
                bypassCounter++;
                //owner.movement.Jump();
            }
            else
            {
                bypassCounter = 0;
            }
            travaledDistance = 0;
            maxDistance = 0;
            currentTime = 0;
        }
    }

    private void MoveAway()
    {
        isMoveAway = true;
        Vector3 start = owner.transform.position + new Vector3(0, 1, 0);
        Vector3 point = start + direccts[Random.Range(0, direccts.Count - 1)] * 6;
        owner.commands.Remove(extraCommad);
        extraCommad = new MoveCommand(owner, point);
        owner.PushCommand(extraCommad);
    }

    private void FindPath()
    {
        Vector3 start = owner.transform.position + new Vector3(0,1,0);
        Physics.Raycast(new Ray(start, owner.transform.forward), out RaycastHit hit, 1f);
        if(hit.collider != null)
        {
            
        }
        for (int i = 0; i < direccts.Count; i++)
        {
            Vector3 position = start + direccts[i];
            Vector3 dir = attacker.Target.transform.position - position;
            RaycastHit[] raycast = Physics.RaycastAll(new Ray(position, dir), dir.magnitude);
            if (hit.collider == null)
            {
                Physics.RaycastAll(new Ray(start, direccts[i]), 1f);
                if (hit.collider == null)
                {

                }
            }
        }
    }

    public override void StartCommand()
    {
        if(target == null)
        {
            target = UnityExtension.GetClosest(owner, PController.instance.playerUnits) as CharacterManager;
        }
        owner.Command = new FindEnemyCommand(owner);
    }

}
