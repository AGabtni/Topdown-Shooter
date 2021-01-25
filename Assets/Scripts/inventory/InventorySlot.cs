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

            currentItem.Equip();
        }
    }

    public void FillSlot(Item newItem)
    {
        currentItem = newItem;
        icon.sprite = currentItem.defaulSprite;
        icon.SetNativeSize();
        if (newItem.itemType == ItemType.Weapon)
            icon.rectTransform.sizeDelta /= 2.75f;
        else
            icon.rectTransform.sizeDelta /= 2f;

        icon.enabled = true;
    }

    public void ClearSlot()
    {
        currentItem = null;
        icon.sprite = null;
        icon.enabled = false;
    }

    public bool IsSlotOccupied(){
        return currentItem == null ? false : true;
    }

}