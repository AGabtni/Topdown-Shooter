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

        if (col.gameObject.layer == LayerMask.NameToLayer("Item") || col.gameObject.layer == LayerMask.NameToLayer("Player"))
            return;


        //TODO  : check for health script on collided obj
        //if ()
        //{
        //    int damage = EquipmentManager.instance.currentWeapon.damageModifier;
        //    //TODO : adjust player health
        //
        //
        //}
        Debug.Log("BULLET HIT : " + col.gameObject.name);


        GameObject effect = effectPooler.GetPooledObject();
        effect.transform.position = transform.position;
        effect.gameObject.SetActive(true); 
        effect.GetComponent<DesactivateObject>().Desactivate(.25f);
        gameObject.SetActive(false);

    }



}
