using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterCommand
{
    protected CharacterManager SelfGroup { get; set; }
    protected CharacterManager Target { get; set; }
    public virtual void DoCommand()
    {
        
    }

    public virtual void OnStay(CharacterManager target)
    {
        //SelfGroup.TryAttackUnit(target);
    }
}

public class AttackCommand : CharacterCommand
{
    public AttackCommand(CharacterManager paramSelf, CharacterManager paramTarget)
    {
        SelfGroup = paramSelf;
        Target = paramTarget;
        DoCommand();
    }
    public override void DoCommand()
    {
        Debug.Log("AttackCommand " + SelfGroup.name);

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
public class MoveCommand : CharacterCommand
{
    public Vector3 NewPosition { get; set; }
    public Vector3 GroupOffset { get; set; }
    public MoveCommand(CharacterManager paramGroup, Vector3 paramNewPosition, Vector3 paramGroupOffset)
    {
        SelfGroup = paramGroup;
        NewPosition = paramNewPosition;
        GroupOffset = paramGroupOffset;

       // SelfGroup.MoveGroupToPoint2D(NewPosition, GroupOffset);
        DoCommand();
    }

    public override void DoCommand()
    {
        //if (SelfGroup.CheckStopped())
        //{
        //    SelfGroup.command = new StopCommand(SelfGroup);
        //}
    }
}
public class StopCommand : CharacterCommand
{
    public Vector3 NewPosition { get; set; }
    public Vector3 GroupOffset { get; set; }
    public StopCommand(CharacterManager paramSelfGroup)
    {
        SelfGroup = paramSelfGroup;
        DoCommand();
    }

    public override void DoCommand()
    {
    }

}

public class PursueCommand : CharacterCommand
{
    public PursueCommand(CharacterManager paramGroup, CharacterManager paramTarget)
    {
        SelfGroup = paramGroup;
        Target = paramTarget;

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
    public ProtectionCommand(CharacterManager paramGroup, CharacterManager paramTarget)
    {
        SelfGroup = paramGroup;
        Target = paramTarget;

        DoCommand();
    }

    public override void DoCommand()
    {
        Debug.Log("PursueCommand " + SelfGroup.name);

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

