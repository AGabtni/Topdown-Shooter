using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Item : ScriptableObject
{

    //public string itemName;
    public Sprite defaulSprite;
    public Transform itemPrefab ;

    public virtual void Equip(){
        
    }

    public virtual void UnEquip(){
        
    }
    
    public virtual void Use() {
        

    }








}
