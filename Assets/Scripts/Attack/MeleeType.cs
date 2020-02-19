using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeType : AttackType
{
    public override void Attack(Health enemy, Health owner)
    {
        enemy.TakeDamage(damage);
    }

    //private float CalculdateDamage(Health enemy)
    //{
    //    float totalDamage = damage - enemy.armor;
    //    totalDamage = totalDamage > 0 ? totalDamage : 0;
    //    return totalDamage;
    //}

}