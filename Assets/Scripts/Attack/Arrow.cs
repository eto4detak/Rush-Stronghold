using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public Rigidbody rb;
    public float liveTime = 50f;
    private bool isDamage;
    private Vector3 oldPosition;
    private float damage;

    private void Awake()
    {
        Destroy(gameObject, liveTime);
        rb = GetComponent<Rigidbody>();
    }


    private void Update()
    {
        transform.LookAt(transform.position + transform.position  - oldPosition);
        oldPosition = transform.position;
    }
    public void Setup(float _damage)
    {
        damage = _damage;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (isDamage) return;
        isDamage = true;
        Health health = other.collider.GetComponent<Health>();
        if(health != null)  Hit(health);
        rb.velocity =Vector3.zero;
        Destroy(this);
    }

    private void Hit(Health health)
    {
        health.TakeDamage(damage);
    }

}
