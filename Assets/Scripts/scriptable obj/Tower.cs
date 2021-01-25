using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new TowerName", menuName = "Enemy/Towers")]
public class Tower : ScriptableObject
{   
    public TowerEvolutions evolution;
    public int damageModifier;
    public AmmoType ammoType;
    public int ammoForce;
    public float cooldown;
    public float shootRadius;
    public LayerMask targetMask;
}
public enum TowerEvolutions{
    Evolution1,
    Evolution2,
    Evolution3,
}