using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class PController : MonoBehaviour
{
    #region Singleton
    static protected PController s_Instance;
    static public PController instance { get { return s_Instance; } }
    #endregion
    void Awake()
    {
        #region Singleton
        if (s_Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        s_Instance = this;
        #endregion
    }

    [Serializable]
    public struct TeamColor
    {
        public Team team;
        public Material color;
    }

    public List<CharacterManager> playerUnits = new List<CharacterManager>();
    public List<CharacterManager> enemyUnits = new List<CharacterManager>();
    public UnityEvent EventDeadPlayers = new UnityEvent();
    public Material playerMaterial;
    public Material enemyMaterial;

    private void Start()
    {
        StartUnit();
    }





    public void AttachedUnit(CharacterManager unit)
    {
        RemoveUnit(unit);
        if (unit.GetTeam() == Team.Player1)
        {
            if(!playerUnits.Exists(x => x.Equals(unit))) playerUnits.Add(unit);
        }
        else if (unit.GetTeam() == Team.Hostile)
        {
            if (!enemyUnits.Exists(x => x.Equals(unit))) enemyUnits.Add(unit);
        }
    }


    public CharacterManager GetClosestFreePlayerUnit(Vector3 target)
    {
        CharacterManager closest = null;
        float distance = Mathf.Infinity;
        float mindistance = Mathf.Infinity;
        for (int i = 0; i < playerUnits.Count; i++)
        {
            if (playerUnits[i] == null) continue;
            if (playerUnits[i].armament.isAttack) continue;
            distance = (target - playerUnits[i].transform.position).magnitude;
            if (distance < mindistance)
            {
                mindistance = distance;
                closest = playerUnits[i];
            }
        }
        return closest;
    }

    public List<CharacterManager> GetPlayerFreeUnits()
    {
        List<CharacterManager> frees = new List<CharacterManager>();
        for (int i = 0; i < playerUnits.Count; i++)
        {
            if (playerUnits[i].isFree) frees.Add(playerUnits[i]);
        }
        return frees;
    }


    public Material GetColor(Team team)
    {
        if (team == Team.Player1) return playerMaterial;
        return enemyMaterial;
    }

    private void RemoveUnit(CharacterManager removed)
    {
        if(removed.GetTeam() == Team.Player1)
        {
            playerUnits.Remove(removed);
            if (playerUnits.Count == 0) EventDeadPlayers?.Invoke();
        }
        else
        {
            enemyUnits.Remove(removed);
        }
    }
    private void StartUnit()
    {
        for (int i = 0; i < playerUnits.Count; i++)
        {
            if (playerUnits[i] != null)
                playerUnits[i].die.AddListener(RemoveUnit);

        }
        for (int i = 0; i < enemyUnits.Count; i++)
        {
            if (enemyUnits[i] != null)
                enemyUnits[i].die.AddListener(RemoveUnit);
        }
    }
}

