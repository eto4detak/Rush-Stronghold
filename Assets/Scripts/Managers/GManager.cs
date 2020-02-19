using System;
using System.Collections.Generic;
using UnityEngine;

public class GManager : MonoBehaviour
{
    public static float startPositionY = 5.5f;
    public static float deltaPositionY = 0.1f;
    public static float minPositionY = -0.1f;
    void Start()
    {
        Level.instance.StartLevel();
    }

}