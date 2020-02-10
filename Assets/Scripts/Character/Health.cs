using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public float currentHealth;
    public float maxHealth;
    public UnityEvent EventDie = new UnityEvent();
    public UnityEvent TakeDamageEvent = new UnityEvent();
    public UnityEvent EventSetCommand = new UnityEvent();
    public List<Renderer> forColor;
    public float armor = 0;
    public Collider body;
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


    private void Start()
    {
        SetTeam(team);
    }


    public Team GetTeam()
    {
        return team;
    }

    public void SetTeam(Team value)
    {
        if(value == Team.Player1)  ChangeColor(PController.instance.playerMaterial);
        else  ChangeColor(PController.instance.enemyMaterial);
        team = value;
        EventSetCommand?.Invoke();
    }


    private void ChangeColor(Material newColor)
    {
        for (int i = 0; i < forColor.Count; i++)
        {
            forColor[i].material = newColor;
        }
    }


    public void TakeDamage(float damage)
    {
        CurrentHealth -= damage;
        TakeDamageEvent?.Invoke();
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
