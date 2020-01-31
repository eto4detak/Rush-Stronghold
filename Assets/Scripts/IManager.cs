using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IManager 
{
}
public interface IMovable
{
    void SetTargetMovement(Transform target);
    void SetTargetMovement(Vector3 target);
    void SetSpeed(float speed);
}
