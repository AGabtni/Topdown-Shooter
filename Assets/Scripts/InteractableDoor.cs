using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableDoor : Interactable
{
    [SerializeField] DoorDirection doorDirection = DoorDirection.Up;
    [SerializeField] InteractableDoor otherDoor;
    Collider2D doorCollider;
    enum DoorDirection
    {

        Up,
        Down,
        Left,
        Right,

    };
    Dictionary<DoorDirection, Vector2> directions = new Dictionary<DoorDirection, Vector2>(){
        {DoorDirection.Up, Vector2.up},
        {DoorDirection.Down, Vector2.down},
        {DoorDirection.Right, Vector2.right},
        {DoorDirection.Left, Vector2.left},


    };

    // Start is called before the first frame update
    void Start()
    {

        doorCollider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }



    void OnTriggerEnter2D(Collider2D col)
    {

        //TODO  : desactivate gameobject instead for object pooling 
        if (IsInteractiveAgent(col.gameObject.layer) && !hasInteracted)
        {
            Debug.Log("Player in");
            hasInteracted = true;
            doorCollider.isTrigger = false;
            otherDoor.hasInteracted = true;
            otherDoor.doorCollider.isTrigger = false;
            otherDoor.OnPlayerEnter(col.gameObject.transform);

        }
    }
    public void OnPlayerEnter(Transform player)
    {

        Vector2 newPosition = transform.position;
        newPosition += directions[doorDirection] * 2;
        player.position = newPosition;
        StartCoroutine(MovePlayer(player));
    }
    IEnumerator MovePlayer(Transform player)
    {
        yield return new WaitForSeconds(0.1f);

        Rigidbody2D body = player.GetComponent<Rigidbody2D>();
        body.MovePosition(body.position + directions[doorDirection] * 5);

        yield return null;

        hasInteracted = false;
        otherDoor.hasInteracted = false;

        doorCollider.isTrigger = true;
        otherDoor.doorCollider.isTrigger = true;

    }


}
