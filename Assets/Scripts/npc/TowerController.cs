using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[RequireComponent(typeof(Health))]
public class TowerController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject towerGFX;
    [SerializeField] Tower tower;
    [SerializeField] ObjectPooler bulletPooler;
    [SerializeField] Transform firePoint;

    [Tooltip("Layer masks of gameobjects that will be ignored when an attack collides with them")]
    [SerializeField] LayerMask ignoreMask;
    [SerializeField] LayerMask targetMask;
    [SerializeField] ParticleSystem deathPS;


    Health health;
    Animator towerAnimator;
    float targetAngle;
    float targetX;
    float targetY;
    float shootTimer = 0f;
    bool timerActive;
    void Start()
    {
        health = GetComponent<Health>();
        towerAnimator = towerGFX.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        if (!health.IsAlive()) { StartCoroutine(OnDeath()); return; }



        Transform target = DetectTarget();
        if (target)
        {
            Vector2 direction = target.position - transform.position;
            targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            targetAngle = Snapping.Snap(targetAngle, 45f);
            targetX = Mathf.Cos(targetAngle * Mathf.Deg2Rad);
            targetY = Mathf.Sin(targetAngle * Mathf.Deg2Rad);
            if (timerActive)
                shootTimer -= Time.deltaTime;
            if (shootTimer < 0)
                shootTimer = 0;
            Shoot();
        }



    }

    void LateUpdate()
    {


        towerAnimator.SetFloat("Horizontal", targetX);
        towerAnimator.SetFloat("Vertical", targetY);

    }


    Transform DetectTarget()
    {

        RaycastHit2D explosionHit = Physics2D.CircleCast(transform.position, tower.shootRadius, Vector2.up, tower.shootRadius, tower.targetMask);
        return explosionHit ? explosionHit.transform : null;
    }


    void Shoot()
    {
        if (shootTimer > 0)
            return;
        timerActive = false;

        //Change Fire point rotation
        Vector3 rotation = firePoint.rotation.eulerAngles;
        rotation.z = targetAngle;
        firePoint.rotation = Quaternion.Euler(rotation);
        firePoint.localPosition = new Vector2(targetX * 1.5f, targetY * 1.5f);

        //Instantiate and set bullet
        GameObject bullet = bulletPooler.GetPooledObject();
        bullet.GetComponent<Bullet>().SetupBullet(ignoreMask, targetMask, tower.damageModifier);

        //Change bullet direction and rotation
        bullet.transform.position = firePoint.position;
        bullet.transform.rotation = firePoint.rotation;
        bullet.SetActive(true);

        //Add force to bullet
        bullet.GetComponent<Rigidbody2D>().AddForce(firePoint.right * tower.ammoForce, ForceMode2D.Impulse);

        shootTimer = tower.cooldown;
        timerActive = true;

    }

    IEnumerator OnDeath()
    {
        if (isDead) yield break;
        isDead = true;
        ParticleSystem deathSFX = Instantiate(deathPS);
        deathSFX.transform.position = transform.position;
        yield return new WaitForSeconds(deathSFX.main.duration);
        Destroy(deathSFX.gameObject);

        gameObject.SetActive(false);
    }

    bool isDead;
    void OnEnable()
    {

        isDead = false;
    }
}
