using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterContrMovement : MonoBehaviour, IMovable
{
    public CharacterController ch_controller;
    public Animator animator;
    public float speed = 1f;

    private Vector3 moveVector;
    private float gravitiForce;
    private Vector3? target;
    private float allowableError = 0.1f;
    private float jumpForce = 0.1f;
    private Vector3 oldPosition;
    private float deltaDistance;
    private float turnRotation = 0.1f;
    private readonly int hashRun = Animator.StringToHash("Run");
    private bool bypass;
    private float maxBypassTime = 2f;
    private float currentBypassTime;

    private void FixedUpdate()
    {
        deltaDistance = (transform.position - oldPosition).magnitude;
        oldPosition = transform.position;
        moveVector = Vector3.zero;
        GoInTarget();
        CharacterMove();
        GamingGravity();
    }

    public void MoveTo(Transform _target)
    {
        target = _target.position;
    }

    public void MoveTo(Vector3 _target)
    {
        target = _target;
    }
    public void SetSpeed(float _speed)
    {
        speed = _speed;
    }

    public void Stop()
    {
        target = null;
    }

    public void Jump()
    {
        if (ch_controller.isGrounded)  gravitiForce = jumpForce;
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
        //needed rotation
        //bool neededRotation = bypass || (moveVector.magnitude > 0.001f && deltaDistance < 0.02f);
        bool neededRotation = moveVector.magnitude > 0.001f && deltaDistance < 0.02f;
        if (neededRotation)
        {
            //bypass = true;
            //currentBypassTime += Time.deltaTime;
            //if (currentBypassTime > maxBypassTime)
            //{
            //    bypass = false;
            //}
           // moveVector = moveVector.normalized + transform.right;
        }

        if (moveVector.magnitude > 0.001f)
        {
            animator.SetBool(hashRun, true);
        }
        else
        {
            animator.SetBool(hashRun, false);
        }
        moveVector = moveVector.normalized / 10 * speed;

        if (Vector3.Angle(Vector3.forward, moveVector) > 1f || Vector3.Angle(Vector3.forward, moveVector) == 0)
        {
            Vector3 direct = Vector3.RotateTowards(transform.forward, moveVector, turnRotation, 0.0f);
            transform.rotation = Quaternion.LookRotation(direct);
        }
       // if (moveVector.magnitude > 0.01f && deltaDistance < 0.01f) Jump();
        moveVector.y = gravitiForce;
        ch_controller.Move(moveVector);
    }

    private void GamingGravity()
    {
        if (!ch_controller.isGrounded) gravitiForce -= Time.deltaTime;
        else gravitiForce = 0;
    }
}
