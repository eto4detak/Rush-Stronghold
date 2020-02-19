using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private Button buttonPref;
    [SerializeField] private GameObject levelPanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject contentLevelPanel;
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private TextMeshProUGUI levelTitle;
    [SerializeField] private Button btnPause;
    [SerializeField] private Button btnContinue;
    [SerializeField] private Button btnRestart;
    [SerializeField] private Button btnLevels;
    [SerializeField] private Button btnExit;
    [SerializeField] private Button btnBack;
    [SerializeField] private Button btnSettings;


    #region Singleton
    static protected MainMenuManager s_Instance;
    static public MainMenuManager instance { get { return s_Instance; } }
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

    void Start()
    {
        btnPause.onClick.AddListener(ShowMainMenu);
        btnContinue.onClick.AddListener(OnContinueGame);
        btnRestart.onClick.AddListener(OnRestartLevel);
        btnLevels.onClick.AddListener(OnOpenLevelPanel);
        btnSettings.onClick.AddListener(OnOpenSettingsPanel);
        btnExit.onClick.AddListener(OnExitGame);
        btnBack.onClick.AddListener(ShowMainMenu);
        SetTitleLevel();
        if (levelPanel) levelPanel.SetActive(false);
        SetupLevelsInPanel();
        SaveCurrentLevel();
    }

    public void ShowMainMenu()
    {
        GMode.instance.PauseGame();
        btnPause.gameObject.SetActive(false);
        menuPanel.gameObject.SetActive(true);
        if (levelPanel) levelPanel.SetActive(false);
        if (settingsPanel) settingsPanel.SetActive(false);
        if (mainPanel) mainPanel.SetActive(true);

    }
    public void HideMainMenu()
    {
        menuPanel.gameObject.SetActive(false);
        btnPause.gameObject.SetActive(true);
    }

    public void OnOpenLevelPanel()
    {
        if (levelPanel) levelPanel.SetActive(true);
        if (mainPanel) mainPanel.SetActive(false);
    }
    public void OnOpenSettingsPanel()
    {
        if (settingsPanel) settingsPanel.SetActive(true);
        if (mainPanel) mainPanel.SetActive(false);
    }

    public void OnContinueGame()
    {
        bool startScreen = Level.instance == null;
        if (startScreen)
        {
            SaveLoad.GetInstance().Load();
            int lvlNumber = SaveLoad.GetInstance().pData.lastLevel > 0 ? SaveLoad.GetInstance().pData.lastLevel : 1;
            LevelManager.instance.LoadLevel(lvlNumber);
        }
        else
        {
            HideMainMenu();
            GMode.instance.ContinueGame();
        }
    }

    public void OnExitGame()
    {
        Application.Quit();
    }

    public void OnRestartLevel()
    {
        LevelManager.instance.RestartLevel();
        //int level = SceneManager.GetActiveScene().buildIndex;
        //level = level > 0 ? level : 1;
        //SceneManager.LoadScene(level);
    }


    private void SaveCurrentLevel()
    {
        if (Level.instance != null)
        {
            SaveLoad.GetInstance().Load();
            SaveLoad.GetInstance().pData.lastLevel = LevelManager.instance.levelData.levelNumber;
            SaveLoad.GetInstance().Save(SaveLoad.GetInstance().pData);
        }
    }

    private void SetupLevelsInPanel()
    {
        int lvlCount = SaveLoad.GetInstance().pData.maxLevel;
        for (int i = 1; i < lvlCount+1; i++)
        {
            Button btnLevel = Instantiate(buttonPref, contentLevelPanel.transform);
            btnLevel.gameObject.SetActive(true);
            int lvl = i;
            btnLevel.GetComponentInChildren<TextMeshProUGUI>().text = "Level " + lvl.ToString();
            btnLevel.name = "" + i;
            btnLevel.onClick.AddListener(() => LoadLevel(btnLevel.name));
        }
    }

    private void LoadLevel(string lvl)
    {
        LevelManager.instance.LoadLevel(Int32.Parse(lvl));
    }

    private void SetTitleLevel()
    {
        //int level = LevelManager.instance.levelData.levelNumber;
        //if(level < 1)
        //{
        //    SaveLoad.GetInstance().Load();
        //    level = 0;
        //}
        //levelTitle.text = "Level " + level;
    }
}
