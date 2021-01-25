
using UnityEngine;


[CreateAssetMenu(fileName = "new Gun", menuName = "Items/Weapon")]

public class Weapon : Item
{

    public AmmoType ammoType;
    public int damageModifier;
    public int ammoForce;
    public float cooldown;
    
    public override void Equip()
    {

        base.Equip();
        EquipmentManager.instance.EquipWeapon(this);

    }

    public override void UnEquip()
    {
        base.UnEquip();
        EquipmentManager.instance.UnequipWeapon();

    }
    public override void Use()
    {


        base.Use();
        
       

    }




}
