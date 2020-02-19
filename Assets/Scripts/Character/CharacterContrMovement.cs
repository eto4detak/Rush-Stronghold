using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterContrMovement : MonoBehaviour, IMovable
{
    public Animator animator;
    public float speed = 1f;
    public bool run;
    public float jumpForce = 0.3f;

    private Vector3 moveVector;
    private float gravitiForce;
    private Vector3? target;
    private float allowableError = 0.1f;
    private Vector3 oldPosition;
    private float deltaDistance;
    private float turnRotation = 0.1f;
    private readonly int hashRun = Animator.StringToHash("Run");
    private bool bypass;
    private float maxBypassTime = 2f;
    private float currentBypassTime;
    private CharacterController ch_controller;

    public Vector3 MoveVector { get => moveVector; private set => moveVector = value; }

    private void Awake()
    {
        ch_controller = GetComponent<CharacterController>();
        oldPosition = transform.position;
    }

    private void FixedUpdate()
    {
        deltaDistance = (transform.position - oldPosition).magnitude;
        MoveVector = Vector3.zero;
        GoInTarget();
        CharacterMove();
        GamingGravity();
        oldPosition = transform.position;
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
        MoveVector = new Vector3(target.Value.x - transform.position.x, 0, target.Value.z - transform.position.z);
        if (MoveVector.magnitude < allowableError)
        {
            target = null;
        }
    }

    //private void Control()
    //{
    //    moveVector.x = Input.GetAxis("Horizontal");
    //    moveVector.z = Input.GetAxis("Vertical");
    //}

    private void CharacterMove()
    {
        run = MoveVector.magnitude > 0.001f;
        animator.SetBool(hashRun, run);
        MoveVector = MoveVector.normalized / 10 * speed;

        if (Vector3.Angle(Vector3.forward, MoveVector) > 1f || Vector3.Angle(Vector3.forward, MoveVector) == 0)
        {
            Vector3 direct = Vector3.RotateTowards(transform.forward, MoveVector, turnRotation, 0.0f);
            transform.rotation = Quaternion.LookRotation(direct);
        }
        MoveVector = new Vector3(MoveVector.x, gravitiForce, MoveVector.z);
        ch_controller.Move(MoveVector);
    }

    private void GamingGravity()
    {
        if (!ch_controller.isGrounded) gravitiForce -= Time.deltaTime;
        else gravitiForce = 0;
    }
}
