using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Events;
public class EquipmentSlot : MonoBehaviour
{

    //Callback for using item
    UnityEvent onItemUsed;
    public Button removeButton;
    Item item;
    Image icon;

    public void Start()
    {
        if (onItemUsed == null)
            onItemUsed = new UnityEvent();
        onItemUsed.AddListener(UpdateSlot);


        icon = transform.Find("Icon").GetComponent<Image>();


        ClearSlot();
    }


    public void AddItem(Item newItem)
    {

        item = newItem;

        //removeButton.interactable = true;
        icon.enabled = true;

        icon.sprite = item.defaulSprite;
        icon.SetNativeSize();
        icon.rectTransform.sizeDelta /= 2.5f;

        onItemUsed.Invoke();

    }



    public void ClearSlot()
    {


        item = null;


        icon.sprite = null;
        icon.enabled = false;
        //removeButton.interactable = false;

    }

    void UpdateSlot()
    {

        //if (EquipmentManager.instance.weaponInstance != null)
        //    Amount.text = "" + EquipmentManager.instance.weaponInstance.GetComponent<FireWeapon>().currentAmmo;


    }
    public void OnSlotClicked()
    {

        if (item != null)
        {

            item.Use();
            onItemUsed.Invoke();

        }

    }
}