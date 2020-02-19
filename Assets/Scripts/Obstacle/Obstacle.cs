using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public Health health;
    public Animator animator;
    public GameObject finishObject;

    private readonly int hashDie = Animator.StringToHash("Die");
    private readonly int hashTakeDamage = Animator.StringToHash("TakeDamage");

    private void Start()
    {
        health?.EventDie.AddListener(Die);
        health?.EventTakeDamage.AddListener(TakeDamage);
    }

    private void Die()
    {
        if (animator != null) animator.SetTrigger(hashDie);
        //if(animator != null) animator?.SetBool(hashAttackPara, true);
        if (finishObject != null) finishObject.SetActive(true);
        Destroy(gameObject);
    }

    private void TakeDamage()
    {
        if (animator != null) animator?.SetTrigger(hashTakeDamage);
    }

}
