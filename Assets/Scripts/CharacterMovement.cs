using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class CharacterMovement : MonoBehaviour, IMovable
{
    public Animator animator;
    public NavMeshAgent agent;
    public Rigidbody rb;
    public Transform target;

    public float speed = 1f;
    public float turnSpeed = 0.1f;
    private Vector3 destinationPosition;
    private bool handleInput = true;
    private WaitForSeconds inputHoldWait;
    private readonly int hashSpeedPara = Animator.StringToHash("Speed");
    private const float stopDistanceProportion = 0.1f;
    private const float navMeshSampleDistance = 4f;

    private Vector3 oldPosition;
    private Vector3 moveVector;

    private void FixedUpdate()
    {
        float currentSpeed = (transform.position - oldPosition).magnitude / Time.deltaTime;
        animator.SetFloat(hashSpeedPara, currentSpeed);

        if (target)
        {
            moveVector = target.position - transform.position;
            if (Mathf.Abs(moveVector.x + moveVector.z) < 0.1f)
            {
                animator.SetFloat(hashSpeedPara, 0);
            }
            else
            {
                Move();
                Turn();
            }
        }

        oldPosition = transform.position;
    }
    public void MoveTo(Transform newtarget)
    {
        target = newtarget;
    }
    public void MoveTo(Vector3 target)
    {

    }
    public void SetSpeed(float speed)
    {

    }

    public void Stop()
    {

    }

    private void Move()
    {
        Vector3 movement = transform.forward  * speed * Time.deltaTime;
        rb.MovePosition(rb.position + movement);
    }

    private void Turn()
    {
        Vector3 direct = Vector3.RotateTowards(transform.forward, moveVector, 0.1f, 0);
        transform.rotation = Quaternion.LookRotation(direct);
    }


}
