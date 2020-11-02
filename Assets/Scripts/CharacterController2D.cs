﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]

public class CharacterController2D : MonoBehaviour
{
    // Start is called before the first frame update


    [SerializeField] private Camera cam;
    [SerializeField] private Transform weaponHandle;
    [SerializeField] private float movSpeed = 5f;
    Rigidbody2D body;
    Animator animController;
    bool isFacingRight = true;
    void Start()
    {


        body = GetComponent<Rigidbody2D>();
        animController = GetComponent<Animator>();
    }


    Vector2 movement;
    Vector2 mousePos;
    float relativeMouseX;
    float relativeMouseY;



    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 characterDirection = mousePos - body.position;
        float relativeAngle = Mathf.Atan2(characterDirection.y, characterDirection.x) * Mathf.Rad2Deg;
        relativeMouseX = Mathf.Cos(relativeAngle * Mathf.Deg2Rad);
        relativeMouseY = Mathf.Sin(relativeAngle * Mathf.Deg2Rad);




        if ((relativeMouseX > 0) && !isFacingRight)
        {
            Flip();
        }
        else if ((relativeMouseX < 0) && isFacingRight)
        {
            Flip();
        }

    }



    void FixedUpdate()
    {


        body.MovePosition(body.position + movement.normalized * movSpeed * Time.fixedDeltaTime);

        //transform.rotation 
        animController.SetFloat("Horizontal", relativeMouseX);
        animController.SetFloat("Vertical", relativeMouseY);
        animController.SetFloat("Speed", new Vector2(relativeMouseX, relativeMouseY).sqrMagnitude);
        if (EquipmentManager.instance.weaponInstance != null)
        {
            RotateWeapon();

        }


    }

    void RotateWeapon()
    {
        Vector2 newPosition = new Vector2(isFacingRight ? relativeMouseX : -relativeMouseX, relativeMouseY);
        newPosition.x /= 2;
        newPosition.y /= 2;
        weaponHandle.localPosition = newPosition;

        Animator weaponAnimator = EquipmentManager.instance.weaponInstance.GetComponent<Animator>();
        weaponAnimator.SetFloat("Horizontal", relativeMouseX);
        weaponAnimator.SetFloat("Vertical", relativeMouseY);


        if (relativeMouseY < 0)
        {
            EquipmentManager.instance.weaponInstance.GetComponent<SpriteRenderer>().sortingOrder = 1;
            GetComponent<SpriteRenderer>().sortingOrder = 0;
        }
        else
        {
            EquipmentManager.instance.weaponInstance.GetComponent<SpriteRenderer>().sortingOrder = 0;
            GetComponent<SpriteRenderer>().sortingOrder = 1;
        }


    }
    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        isFacingRight = !isFacingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}