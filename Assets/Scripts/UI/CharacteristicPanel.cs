using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacteristicPanel : MonoBehaviour
{
    public SpriteRenderer prefabItem;
    public Characteristic owner;
    private List<ItemPanel> items = new List<ItemPanel>(); 

    public struct ItemPanel
    {
        public Feature feature;
        public GameObject item;
    }

    private void Start()
    {
       // owner.OnAddFeature += CreateItem;
        //owner.OnRemoveFeature += RemoveItem;
    }


    //private void OnDestroy()
    //{
    //    owner.OnAddFeature -= CreateItem;
    //    owner.OnRemoveFeature -= RemoveItem;
    //}

    private void RemoveItem(Feature deletedFature)
    {
        //ItemPanel deaded = items.Find(x => x.feature == deletedFature);
        //items.Remove(deaded);
        //Destroy(deaded.item);
    }

    private void CreateItem(Feature addedFeature)
    {
        var instance =  Instantiate(prefabItem, transform.position,transform.rotation, transform);
        instance.gameObject.SetActive(true);
       // instance.sprite = addedFeature.data.icon;
        items.Add(new ItemPanel() {feature = addedFeature, item = instance.gameObject });
    }

}
