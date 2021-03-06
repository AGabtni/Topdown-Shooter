﻿using System.Collections;
using System.Linq;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class Bullet : MonoBehaviour
{
    [SerializeField] EffectType collisionEffectType;
    LayerMask targetMasks;
    LayerMask collisionMasks;
    int damage;

    // Update is called once per frame
    public void SetupBullet(LayerMask collisionMasks, LayerMask targetMasks, int damage)
    {

        this.targetMasks = targetMasks;
        this.collisionMasks = collisionMasks;
        this.damage = damage;

    }

    protected bool IsPartOfMasks(LayerMask mask, LayerMask masks)
    {
        return masks == (masks | (1 << mask));

    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (IsPartOfMasks(col.gameObject.layer, collisionMasks))
            return;

        if (IsPartOfMasks(col.gameObject.layer, targetMasks))
        {
            Health health = col.transform.root.GetComponent<Health>();


            if (health && health.IsAlive())
            {
                MaterialModifier modifier = col.transform.root.GetComponentInChildren<MaterialModifier>();
                if (modifier)
                    modifier.SetTintColor(new Color(1, 0, 0, 1f));

                health.ChangeHealth(-damage);
                CinemachineShake.Instance.ShakeCamera(5, 0.2f);
            }

        }

        GameObject effect = FindObjectsOfType<EffectPooler>()
                    .First(pooler => pooler.effectType == collisionEffectType)
                    .GetPooledObject();
        effect.transform.position = transform.position;
        effect.gameObject.SetActive(true);
        effect.GetComponent<DesactivateObject>().Desactivate(.25f);
        gameObject.SetActive(false);

    }



}
