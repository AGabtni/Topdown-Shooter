using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class Bullet : MonoBehaviour
{
    [SerializeField] ObjectPooler effectPooler;
    LayerMask targetMasks;
    LayerMask ignoreMasks;
    int damage;

    // Update is called once per frame
    public void SetupBullet(LayerMask ignoreMasks, LayerMask targetMasks, int damage)
    {

        this.targetMasks = targetMasks;
        this.ignoreMasks = ignoreMasks;
        this.damage = damage;

    }

    protected bool IsPartOfMasks(LayerMask mask, LayerMask masks)
    {
        return masks == (masks | (1 << mask));

    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (IsPartOfMasks(col.gameObject.layer, ignoreMasks))
            return;

        if (IsPartOfMasks(col.gameObject.layer, targetMasks))
        {
            Health health = col.GetComponent<Health>();
            if (health)
            {
                health.ChangeHealth(-damage);
                health.OnHealtedChange.Invoke();
                CinemachineShake.Instance.ShakeCamera(5, 0.2f);
            }
        }

        GameObject effect = effectPooler.GetPooledObject();
        effect.transform.position = transform.position;
        effect.gameObject.SetActive(true);
        effect.GetComponent<DesactivateObject>().Desactivate(.25f);
        gameObject.SetActive(false);

    }



}
