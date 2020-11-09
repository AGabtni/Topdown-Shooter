using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EquipmentManager : MonoBehaviour
{
    #region Singleton

    public static EquipmentManager instance;
    //public MeshRenderer targetMesh;



    void Awake()
    {

        instance = this;

    }

    #endregion


    [SerializeField] Transform weaponHandle;
    [SerializeField] EquipmentSlot weaponSlot;

    // Callback for when an item is equipped/unequipped
    public delegate void OnWeaponChanged(Weapon newWeapon, Weapon oldWeapon);
    public OnWeaponChanged onWeaponChanged;


    private Transform _weaponInstance;
    public Transform weaponInstance
    {
        get { return _weaponInstance; }
        private set { _weaponInstance = value; }
    }

    private Weapon _currentWeapon;
    public Weapon currentWeapon
    {
        get { return _currentWeapon; }
        private set { _currentWeapon = value; }
    }



    public void EquipWeapon(Weapon newWeapon)
    {
        Weapon oldWeapon = UnequipWeapon();

        if (onWeaponChanged != null)
        {
            onWeaponChanged.Invoke(newWeapon, oldWeapon);
        }

        //Spawn weapon
        currentWeapon = newWeapon;

        //TODO : pool instead
        weaponInstance = Instantiate(currentWeapon.itemPrefab) as Transform;
        weaponInstance.SetParent(weaponHandle);
        weaponInstance.localPosition = Vector3.zero;
        weaponInstance.localScale = Vector3.one;
        //Remove weapons pickup component
        if (weaponInstance.GetComponent<InteractableWeapon>())
            GameObject.Destroy(weaponInstance.GetComponent<InteractableWeapon>());


        weaponSlot.AddItem(currentWeapon);

    }



    public Weapon UnequipWeapon()
    {
        Weapon oldWeapon = null;

        if (currentWeapon != null)
        {

            oldWeapon = currentWeapon;
            Inventory.instance.AddItem(oldWeapon);
            weaponSlot.ClearSlot();

            currentWeapon = null;
            if (weaponInstance != null)
            {
                Destroy(weaponInstance.gameObject);

            }

            if (onWeaponChanged != null)
            {

                onWeaponChanged.Invoke(null, oldWeapon);

            }






        }


        return oldWeapon;

    }

    public bool IsWeaponEquipped(){

        return currentWeapon != null ? true : false;
    }

}
