using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public float currentHealth;
    public float maxHealth;
    public UnityEvent EventDie = new UnityEvent();
    public UnityEvent EventTakeDamage = new UnityEvent();
    public UnityEvent EventSetCommand = new UnityEvent();
    public float armor = 0;
    public Collider body;
    public Transform center;

    [SerializeField]
    private Team team;
    [SerializeField]
    private ParticleSystem prefabSoul;
    private bool isDie;


    public float CurrentHealth
    {
        get => currentHealth;
        set
        {
            currentHealth = value;
            
            if (currentHealth <= 0)
            {
                currentHealth = 0;
                OnDie();
            }
        }
    }
    public Team GetTeam()
    {
        return team;
    }

    public void SetTeam(Team value)
    {
        team = value;
        EventSetCommand?.Invoke();
    }


    public Transform GetCenter()
    {
        if (center != null) return center;
        return transform;
    }

    public void TakeDamage(float _damage)
    {
        float damage = _damage - armor;
        damage = damage > 0 ? damage : 0;
        CurrentHealth -= damage;
        EventTakeDamage?.Invoke();
    }

    private void OnDie()
    {
        isDie = true;
        enabled = false;
        EventDie?.Invoke();
        if (body != null) Destroy(body);
        if (prefabSoul)
        {
            ParticleSystem soul = Instantiate(prefabSoul, transform.position, Quaternion.identity);
            soul.transform.parent = null;
            ParticleSystem.MainModule mainModule = soul.main;
            mainModule.startColor = PController.instance.GetColor(team).color;
            soul.Play();
            Destroy(soul.gameObject, mainModule.duration);
        }

        Destroy(this);
    }

}
