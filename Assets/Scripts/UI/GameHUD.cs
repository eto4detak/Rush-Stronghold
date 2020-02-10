using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class GameHUD : MonoBehaviour
{
    [Header("Images")]
    public GameObject frontImg;
    public GameObject targetImg;
    public GameObject targetList;

    [Header("Commands Buttons")]
    public GameObject btnAttack;
    public GameObject btnMove;
    public GameObject btnStop;

    [Header("UnitUI")]
    public Text damage;
    public Text armor;
    public Text speed;

    public void SelectUnit(CharacterManager target) 
    {
      //  frontImg.GetComponent<Button>().image.sprite = target.GetSprite();
      //  speed.text = target.speed.ToString();
        //if (target.command is AttackCommand)
        //{
        //    btnAttack.GetComponent<Button>().image.color = Color.red;
        //    targetImg.GetComponent<Button>().image.sprite = null;
        //    targetList.GetComponentInChildren<Text>().text = "";
        //}
    }


    public void SetTarget<T>(T target) where T : class
    {
        return;
        //if (target is UnitGroup)
        //{
        //    targetImg.GetComponent<Button>().image.sprite = Resources.Load<Sprite>("Sprite/sqareUnit");
        //    targetList.GetComponentInChildren<Text>().text = (target as UnitGroup).name;
        //}
    }


    public void ClearTarget()
    {
        return;
        targetImg.GetComponent<Button>().image.sprite = null;
        targetList.GetComponentInChildren<Text>().text = null;
    }


    public void ClearPanel()
    {
        damage.text = "";
        armor.text = "";
        speed.text = "";

        frontImg.GetComponent<Button>().image.sprite = null;
        targetImg.GetComponent<Button>().image.sprite = null;
        targetList.GetComponentInChildren<Text>().text = "";
        btnAttack.GetComponent<Button>().image.color = Color.white;
        btnMove.GetComponent<Button>().image.color = Color.white;
        btnStop.GetComponent<Button>().image.color = Color.white;
    }
}
