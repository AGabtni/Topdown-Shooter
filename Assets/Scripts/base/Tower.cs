using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new TowerName", menuName = "Towers")]

public class Tower : ScriptableObject
{
    public int damageModifier;
    public int ammoForce;
    public float cooldown;
    public float shootRadius;
    public LayerMask targetMask;
}
