using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unions : MonoBehaviour
{
    [Serializable]
    public enum p_union
    {
        Allies,
        Enemies,
        Neitrals,
    }
    [Serializable]
    public struct p_unions
    {
        public string Name;
        public Team Team1;
        public p_union Union;
        public Team Team2;
    }

    public List<p_unions> _Unions = new List<p_unions>();

    #region Singleton
    static protected Unions s_Instance;
    static public Unions instance { get { return s_Instance; } }
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

    public bool CheckEnemies(Team _team1, Team _team2)
    {
        for (int i = 0; i < _Unions.Count; i++)
        {
            if ((_Unions[i].Team1.Equals(_team1) && _Unions[i].Team2.Equals(_team2))
                || (_Unions[i].Team1.Equals(_team2) && _Unions[i].Team2.Equals(_team1)))
            {
                switch (_Unions[i].Union)
                {
                    case p_union.Enemies:
                        return true;
                }
            }
        }
        return false;
    }
    public static bool CheckAllies(Team _team1, Team _team2)
    {
        if (_team1 == _team2) return true;
        return false;
    }
}
