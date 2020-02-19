using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIUnit : MonoBehaviour
{
    protected CharacterManager owner;
    protected bool isManeuver;
    protected float timeManeuver = 1f;
    protected float currentTimeManeuver;


    protected void Awake()
    {
        owner = GetComponent<CharacterManager>();
        owner.health.EventDie.AddListener(OnDestroy);
    }

    protected void OnDestroy()
    {
        Destroy(this);
    }

    private void Start()
    {
        if (owner == null || owner.GetTeam() != Team.Hostile) enabled = false;
            
    }

    private void Update()
    {
        if (owner == null || owner.enabled == false) enabled = false;
    }


    public virtual void StartCommand()
    {
        
    }


    protected void Maneuver()
    {
        isManeuver = true;
      //  owner.command = new MoveCommand(owner, transform.position + transform.right * 2);
    }
    protected void StopManeuver()
    {
        isManeuver = false;

    }
}
