using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public SaveLoad saved;
    public LevelData levelData;
    
    #region Singleton
    static protected LevelManager s_Instance;
    static public LevelManager instance { get { return s_Instance; } }
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
        DontDestroyOnLoad(gameObject);
        #endregion
        saved = SaveLoad.GetInstance();
        levelData = GetDataLevel(0);
    }


    public void LoadLevel(int levelNumber)
    {
        levelData = GetDataLevel(levelNumber);
        SceneManager.LoadScene(levelData.sceneNumber);
    }
    public void LoadNextLevel()
    {
        LoadLevel(levelData.levelNumber+1);
    }

    public void RestartLevel()
    {
        if(levelData != null)
         LoadLevel(levelData.levelNumber);
    }

    public LevelData GetDataLevel(int levelNumber)
    {
        if (levelNumber == 0)
        {
            return new LevelData()
            {
                levelNumber = 0,
                sceneNumber = 0,
            };
        }
        else if (levelNumber == 1)
        {
            return new LevelData()
            {
                levelNumber = 1,
                sceneNumber = 1,
            };
        }
        else if (levelNumber == 2)
        {
            return new LevelData()
            {
                levelNumber = 2,
                sceneNumber = 1,
            };
        }
        else
        {
            return new LevelData()
            {
                levelNumber = 3,
                sceneNumber = 1,
            };
        }
    }

}

public class LevelData
{
    public int sceneNumber;
    public int levelNumber;
    public int partyCount;
    public int roundCount;
}
