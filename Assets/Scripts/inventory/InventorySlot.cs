using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InventorySlot : MonoBehaviour
{

    Item currentItem;
    Image icon;

    void Awake()
    {
        icon = transform.Find("Icon").GetComponent<Image>();
    }


    //Slot button event callback
    public void OnSlotClicked()
    {

        if (currentItem != null)
        {


            //TODO : Unequip currently equipped weapon


            
            currentItem.Equip();
        }
    }

    public void FillSlot(Item newItem)
    {
        currentItem = newItem;
        icon.sprite = currentItem.defaulSprite;
        icon.enabled = true;
    }

    public void ClearSlot()
    {
        currentItem = null;
        icon.sprite = null;
        icon.enabled = false;
    }


}