using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public sealed class Inventory : MonoBehaviour
{
    #region Singleton

    public static Inventory instance;
    //public MeshRenderer targetMesh;



    void Awake()
    {

        instance = this;

    }

    #endregion

    [SerializeField] InventoryUI inventoryUI;
    int maxSpace = 9;
    [HideInInspector] public List<Item> itemsList = new List<Item>();


    public bool AddItem(Item item)
    {

        if (itemsList.Count == maxSpace)
        {
            return false;
        }

        if (itemsList.Count == 0)
            item.Equip();

        itemsList.Add(item);
        if (inventoryUI.onItemUpdate != null)
            inventoryUI.onItemUpdate.Invoke();
        return true;
    }

    public void RemoveItem(Item item)
    {
        itemsList.Remove(item);
        inventoryUI.onItemUpdate.Invoke();
    }


}