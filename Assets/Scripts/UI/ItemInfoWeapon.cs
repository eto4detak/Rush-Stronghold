using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemInfoWeapon : MonoBehaviour
{
    public Button btnFront;
    public Text textName;
    public Text count;

    //private ISpell weapon;
    //private CharacterManager owner;
    //public void Setup(CharacterManager _owner, ISpell _weapon)
    //{
    //    owner = _owner;
    //    weapon = _weapon;
    //    var data = weapon.GetData();

    //    textName.text = data.featureName.ToString();
    //    btnFront.image.sprite = data.icon;
    //    btnFront.onClick.RemoveAllListeners();
    //    btnFront.onClick.AddListener(OnClickSelectWeapon);
    //}


    //private void OnClickSelectWeapon()
    //{
    //    owner.attack.SelectSpell(weapon);
    //}
}
