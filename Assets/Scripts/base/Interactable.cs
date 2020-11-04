using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField] Item item;
    public float interactionRadius = 1f;
    public LayerMask interactiveAgents;
    bool isFocused = false;
    bool hasInteracted = false;
    bool cantIteract = false;

    void Start(){


    }
    public virtual void Interact()
    {


    }


    void FixedUpdate()
    {


        //TODO : activate outline shader material when player is within
        // interaction radius
    }
    void OnTriggerEnter2D(Collider2D col)
    {

        //TODO  : desactivate gameobject instead for object pooling 
        if (col.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (Inventory.instance.AddItem(item))
                Destroy(gameObject);
        }
    }
}
