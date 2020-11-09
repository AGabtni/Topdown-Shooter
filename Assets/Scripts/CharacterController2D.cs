using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Health))]
public class CharacterController2D : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] Transform characterGFX;
    [SerializeField] Camera cam;
    [SerializeField] Transform weaponHandle;
    [SerializeField] float movSpeed = 5f;

    [Tooltip("Layer masks of gameobjects that will be ignored when an attack collides with them")]
    [SerializeField] LayerMask ignoreMask;
    [SerializeField] LayerMask targetMask;
    Health health;
    Rigidbody2D body;
    Animator animController;
    bool isFacingRight = true;
    void Start()
    {


        body = GetComponent<Rigidbody2D>();
        animController = characterGFX.GetComponent<Animator>();
        health = GetComponent<Health>();
    }


    Vector2 movement;
    Vector2 mousePos;

    float relativeAngle;
    float relativeMouseX;
    float relativeMouseY;

    float shootTimer = 0f;
    bool timerActive;

    void Update()
    {

        if (!health.IsAlive())
            return;



        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 characterDirection = mousePos - body.position;
        relativeAngle = Mathf.Atan2(characterDirection.y, characterDirection.x) * Mathf.Rad2Deg;
        relativeAngle = Snapping.Snap(relativeAngle, 45f);
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

        if (timerActive)
            shootTimer -= Time.deltaTime;
        if (shootTimer < 0)
            shootTimer = 0;
        if (Input.GetButtonDown("Fire1"))
            Shoot();
    }



    void FixedUpdate()
    {


        body.MovePosition(body.position + movement.normalized * movSpeed * Time.fixedDeltaTime);

        //transform.rotation 
        animController.SetFloat("Horizontal", relativeMouseX);
        animController.SetFloat("Vertical", relativeMouseY);
        animController.SetFloat("Speed", movement.sqrMagnitude);
        if (EquipmentManager.instance.weaponInstance != null)
        {
            RotateWeapon();

        }


    }

    void Shoot()
    {


        if (EquipmentManager.instance.weaponInstance != null)
        {
            if (shootTimer > 0)
                return;
            timerActive = false;


            Transform weapon = EquipmentManager.instance.weaponInstance;
            GameObject bullet = weapon.GetComponent<ObjectPooler>().GetPooledObject();
            bullet.GetComponent<Bullet>().SetupBullet(ignoreMask, targetMask, EquipmentManager.instance.currentWeapon.damageModifier);
            Vector2 direction = new Vector2(relativeMouseX, relativeMouseY);
            bullet.transform.position = (Vector2)weaponHandle.position + direction;

            //Rotate bullet to face direction
            Vector3 rotation = bullet.transform.rotation.eulerAngles;
            rotation.z = relativeAngle;
            bullet.transform.rotation = Quaternion.Euler(rotation);
            bullet.SetActive(true);

            //Add force to bullet
            bullet.GetComponent<Rigidbody2D>().AddForce(direction * EquipmentManager.instance.currentWeapon.ammoForce, ForceMode2D.Impulse);

            shootTimer = EquipmentManager.instance.currentWeapon.cooldown;
            timerActive = true;

        }


    }
    void RotateWeapon()
    {
        Vector2 newPosition = new Vector2(isFacingRight ? relativeMouseX : -relativeMouseX, relativeMouseY);
        newPosition.x /= 1.5f;
        newPosition.y /= 1.5f;
        weaponHandle.localPosition = newPosition;


        Animator weaponAnimator = EquipmentManager.instance.weaponInstance.GetComponent<Animator>();
        weaponAnimator.SetFloat("Horizontal", relativeMouseX);
        weaponAnimator.SetFloat("Vertical", relativeMouseY);


        if (relativeMouseY < 0)
        {
            EquipmentManager.instance.weaponInstance.GetComponent<SpriteRenderer>().sortingOrder = 2;
            characterGFX.GetComponent<SpriteRenderer>().sortingOrder = 1;
        }
        else
        {
            EquipmentManager.instance.weaponInstance.GetComponent<SpriteRenderer>().sortingOrder = 1;
            characterGFX.GetComponent<SpriteRenderer>().sortingOrder = 2;
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
