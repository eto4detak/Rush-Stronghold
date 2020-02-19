using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Level : MonoBehaviour
{
    public float startDelay = 1f;
    public float endDelay = 3f;
    public TextMeshProUGUI messageText;
    public List<VictoryPoint> victoryPoints;
    public bool startGame;
    public bool restartGame;
    public bool continueGame;

    private int levelNumber = 0;
    private WaitForSeconds pausetWait;
    private WaitForSeconds oneWait;
    private WaitForSeconds endWait;
    private GameObject roundWinner;
    private GameObject gameWinner;
    private GameFinish levelResultat;
    
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
        pausetWait = new WaitForSeconds(startDelay);
        oneWait = new WaitForSeconds(1f);
        endWait = new WaitForSeconds(endDelay);
    }
    
    public void StartLevel()
    {
        levelNumber = LevelManager.instance.levelData.levelNumber;
        for (int i = 0; i < victoryPoints.Count; i++)
        {
            victoryPoints[i].Capture.AddListener(CheckGame);
        }
        StartCoroutine(GameLoop());
    }

    private IEnumerator GameLoop()
    {
        yield return StartCoroutine(LevelPausing());
        yield return StartCoroutine(LevelStarting());
        yield return StartCoroutine(LevelPlaying());
        yield return StartCoroutine(LevelEnding());
    }
    private IEnumerator LevelPausing()
    {
        GameHUD.instance.ViewBtnRestart(false);
        GMode.instance.ContinueGame();
        GameHUD.instance.ViewLvlLabel(true);
        CharacterManager[] allUnits = Resources.FindObjectsOfTypeAll<CharacterManager>();
        for (int i = 0; i < allUnits.Length; i++)
        {
            allUnits[i].DesableUnit();
        }
        MainMenuManager.instance.HideMainMenu();
        startGame = false;
        while (!startGame)
        {
            
            yield return null;
        }
    }

    private IEnumerator LevelStarting()
    {
        //for (int i = 0; i < startDelay; i++)
        //{
        //    ShowMessage("Starting " + ((int)startDelay - i).ToString());
        //    yield return oneWait;
        //}
        yield return null;
        List<CharacterManager> allUnits = new List<CharacterManager>(Resources.FindObjectsOfTypeAll<CharacterManager>());
        allUnits.RemoveAll(x => x.gameObject.activeSelf == false);
        SpawnPoint[] allSpawn = Resources.FindObjectsOfTypeAll<SpawnPoint>();
        for (int i = 0; i < allSpawn.Length; i++)
        {
            allSpawn[i].SetLoop(true);
        }
        AIUnit tempAI;
        for (int i = 0; i < allUnits.Count; i++)
        {
            allUnits[i].EnableUnit();
        }
        List<CharacterManager> list = new List<CharacterManager>(PController.instance.playerUnits);
        list.RemoveAll(x => x.Command != null);
        MissionManager.instance.SetStartinпTarget(list);
        MissionManager.instance.SetStartinпTarget(new List<CharacterManager>(PController.instance.enemyUnits));
        MouseManager.instance.enabled = true;

        for (int i = 0; i < allUnits.Count; i++)
        {
            tempAI = allUnits[i].GetComponent<AIUnit>();
            if (tempAI != null) tempAI.StartCommand();
        }
        HiddenMessage(string.Empty);
        GameHUD.instance.ViewLvlLabel(false);
    }

    private IEnumerator LevelPlaying()
    {
        MusicPlayer.instance.StopFirstSound();
        while (levelResultat == null)
        {
            CheckGame();
            yield return null;
        }
    }

    private IEnumerator LevelEnding()
    {
        SpawnPoint[] allSpawn = Resources.FindObjectsOfTypeAll<SpawnPoint>();
        for (int i = 0; i < allSpawn.Length; i++)
        {
            allSpawn[i].SetLoop(false);
        }
        for (int i = 0; i < PController.instance.enemyUnits.Count; i++)
        {
            PController.instance.enemyUnits[i].Command = new StopCommand(PController.instance.enemyUnits[i]);
        }
        for (int i = 0; i < PController.instance.playerUnits.Count; i++)
        {
            PController.instance.playerUnits[i].Command = new StopCommand(PController.instance.playerUnits[i]);
        }
        levelResultat.FinishLevel();
        restartGame = false;
        while (true)
        {
            if (restartGame)
            {
                LevelManager.instance.RestartLevel();
                yield break;
            }
            if (continueGame)
            {
                LevelManager.instance.LoadNextLevel();
                yield break;
            }
            yield return null;
        }
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

    private void CheckGame()
    {
        if(PController.instance.enemyUnits.Count == 0)
        {
            SetGameWin();
        }
        else if(PController.instance.playerUnits.Count == 0)
        {
            SetGameOver();
        }
    }

    private void SetGameWin()
    {
        if(levelResultat == null) levelResultat = new GameWin(PController.instance.playerUnits.Count);
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
