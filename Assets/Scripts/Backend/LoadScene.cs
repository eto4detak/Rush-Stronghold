using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadScene : MonoBehaviour
{
    void Start()
    {
        int level = SaveLoad.GetInstance().pData.lastLevel;
        level = level > 0 ? level : 0;
        LevelManager.instance.LoadLevel(level);
    }
}
