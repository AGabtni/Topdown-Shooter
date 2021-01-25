using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableDoor : Interactable
{


    [SerializeField] DoorDirection doorDirection = DoorDirection.Up;
    [SerializeField] InteractableDoor otherDoor;
    [SerializeField] bool isOneWayDoor = false;
    private Animator doorAnimator;
    private Collider2D doorCollider;
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




    void OnTriggerEnter2D(Collider2D col)
    {

        if (IsInteractiveAgent(col.gameObject.layer) && !hasInteracted)
        {
            StartCoroutine(OnDoorEnter(col.gameObject.transform.root));
        }
        
    }



    IEnumerator OnDoorEnter(Transform player)
    {
        Vector2 newPosition = otherDoor.transform.position;
        newPosition += directions[doorDirection] * 2;
        player.position = newPosition;

        LockDoor();
        //If this door is one way , disable the door on the other side as well
        if (isOneWayDoor)
        {
            otherDoor.LockDoor();
            yield break;
        }
        //Make the door interactable again
        yield return new WaitForSeconds(0.5f);
        UnlockDoor();


    }
    public void LockDoor()
    {
        hasInteracted = true;
        doorCollider.isTrigger = false;
        if (doorAnimator)
            doorAnimator.SetBool("isOpen", false);

    }
    void UnlockDoor()
    {
        hasInteracted = false;
        doorCollider.isTrigger = true;
        if (doorAnimator)
            doorAnimator.SetBool("isOpen", true);
    }

}
