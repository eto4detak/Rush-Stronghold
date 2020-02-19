using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInfo : MonoBehaviour
{
    public ParticleSystem damageEffectPrefab;
    public GameObject info;

    private ParticleSystem psDamage;
    private Health owner;
    void Start()
    {
        if (damageEffectPrefab == null) return;
        psDamage = Instantiate(damageEffectPrefab, transform.position, transform.rotation, transform);

        owner = GetComponent<Health>();
        if (owner != null) owner.EventTakeDamage.AddListener(StartDamage);
    }

    private void StartDamage()
    {
        if (!psDamage.isPlaying) psDamage.Play();
    }

}
