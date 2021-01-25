using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AmmoType
{
    Cannon,
    Flamethrower,
    Laser,
    Machinegun,
    Matter,
    Pistol,
    Rocket,
    Shotgun,
    Spazer

}
public class AmmoPooler : ObjectPooler
{
    public AmmoType ammoType;
}
