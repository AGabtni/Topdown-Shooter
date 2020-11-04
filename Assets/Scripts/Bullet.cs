using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class Bullet : MonoBehaviour
{
    [SerializeField] ObjectPooler effectPooler;
    // Update is called once per frame
    void Update()
    {

    }


    void OnTriggerEnter2D(Collider2D col)
    {
        //TODO  : make it character agnostic 
        if (col.gameObject.layer == LayerMask.NameToLayer("Item") || col.gameObject.layer == LayerMask.NameToLayer("Player"))
            return;

        Health health = col.GetComponent<Health>();
        if (health)
        {
            int damage = EquipmentManager.instance.currentWeapon.damageModifier;
            health.ChangeHealth(-damage);
            health.OnHealtedChange.Invoke();
        }
        Debug.Log("BULLET HIT : " + col.gameObject.name);


        GameObject effect = effectPooler.GetPooledObject();
        effect.transform.position = transform.position;
        effect.gameObject.SetActive(true);
        effect.GetComponent<DesactivateObject>().Desactivate(.25f);
        gameObject.SetActive(false);

    }



}
