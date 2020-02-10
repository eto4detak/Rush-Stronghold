using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAttack : MonoBehaviour, IAttack
{
    public bool isAttack = false;
    public Animator animator;
    public AttackType attackType;

    private readonly int hashAttackPara = Animator.StringToHash("Attack");
    private Health target;
    private float rotSpeed = 5f;
    private CharacterManager owner;

    private void Awake()
    {
        owner = GetComponent<CharacterManager>();
    }

    private void Update()
    {
        if (target)
        {
            Attacking();
        }
    }


    public void NoAttack()
    {
        target = null;
        isAttack = false;
        animator.SetBool(hashAttackPara, isAttack);
    }

    public void Attack(Health newTarget)
    {
        target = newTarget;
        isAttack = true;
    }

    public void EventAttack()
    {
        if (target)
        {
            attackType.Attack(target, owner.health);
        }
        else
        {
            Debug.Log("else EventAttack");
        }
    }

    public void SetTarget(Health newTarget)
    {
        target = newTarget;
    }

    private void LookAtTarget()
    {
        Vector3 attackDirection = target.transform.position - transform.position;
        transform.rotation = Quaternion.Slerp(transform.rotation,
            Quaternion.LookRotation( new Vector3(attackDirection.x, 0, attackDirection.z)), rotSpeed * Time.deltaTime);
    }

    private void Attacking()
    {
        animator.SetBool(hashAttackPara, isAttack);
        LookAtTarget();
    }

}
