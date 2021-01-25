using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType{

    Weapon,
    Health,
    TimerAddOn
}

public class Item : ScriptableObject
{

    //public string itemName;
    public Sprite defaulSprite;
    public Transform itemPrefab;
    public ItemType itemType;
    public virtual void Equip()
    {

    }

    public virtual void UnEquip()
    {

    }

    public virtual void Use()
    {


    }








}
