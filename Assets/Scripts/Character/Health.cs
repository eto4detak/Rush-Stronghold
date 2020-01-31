using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [SerializeField]
    private float currentHealth;
    public float maxHealth;
    public UnityEvent EventDie = new UnityEvent();
    private bool isDie;

    public float CurrentHealth
    {
        get => currentHealth;
        set
        {
            currentHealth = value;
            if (currentHealth < 0)
            {
                currentHealth = 0;
                OnDie();
            }
        }
    }

    public void TakeDamage(float damage)
    {
        CurrentHealth -= damage;
    }

    private void OnDie()
    {
        isDie = true;
        enabled = false;
        EventDie?.Invoke();
    }


}
