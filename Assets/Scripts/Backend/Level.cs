using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Level : MonoBehaviour
{
    public float startDelay = 3f;
    public float endDelay = 3f;
    public GameObject[] spawnPoints;
    public Text messageText;
    public List<VictoryPoint> victoryPoints;

    private int levelNumber = 0;
    private WaitForSeconds startWait;
    private WaitForSeconds endWait;
    private GameObject roundWinner;
    private GameObject gameWinner;
    private GameFinish levelResultat;
    
    //private LevelRecord records;
    #region Singleton
    static protected Level s_Instance;
    static public Level instance { get { return s_Instance; } }
    #endregion

    private void Awake()
    {
        #region Singleton
        if (s_Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        s_Instance = this;
        #endregion
        startWait = new WaitForSeconds(startDelay);
        endWait = new WaitForSeconds(endDelay);
       // records = new LevelRecord();
    }


    public void StartLevel()
    {
        levelNumber = LevelManager.instance.levelData.levelNumber;
        for (int i = 0; i < victoryPoints.Count; i++)
        {
            victoryPoints[i].Capture.AddListener(CheckGameWin);
        }
        PController.instance.EventDeadPlayers.AddListener(SetGameOver);
        StartCoroutine(GameLoop());
    }

    private IEnumerator GameLoop()
    {
        yield return StartCoroutine(LevelStarting());
        yield return StartCoroutine(LevelPlaying());
        yield return StartCoroutine(LevelEnding());
    }

    private IEnumerator LevelStarting()
    {
        //MouseManager.instance.enabled = false;
        for (int i = 0; i < PController.instance.enemyUnits.Count; i++)
        {
            PController.instance.enemyUnits[i].DesableUnit();
        }
        for (int i = 0; i < PController.instance.playerUnits.Count; i++)
        {
            PController.instance.playerUnits[i].DesableUnit();
        }
        MainMenuManager.instance.HideMainMenu();
        ShowMessage("Level " + levelNumber);
        yield return startWait;
    }

    private IEnumerator LevelPlaying()
    {
        for (int i = 0; i < PController.instance.enemyUnits.Count; i++)
        {
            PController.instance.enemyUnits[i].EnableUnit();
        }
        for (int i = 0; i < PController.instance.playerUnits.Count; i++)
        {
            PController.instance.playerUnits[i].EnableUnit();
        }
        MouseManager.instance.enabled = true;
        MissionManager.instance.SetStartingEnemyTarget(PController.instance.enemyUnits);
        HiddenMessage(string.Empty);
        while (levelResultat == null)
        {
           /// MissionManager.instance.SetStartingPlayerTarget(PController.instance.GetPlayerFreeUnits());
            yield return null;
        }
    }

    private IEnumerator LevelEnding()
    {
        for (int i = 0; i < PController.instance.enemyUnits.Count; i++)
        {
            PController.instance.enemyUnits[i].command = new StopCommand(PController.instance.enemyUnits[i]);
        }
        for (int i = 0; i < PController.instance.playerUnits.Count; i++)
        {
            PController.instance.playerUnits[i].command = new StopCommand(PController.instance.playerUnits[i]);
        }
        ShowMessage(levelResultat.Resulttext());
        yield return endWait;
        levelResultat.FinishLevel();
    }

    private string EndMessage()
    {
        string message = "Winner!";
        //if (m_RoundWinner != null)
        //    message = m_RoundWinner.m_ColoredPlayerText + " WINS THE ROUND!";
        message += "\n\n\n\n";
        //for (int i = 0; i < records.recordList.Count; i++)
        //{
        //    message += records.recordList[i] + " \n";
        //}
        //if (m_GameWinner != null)
        //    message = m_GameWinner.m_ColoredPlayerText + " WINS THE GAME!";

        return message;
    }

    private void EnableVictoryPoints()
    {
        for (int i = 0; i < victoryPoints.Count; i++)
        {
            victoryPoints[i].enabled = true;
        }
    }

    private void DisableSportsmanPath()
    {
        //for (int i = 0; i < playerUnits.Count; i++)
        //{
        //    //sportsmans[i].mission.isFinish = false;
        //    //sportsmans[i].mission.currentIndexPoint = 0;
        //    //sportsmans[i].mission.path = new List<MissionLine>();
        //}
    }

    private void SetGameOver()
    {
        if (levelResultat == null)
            levelResultat = new GameOver();
    }

    private void CheckGameWin()
    {
        for (int i = 0; i < victoryPoints.Count; i++)
        {
            if (victoryPoints[i].isCapture == false) return;
        }
        SetGameWin();
    }

    private void SetGameWin()
    {
        if(levelResultat == null) levelResultat = new GameWin();
    }

    private void ShowMessage(string text)
    {
        messageText.gameObject.SetActive(true);
        // messageText.text = "Level " + LevelManager.instance.levelData.levelNumber;
        messageText.text = text;
    }
    private void HiddenMessage(string text)
    {
        messageText.gameObject.SetActive(false);
        messageText.text = text;
    }
}
