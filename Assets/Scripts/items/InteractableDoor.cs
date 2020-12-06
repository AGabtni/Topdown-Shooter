using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableDoor : Interactable
{


    [SerializeField] DoorDirection doorDirection = DoorDirection.Up;
    [SerializeField] InteractableDoor otherDoor;
    [SerializeField] bool isOneWayDoor = false;
    Animator doorAnimator;

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
        doorAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OpenDoor()
    {

    }
    void CloseDoor()
    {


    }

    void OnTriggerEnter2D(Collider2D col)
    {

        //TODO  : desactivate gameobject instead for object pooling 
        if (IsInteractiveAgent(col.gameObject.layer) && !hasInteracted)
        {
            Debug.Log("Player in");
            OnDoorEnter(col.gameObject.transform);

        }
    }


    public void OnDoorEnter(Transform player)
    {
        StartCoroutine(MoveAgent(player));
    }

    IEnumerator MoveAgent(Transform player)
    {
        Vector2 newPosition = otherDoor.transform.position;
        newPosition += directions[doorDirection] * 2;
        player.position = newPosition;

        hasInteracted = true;
        doorCollider.isTrigger = false;
        //If this door is one way , disable the door on the other side as well
        if (isOneWayDoor)
        {
            otherDoor.hasInteracted = true;
            otherDoor.doorCollider.isTrigger = false;
            doorAnimator.SetBool("isOpen", false);
            otherDoor.GetComponent<Animator>().SetBool("isOpen", false);
            yield break;
        }
        //Make the door interactable again
        yield return new WaitForSeconds(0.5f);
        doorCollider.isTrigger = true;
        hasInteracted = false;

  


    }


}
