using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterManager : MonoBehaviour, IMovable, IAttack, IUnit
{
    public CharacterContrMovement movement;
    public CharacterAttack armament;
    public CharacterCommand command;
    public Animator animator;
    public Health health;
    public float targetRadius = 1.5f;
    public CharacterEvent die = new CharacterEvent();
    public CharacterEvent EventStart = new CharacterEvent();
    public bool isFree = true;

    private readonly int hashDieAnim = Animator.StringToHash("Die");

    private void Awake()
    {
        movement = GetComponent<CharacterContrMovement>();
        //if(m != null) movement = m;
        //else movement = GetComponent<CharacterContrMovement>();
    }

    private void Start()
    {
        EventStart.Invoke(this);
        health?.EventDie.AddListener(OnDie);
        PController.instance.AttachedUnit(this);
    }

    private void Update()
    {
        if (command == null) return;
        if(command.CheckCanCancel())
        {
            command = new GuardCommand(this);
        }
        command.DoCommand();

    }

    public void EnableUnit()
    {
        enabled = true;
        if (movement != null) movement.enabled = true;
    }
    public void DesableUnit()
    {
        if(movement != null) movement.enabled = false;
        enabled = false;
    }


    public void MoveTo(Transform target)
    {
        isFree = false;
        armament.NoAttack();
        movement.MoveTo(target);
    }

    public void MoveTo(Vector3 target)
    {
        isFree = false;
        armament.NoAttack();
        movement.MoveTo(target);
    }

    public void SetSpeed(float _speed)
    {
        movement.SetSpeed(_speed);
    }

    public void Attack(Health newTarget)
    {
        isFree = false;
        movement.Stop();
        armament.Attack(newTarget);
    }

    public void NoAttack()
    {
        isFree = true;
        armament.NoAttack();
    }
    public void Stop()
    {
        isFree = true;
        if(movement != null) movement.Stop();
    }

    public List<Health> FilterEnemy(Collider[] findStack)
    {
        List<Health> found = new List<Health>();
        Health tempUnit;
        for (int i = 0; i < findStack.Length; i++)
        {
            tempUnit = findStack[i].GetComponent<Health>();
            if (tempUnit == null || tempUnit.GetTeam() == health.GetTeam()) continue;
            found.Add(tempUnit);
        }
        return found;
    }

    public List<Health> CheckAllies(Collider[] findStack)
    {
        List<Health> found = new List<Health>();
        Health tempUnit;
        for (int i = 0; i < findStack.Length; i++)
        {
            tempUnit = findStack[i].GetComponent<Health>();
            if(tempUnit && Unions.CheckAllies(tempUnit.GetTeam(), health.GetTeam())) found.Add(tempUnit);
        }
        return found;
    }

    public Team GetTeam()
    {
        return health.GetTeam();
    }

    public Transform GetTransform()
    {
        return transform;
    }

    public Health FindTarget()
    {
        List<Health> enemies = FilterEnemy(Physics.OverlapSphere(transform.position, targetRadius));
        return this.GetClosest(enemies) as Health;
    }

    public List<CharacterManager> GetClosestGroup(List<CharacterManager> stack)
    {
        List<CharacterManager> group = new List<CharacterManager> { this };
        Vector3 dirToTarget;
        float groupRadius = 10f;
        for (int i = 0; i < stack.Count; i++)
        {
            if (group.Exists(x => x.Equals(stack[i]))) continue;
            dirToTarget = stack[i].transform.position - transform.position;
            if (dirToTarget.sqrMagnitude < groupRadius * groupRadius)
            {
                group.Add(stack[i]);
            }
        }

        return group;
    }

    private void OnDie()
    {
        GetComponent<CharacterController>().enabled = false;
        GetComponent<CharacterContrMovement>().enabled = false;
        GetComponent<Collider>().enabled = false;
        Destroy(this);
        armament.enabled = false;
        enabled = false;
        animator.SetBool(hashDieAnim, true);
        die?.Invoke(this);
    }
}

[System.Serializable]
public class CharacterEvent : UnityEvent<CharacterManager>
{
}