using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterManager : MonoBehaviour, IMovable, IAttack, IUnit
{
    private CharacterCommand command;
    public Animator animator;
    public Health health;
    public float targetRadius = 1.5f;
    public CharacterEvent die = new CharacterEvent();
    public CharacterEvent EventStart = new CharacterEvent();
    public bool isFree = true;
    public CharacterContrMovement movement;
    [HideInInspector]
    public List<CharacterCommand> commands = new List<CharacterCommand>();

    [SerializeField] private ParticleSystem psSelected;
    private readonly int hashDieAnim = Animator.StringToHash("Die");

    public CharacterAttack Armament { get; private set; }
    public CharacterCommand Command { get => command; set => command = value; }

    private void Awake()
    {
        Armament = GetComponent<CharacterAttack>();
        movement = GetComponent<CharacterContrMovement>();
        if(psSelected) psSelected.gameObject.SetActive(false);
    }

    private void Start()
    {
        EventStart.Invoke(this);
        health?.EventDie.AddListener(OnDie);
        PController.instance.AttachedUnit(this);
    }

    private void Update()
    {
        if (Command == null)
        {
            commands.RemoveAll(x => x == null);
            if (commands.Count > 0)
            {
                Command = commands[0];
            }
            else
            {
                return;
            }
        }

        if(Command.CheckCanCancel())
        {
            commands.Remove(Command);
            if (commands.Count > 0)
            {
                commands.RemoveAll(x => x == null);
                Command = commands[0];
            }
            else
            {
                Command = new GuardCommand(this);
            }
        }
        Command.DoCommand();


    }


    public void PushCommand(CharacterCommand _command)
    {
        List < CharacterCommand > newCommands = new List<CharacterCommand>();
        newCommands.Add(_command);

        if (commands.Count > 0)
            newCommands.Add(commands[commands.Count - 1]);
        else newCommands.Add(command);
        commands = newCommands;
        command = _command;
    }


    public void SetPathCommand(List<Vector3> path)
    {
        if (Armament.AttackType is ArcherType)
        {
            Command = new MoveCommand(this, path);
        }
        else
        {
            Command = new RushCommand(this, path);
        }
    }

    public void EnableUnit()
    {
        enabled = true;
        if (movement != null) movement.enabled = true;
    }
    public void DesableUnit()
    {
        if(movement != null) movement.enabled = false;
        //enabled = false;
    }


    public void MoveTo(Transform target)
    {
        isFree = false;
        Armament.NoAttack();
        movement.MoveTo(target);
    }

    public void MoveTo(Vector3 target)
    {
        isFree = false;
        Armament.NoAttack();
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
        Armament.Attack(newTarget);
    }

    public void NoAttack()
    {
        isFree = true;
        Armament.NoAttack();
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
            if (tempUnit == null || !Unions.instance.CheckEnemies(tempUnit.GetTeam(), health.GetTeam()) ) continue;
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

    public Health FindTarget(float radius = 0)
    {
        if (radius == 0) radius = targetRadius;
        List<Health> enemies = FilterEnemy(Physics.OverlapSphere(transform.position, radius));
        return this.GetClosest(enemies) as Health;
    }

    public List<CharacterManager> GetClosestGroup(List<CharacterManager> stack)
    {
        List<CharacterManager> group = new List<CharacterManager> { this };
        Vector3 dirToTarget;
        float groupRadius = 7f;
        for (int i = 0; i < stack.Count; i++)
        {
            if (stack[i] == null) continue;
            if (group.Exists(x => x.Equals(stack[i]))) continue;
            dirToTarget = stack[i].transform.position - transform.position;
            if (dirToTarget.sqrMagnitude < groupRadius * groupRadius)
            {
                group.Add(stack[i]);
            }
        }

        return group;
    }

    public void PlaySelected()
    {
        if (psSelected == null) return;
        psSelected.gameObject.SetActive(true);
        psSelected.Play();
    }

    private void OnDie()
    {
        GetComponent<CharacterController>().enabled = false;
        GetComponent<CharacterContrMovement>().enabled = false;
        GetComponent<Collider>().enabled = false;
        Armament.enabled = false;
        enabled = false;
        animator.SetTrigger(hashDieAnim);
        die?.Invoke(this);
        Destroy(this);
    }


}

[System.Serializable]
public class CharacterEvent : UnityEvent<CharacterManager>
{
}