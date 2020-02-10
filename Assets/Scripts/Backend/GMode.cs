using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GMode : MonoBehaviour
{
    private bool isPause;
    #region Singleton
    static protected GMode s_Instance;
    static public GMode instance { get { return s_Instance; } }
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

    public void PauseGame()
    {
        isPause = true;
        Time.timeScale = 0;
    }

    public void ContinueGame()
    {
        isPause = false;
        Time.timeScale = 1;
    }
}
