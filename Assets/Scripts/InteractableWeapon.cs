using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableWeapon : Interactable
{
    [SerializeField] Item item;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnTriggerEnter2D(Collider2D col)
    {
        //TODO  : desactivate gameobject instead for object pooling 

        if (IsInteractiveAgent(col.gameObject.layer))
        {
            if (Inventory.instance.AddItem(item))
                Destroy(gameObject);

        }
    }
}
