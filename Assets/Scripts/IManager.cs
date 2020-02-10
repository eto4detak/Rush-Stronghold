using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IManager 
{
}
public interface IMovable
{
    void MoveTo(Transform target);
    void MoveTo(Vector3 target);
    void SetSpeed(float speed);
    void Stop();

}

public interface IAttack
{
    void Attack(Health newTarget);
    void NoAttack();
}


public interface IUnit
{
    Team GetTeam();
    Transform GetTransform();
}
