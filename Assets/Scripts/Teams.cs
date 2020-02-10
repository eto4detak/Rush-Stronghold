using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Team
{
    Player1,
    Player2,
    Neitral,
    Hostile,
}

public class Unions
{


    public static bool CheckEnemies(Team _team1, Team _team2)
    {
        if (_team1 != _team2) return true;
        return false;
    }
    public static bool CheckAllies(Team _team1, Team _team2)
    {
        if (_team1 == _team2) return true;
        return false;
    }
}