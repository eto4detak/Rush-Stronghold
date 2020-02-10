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
    public override void FinishLevel()
    {
        LevelManager.instance.LoadNextLevel();
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
        LevelManager.instance.RestartLevel();
    }


    public override string Resulttext()
    {
        return "Game Over";
    }

}
