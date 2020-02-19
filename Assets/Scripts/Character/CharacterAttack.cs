using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAttack : MonoBehaviour, IAttack
{
    public bool isAttack = false;
    public Animator animator;
    private readonly int hashAttackPara = Animator.StringToHash("Attack");
    private Health target;
    private float rotSpeed = 5f;
    private CharacterManager owner;

    public AttackType AttackType { get; private set; }
    public Health Target { get => target; private set => target = value; }

    private void Awake()
    {
        owner = GetComponent<CharacterManager>();
        AttackType = GetComponent<AttackType>();
    }

    private void Update()
    {
        if (Target)
        {
            Attacking();
        }
    }


    public void NoAttack()
    {
        Target = null;
        isAttack = false;
        animator.SetBool(hashAttackPara, isAttack);
    }

    public void Attack(Health newTarget)
    {
        Target = newTarget;
        isAttack = true;
    }

    public void EventAttack()
    {
        if (Target)
        {
            AttackType.Attack(Target, owner.health);
        }
    }

    public void SetTarget(Health newTarget)
    {
        Target = newTarget;
    }

    private void LookAtTarget()
    {
        Vector3 attackDirection = Target.transform.position - transform.position;
        transform.rotation = Quaternion.Slerp(transform.rotation,
            Quaternion.LookRotation( new Vector3(attackDirection.x, 0, attackDirection.z)), rotSpeed * Time.deltaTime);
    }

    private void Attacking()
    {
        animator.SetBool(hashAttackPara, isAttack);
        LookAtTarget();
    }

}
