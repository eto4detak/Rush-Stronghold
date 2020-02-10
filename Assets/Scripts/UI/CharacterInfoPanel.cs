using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class CharacterInfoPanel : MonoBehaviour
{
    public GameObject frontImg;
    public Slider Health;
    public Scrollbar Morale;
    public Health target;


    public void Setup(Health newTarget)
    {
        target = newTarget;
       // frontImg.GetComponent<Image>().sprite = target.fillImage;
    }


}
