using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System;


public class InventoryUI : MonoBehaviour
{



    [SerializeField] GameObject inventoryPanel;
    public UnityEvent onItemUpdate;
    InventorySlot[] slots;

    void Start()
    {

        slots = FindObjectsOfType<InventorySlot>();
        inventoryPanel.SetActive(false);

        if (onItemUpdate == null)
            onItemUpdate = new UnityEvent();
        onItemUpdate.AddListener(UpdateUI);
    }

    public void OnInventoryBtnClicked()
    {
        inventoryPanel.SetActive(!inventoryPanel.activeSelf);

    }


    public void UpdateUI()
    {

        
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < Inventory.instance.itemsList.Count)
            {

                slots[i].FillSlot(Inventory.instance.itemsList[i]);

            }
            else
            {
                //Otherwise 
                slots[i].ClearSlot();
            }

        }
    }

}