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

        slots = inventoryPanel.GetComponentsInChildren<InventorySlot>();
        inventoryPanel.SetActive(false);

        if (onItemUpdate == null)
            onItemUpdate = new UnityEvent();
        onItemUpdate.AddListener(UpdateUI);
        UpdateUI();
    }

    public void OnInventoryBtnClicked()
    {
        inventoryPanel.SetActive(!inventoryPanel.activeSelf);

    }


    public void UpdateUI()
    {

        
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < Inventory.Instance.itemsList.Count)
            {

                slots[i].FillSlot(Inventory.Instance.itemsList[i]);

            }
            else
            {
                //Otherwise 
                slots[i].ClearSlot();
            }

        }
    }

}