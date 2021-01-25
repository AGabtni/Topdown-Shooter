using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableItem : Interactable
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

        if (IsInteractiveAgent(col.gameObject.layer))
        {
            if (Inventory.Instance.AddItem(item))
                gameObject.SetActive(false);

        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (IsInteractiveAgent(collision.gameObject.layer))
        {
            if (Inventory.Instance.AddItem(item))
                gameObject.SetActive(false);

        }
    }
}
