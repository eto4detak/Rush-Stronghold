using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingPopup : MonoBehaviour
{
    public Button btnMute;
    public Button btnNoMute;
    public Button btnSave;
    public Slider musicSlider;

    #region Singleton
    static protected SettingPopup s_Instance;
    static public SettingPopup instance { get { return s_Instance; } }
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
    private void Start()
    {
        if (btnSave != null) btnSave.onClick.AddListener(Save);
        musicSlider.value = SaveLoad.GetInstance().pData.musicValue;
    }

    public void Show()
    {
        musicSlider.value = SaveLoad.GetInstance().pData.musicValue;
        enabled = true;
    }
    public void Hide()
    {
        enabled = false;
    }



    public void Save()
    {
        if (musicSlider != null)
        {
            SaveLoad.GetInstance().pData.musicValue = musicSlider.value;
            MusicPlayer.instance.SetVolumes(musicSlider.value);
        }
        SaveLoad.GetInstance().Save();
        Hide();
        MainMenuManager.instance.ShowMainMenu();
    }


    private void OnMute()
    {
        SaveLoad.GetInstance().pData.musicMute = true;
        SaveLoad.GetInstance().Save();
    }

    private void OnNoMute()
    {
        SaveLoad.GetInstance().pData.musicMute = false;
        SaveLoad.GetInstance().Save();
    }
}
