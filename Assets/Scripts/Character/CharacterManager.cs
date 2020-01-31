using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour, IMovable
{
    public IMovable movement;
    public CharacterAttack armament;
    public CharacterCommand command;
    public Animator animator;
    public Health health;
    private readonly int hashDieAnim = Animator.StringToHash("Die");
    private void Awake()
    {
        IMovable m = GetComponent<CharacterNavMovement>();
        if(m != null) movement = m;
        else
        {
            movement = GetComponent<CharacterContrMovement>();
        }
    }


    private void Start()
    {
        health.EventDie.AddListener(Die);
    }


    private void Die()
    {
        GetComponent<CharacterController>().enabled = false;
        health.enabled = false;
        armament.enabled = false;
        enabled = false;
        animator.SetBool(hashDieAnim, true);
    }


    private void Update()
    {
        //  command.DoCommand();
       // armament.Attack();
    }

    public void SetTargetMovement(Transform target)
    {
        movement.SetTargetMovement(target);
    }
    public void SetTargetMovement(Vector3 target)
    {
        movement.SetTargetMovement(target);
    }
    public void SetSpeed(float _speed)
    {
        movement.SetSpeed(_speed);
    }
}
