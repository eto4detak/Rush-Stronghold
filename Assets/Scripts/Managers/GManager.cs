using System;
using System.Collections.Generic;
using UnityEngine;

public class GManager : MonoBehaviour
{
    public static float startPositionY = 5.5f;
    public static float deltaPositionY = 0.1f;
    public static float minPositionY = -0.1f;
    public Transform spawnPoint;

    public List<Transform> mission = new List<Transform>();
    public List<CharacterManager> units;

    void Start()
    {

        PlayerAttack();
        Level.instance.StartLevel();
    }


    private void PlayerAttack()
    {
        for (int i = 0; i < units.Count; i++)
        {
            //if(units[i])
            //units[i].mission = new List<Transform>( mission );
        }
    }


}