
using UnityEngine;


[CreateAssetMenu(fileName = "new Gun", menuName = "Items/Weapon")]

public class Weapon : Item
{


    public int damageModifier;
    

    public override void Equip()
    {

        base.Equip();


        EquipmentManager.instance.EquipWeapon(this);
        Inventory.instance.RemoveItem(this);

    }

    public override void UnEquip()
    {
        base.UnEquip();
        EquipmentManager.instance.UnequipWeapon();

    }
    public override void Use()
    {


        base.Use();
        //EquipmentManager.instance.TriggerWeapon();

        //TODO : pool bullet


    }




}
