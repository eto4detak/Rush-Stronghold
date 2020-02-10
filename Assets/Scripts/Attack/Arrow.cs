using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float damage = 10f;
    private bool isDamage;

    private void Awake()
    {
        Destroy(gameObject, 100f);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (isDamage) return;
        isDamage = true;
        Health health = other.collider.GetComponent<Health>();
        if(health != null)  Hit(health);
        Destroy(this);
    }

    private void Hit(Health health)
    {
        health.TakeDamage(damage);
    }

}
