using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterColor : MonoBehaviour
{

    [Serializable]
    public struct p_Skin
    {
        public Team Team;
        public Material skin;
    }

    public List<Renderer> forColor;
    public Renderer oneSkin;

    public List<p_Skin> skins = new List<p_Skin>();

    private void Start()
    {
        SetColor(GetComponent<Health>().GetTeam());
    }
    
    public void SetColor(Team team)
    {
        ChangeColor(PController.instance.GetColor(team));
        ChangeOneSkin(team);
    }

    private void ChangeColor(Material newColor)
    {
        if(forColor != null)
        {
            for (int i = 0; i < forColor.Count; i++)
            {
                forColor[i].material = newColor;
            }
        }
    }

    private void ChangeOneSkin(Team team)
    {
        if (oneSkin != null)
        {
            int index = skins.FindIndex(x => x.Team == team);
            if ( index != -1)
            {
                oneSkin.material = skins[index].skin;
            }
        }
    }

}


