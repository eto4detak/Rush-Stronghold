using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPanel : MonoBehaviour
{
    public ItemInfoWeapon itemInstance;
    public GameObject itemContent;

    private List<ItemInfoWeapon> items = new List<ItemInfoWeapon>();
    private CharacterManager target;
   // private List<Spell> weapons = new List<Spell>();

    #region Singleton
    static protected WeaponPanel s_Instance;
    static public WeaponPanel instance { get { return s_Instance; } }
    #endregion

    void Awake()
    {
        if (s_Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        s_Instance = this;
    }

    public void SetTarget(CharacterManager newTarget)
    {
        gameObject.SetActive(true);
        target = newTarget;
        UpdateWeaponItems();
    }

    public void RemoveTarget()
    {
        gameObject.SetActive(false);
        target = null;
    }

    private void UpdateWeaponItems()
    {
        //for (int i = 0; i < items.Count; i++)
        //{
        //    items[i].gameObject.SetActive(false);
        //}

        //weapons = target.attack.arsenal;

        //if (weapons != null)
        //    for (int i = 0; i < weapons.Count; i++)
        //    {
        //        if (i >= items.Count)
        //        {
        //            items.Add(Instantiate(itemInstance, transform.position, Quaternion.identity, itemContent.transform));
        //        }
        //        items[i].Setup(target, weapons[i]);
        //        items[i].gameObject.SetActive(true);
        //    }
    }

}
