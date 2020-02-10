using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    public float damage = 1f;

    //3d
    private void OnTriggerStay(Collider other)
    {
        Health health =  other.GetComponent<Health>();
        if(health != null) Damage(health);
    }

    private void Damage(Health health)
    {
        health.TakeDamage(damage);
    }
}
