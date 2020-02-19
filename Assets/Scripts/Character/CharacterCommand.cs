using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterCommand
{
    protected CharacterManager self { get; set; }
    protected Health Target { get; set; }
    public virtual void DoCommand()
    {
        
    }

    public virtual void OnStay(CharacterManager target)
    {
        //SelfGroup.TryAttackUnit(target);
    }
    public virtual bool CheckCanCancel()
    {
        return false;
    }
}

public class AttackCommand : CharacterCommand
{
    private float attackRadius = 2f;
    public AttackCommand(CharacterManager _self, CharacterManager _target)
    {
        self = _self;
        Target = _target.health;
        DoCommand();
    }
    public override void DoCommand()
    {
        if ( (Target.transform.position - self.transform.position).magnitude < attackRadius)
        {

            self.Attack(Target);
        }
        else
        {
            self.MoveTo(Target.transform);
        }
    }

    public override bool CheckCanCancel()
    {
        if (Target == null) return true;
        return false;
    }
}
public class MoveCommand : CharacterCommand
{
    public List<Vector3> path { get; set; }
    private float permissibleLenght = 0.2f;
    public MoveCommand(CharacterManager _self, List<Vector3> _path)
    {
        self = _self;
        path = _path;
        DoCommand();
    }
    public MoveCommand(CharacterManager _self, Vector3 point)
    {
        path = new List<Vector3>
        {
            point
        };
        self = _self;
        DoCommand();
    }

    public override void DoCommand()
    {
        if(self.Armament.isAttack) self.NoAttack();
        self.MoveTo(path[0]);

    }
    public override bool CheckCanCancel()
    {
        CheckPath();
        //float lenght = UnityExtension.SumPath(path);
        if (path.Count == 0) return true;
        return false;
    }

    private void CheckPath()
    {
        if (path.Count == 0) return;
        Vector3 direction = (path[0] - self.transform.position);
        direction.y = 0;
        if (direction.magnitude < permissibleLenght)
        {
            path.RemoveAt(0);
            CheckPath();
        }
    }
}
public class StopCommand : CharacterCommand
{
    public Vector3 NewPosition { get; set; }
    public Vector3 GroupOffset { get; set; }
    public StopCommand(CharacterManager _self)
    {
        self = _self;
        DoCommand();
    }

    public override void DoCommand()
    {
        self.Stop();
        self.NoAttack();
    }

}

public class PursueCommand : CharacterCommand
{
    public PursueCommand(CharacterManager paramGroup, CharacterManager paramTarget)
    {
        self = paramGroup;
        Target = paramTarget.health;

        DoCommand();
    }

    public override void DoCommand()
    {
        //if (Target.units.Count > 0)
        //{
        //    for (int i = 0; i < Target.units.Count; i++)
        //    {
        //        if (Target.units[i] != null)
        //        {
        //            SelfGroup.MoveGroupToPoint3D(Target.units[i].transform.position);
        //            return;
        //        }
        //    }
        //}
    }
}
public class ProtectionCommand : CharacterCommand
{
    public ProtectionCommand(CharacterManager paramGroup, CharacterManager _target)
    {
        self = paramGroup;
        Target = _target.health;

        DoCommand();
    }

    public override void DoCommand()
    {
        //if (Target.units.Count > 0)
        //{
        //    for (int i = 0; i < Target.units.Count; i++)
        //    {
        //        if (Target.units[i] != null)
        //        {
        //            SelfGroup.MoveGroupToPoint3D(Target.units[i].transform.position);
        //            return;
        //        }
        //    }
        //}
    }
}
public class GuardCommand : CharacterCommand
{
    private float changeTime;
    private float maxChangeTime = 1f;
    public GuardCommand(CharacterManager _self)
    {
        self = _self;
        DoCommand();
    }

    public override void DoCommand()
    {
        changeTime += Time.deltaTime;
        if (changeTime > maxChangeTime)
        {
            changeTime = 0;
            Target = self.FindTarget();
        }
        if (Target != null)
        {
            self.Attack(Target);
        }
        else
        {
            self.NoAttack();
        }
    }


}

public class RushCommand : CharacterCommand
{
    private List<Vector3> path;
    private float changeTime;
    private float maxChangeTime = 0.5f;
    private float distance;
    private float trigerDistance = 0.3f;
    public RushCommand(CharacterManager _self, List<Vector3> _path)
    {
        path = _path;
        self = _self;
        DoCommand();
    }
    public RushCommand(CharacterManager _self, Vector3 point)
    {
        path = new List<Vector3>
        {
            point
        };
        self = _self;
        DoCommand();
    }
    public override void DoCommand()
    {
        if (path.Count < 1) return;
         changeTime += Time.deltaTime;
        if (changeTime > maxChangeTime)
        {
            changeTime = 0;
            Target = self.FindTarget();
        }
        if (Target != null)
        {
            self.Attack(Target);
        }
        else
        {
            self.MoveTo(path[0]);
        }
    }


    public override bool CheckCanCancel()
    {
        CheckPath();
        if (distance == 0) return true;
        if(distance < 4f)
        {
            RaycastHit[] hits = Physics.RaycastAll(self.transform.position, path[0] - self.transform.position, 100.0F);
            Collider[] lineUnits = new Collider[hits.Length];
            for (int i = 0; i < hits.Length; i++)
            {
                lineUnits[i] = hits[i].collider;
            }
            List<Health> allies = self.CheckAllies(lineUnits);
            float maxDistance = allies.Count * 1f;
            if (distance < maxDistance) return true;
        }
        return false;
    }

    private void CheckPath()
    {
        distance = 0;
        if (path.Count == 0) return;
        Vector3 direction = (path[0] - self.transform.position);
        direction.y = direction.y > 0 ? direction.y : 0;

        distance = direction.magnitude;
        if (distance < trigerDistance)
        {
            path.RemoveAt(0);
            CheckPath();
        }
    }

}


public class FindEnemyCommand : CharacterCommand
{
    private float currentTime;
    private float maxTime = 0.5f;
    private float distance;
    private float trigerDistance = 0.3f;

    public FindEnemyCommand(CharacterManager _self)
    {
        self = _self;
        DoCommand();
    }

    public override void DoCommand()
    {
        currentTime -= Time.deltaTime;
        if (currentTime <= 0)
        {
            currentTime = maxTime;
            List<Health> enemies = self.FilterEnemy(Physics.OverlapSphere(self.transform.position, 40f));
            Target = self.GetClosest(enemies) as Health;
        }
        if(Target != null)
        {
            if ((Target.transform.position - self.transform.position).magnitude < self.targetRadius)
                self.Attack(Target);
            else self.MoveTo(Target.transform);
        }
    }

    public override bool CheckCanCancel()
    {
        return false;
    }
}