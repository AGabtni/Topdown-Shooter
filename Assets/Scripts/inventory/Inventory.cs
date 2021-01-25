using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public sealed class Inventory : GenericSingletonClass<Inventory>
{
  

    [SerializeField] InventoryUI inventoryUI;
    int maxSpace = 9;
    [HideInInspector] public List<Item> itemsList = new List<Item>();


    public bool AddItem(Item item)
    {

        if (itemsList.Count == maxSpace)
        {
            return false;
        }

        if (itemsList.Count == 0 && item.itemType == ItemType.Weapon && !EquipmentManager.instance.IsWeaponEquipped() )
        {
            item.Equip();
            return true;
        }

        itemsList.Add(item);
        if (inventoryUI.onItemUpdate != null)
            inventoryUI.onItemUpdate.Invoke();


        return true;
    }

    public void RemoveItem(Item item)
    {
        itemsList.Remove(item);

        if (inventoryUI.onItemUpdate != null)
            inventoryUI.onItemUpdate.Invoke();
    }


}