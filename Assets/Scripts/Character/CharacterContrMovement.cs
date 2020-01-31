using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterContrMovement : MonoBehaviour, IMovable
{
    public CharacterController ch_controller;
    private Vector3 moveVector;
    private float speed = 1f;
    private float gravitiForce;
    private Vector3? target;
    private float allowableError = 0.1f;

    private void FixedUpdate()
    {
        moveVector = Vector3.zero;
        GoInTarget();
        CharacterMove();
        GamingGravity();
    }

    public void SetTargetMovement(Transform _target)
    {
        target = _target.transform.position;
    }

    public void SetTargetMovement(Vector3 _target)
    {
        target = _target;
    }

    private void GoInTarget()
    {
        if (target == null) return;
        moveVector = new Vector3(target.Value.x - transform.position.x, 0, target.Value.z - transform.position.z);
        if (moveVector.magnitude < allowableError)
        {
            target = null;
        }
    }

    private void Control()
    {
        moveVector.x = Input.GetAxis("Horizontal");
        moveVector.z = Input.GetAxis("Vertical");
    }

    private void CharacterMove()
    {
        moveVector = moveVector.normalized / 10 * speed;
        if (Vector3.Angle(Vector3.forward, moveVector) > 1f || Vector3.Angle(Vector3.forward, moveVector) == 0)
        {
            Vector3 direct = Vector3.RotateTowards(transform.forward, moveVector, 1, 0.0f);
            transform.rotation = Quaternion.LookRotation(direct);
        }
        moveVector.y = gravitiForce;
        ch_controller.Move(moveVector);
    }

    private void GamingGravity()
    {
        if (!ch_controller.isGrounded) gravitiForce -= Time.deltaTime;
        else gravitiForce = 0;
    }

    public void SetSpeed(float _speed)
    {
        speed = _speed;
    }
}
