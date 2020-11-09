using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public float interactionRadius = 1f;
    public LayerMask interactiveAgents;
    protected bool hasInteracted = false;
    //bool cantIteract = false;

    public virtual void Interact()
    {

    }


    void FixedUpdate()
    {


        //TODO : activate outline shader material when player is within
        // interaction radius
    }

    protected bool IsInteractiveAgent(LayerMask mask)
    {
        return interactiveAgents == (interactiveAgents | (1 << mask));

    }
}
