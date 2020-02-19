using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionManager : MonoBehaviour
{
    public Transform target;
    
    #region Singleton
    static protected MissionManager s_Instance;
    static public MissionManager instance { get { return s_Instance; } }
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


    public void SetStartingPlayerTarget(List<CharacterManager> freeUnits)
    {
        for (int i = 0; i < freeUnits.Count; i++)
        {
            Vector3 direction = target.position - freeUnits[i].transform.position ;
            direction.y = 0;
            if (direction.magnitude > 1f)
            {
                freeUnits[i].Command = new RushCommand(freeUnits[i], target.position );
            }
        }
    }
    public void SetStartinпTarget(List<CharacterManager> units)
    {
        for (int i = 0; i < units.Count; i++)
        {
           units[i].Command = new GuardCommand(units[i]);
            //CharacterManager enemy =  UnityExtension.GetClosest(units[i].transform, PController.instance.playerUnits)
            //    as CharacterManager;
            //units[i].command = new AttackCommand(units[i], enemy);
        }
    }

    public void DisableUnits(List<CharacterManager> units)
    {
        for (int i = 0; i < units.Count; i++)
        {
            units[i].Command = new StopCommand(units[i]);
            units[i].gameObject.SetActive(false);
        }
    }
}
