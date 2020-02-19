using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameFinish
{
    public virtual void FinishLevel()
    {
            
    }

    public virtual string Resulttext()
    {
        return "";
    }

}
public class GameWin : GameFinish
{
    public int star;

    public GameWin(int _star = 0)
    {
        star = _star;
        star = star > 0 ? star : 0;
    }

    public override void FinishLevel()
    {
        MusicPlayer.instance.PlayWinSound();
        SaveLoad.GetInstance().Load();
        SaveLoad.GetInstance().pData.star += star;
        SaveLoad.GetInstance().Save();
        SaveLoad.GetInstance().Load();
        GameHUD.instance.SetTotalStar(SaveLoad.GetInstance().pData.star);
        GameHUD.instance.btnContinue.gameObject.SetActive(true);
        //   LevelManager.instance.LoadNextLevel();

    }
    public override string Resulttext()
    {
        return "Game Win";
    }
}

public class GameOver : GameFinish
{
    public override void FinishLevel()
    {
        MusicPlayer.instance.PlayOverGameSound();
        GameHUD.instance.ViewBtnRestart(true);

        //  LevelManager.instance.RestartLevel();

    }


    public override string Resulttext()
    {
        return "Game Over";
    }

}
