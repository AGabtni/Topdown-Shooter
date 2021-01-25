using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum EffectType
{
    Burning,
    Explosion1,
    Explosion2,
    RoundExplosion,
    Shockwave,
    VerticalExplosionSmall,
    VerticalExplosion,
    Vortex,
    TowerExplosion,
    XPlosion,


}
public class EffectPooler : ObjectPooler
{
    public EffectType effectType;

}
