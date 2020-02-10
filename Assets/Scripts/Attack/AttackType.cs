using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackType : MonoBehaviour
{
    public float damage;

    public virtual void Attack(Health enemy, Health owner)
    {
        enemy.TakeDamage(damage);
    }
}
