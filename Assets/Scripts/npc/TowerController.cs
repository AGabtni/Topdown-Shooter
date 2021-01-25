using System.Collections;
using System.Linq;

using UnityEngine;



[RequireComponent(typeof(Health))]
public class TowerController : MonoBehaviour
{
    [Header("Tower settings")]
    [SerializeField] TowerEvolutions lastEvolution = TowerEvolutions.Evolution1;
    [SerializeField] GameObject towerGFX;
    [SerializeField] Tower[] towersSettings;
    [SerializeField] private Tower currentSettings;

    [Header("Shooting settings")]

    [SerializeField] Transform firePoint;

    [Tooltip("Layer masks of gameobjects that will be ignored when an attack collides with them")]
    [SerializeField] LayerMask ignoreMask;


    [Header("SFX settings")]
    [SerializeField] EffectType deathSFX;

    TowerEvolutions currentEvolution;
    private MaterialModifier modifier;
    private Health health;
    private Animator towerAnimator;

    private bool isDead;
    private bool timerActive;

    private float targetAngle;
    private float targetX;
    private float targetY;
    private float shootTimer = 0f;


    public void SetLastEvolution(TowerEvolutions lastEvolution)
    {

        this.lastEvolution = lastEvolution;

    }
    void OnEnable()
    {

        isDead = false;
        GetComponent<Health>().RestoreHealth();
    }
    void Start()
    {
        health = GetComponent<Health>();
        towerAnimator = towerGFX.GetComponent<Animator>();
        modifier = GetComponentInChildren<MaterialModifier>();

        currentEvolution = TowerEvolutions.Evolution1;
        currentSettings = GetTowerSettings(currentEvolution);


    }

    // Update is called once per frame
    void Update()
    {

        if (!health.IsAlive())
        {
            if (currentEvolution == lastEvolution)
                StartCoroutine(OnDeath());

            else
            {
                switch (currentEvolution)
                {
                    case TowerEvolutions.Evolution1:
                        StartCoroutine(EvolveTower(TowerEvolutions.Evolution2));
                        break;
                    case TowerEvolutions.Evolution2:
                        StartCoroutine(EvolveTower(TowerEvolutions.Evolution3));
                        break;
                }
            }

        }
        else
        {
            Transform target = CheckTarget();
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







    }
    void LateUpdate()
    {

        towerAnimator.SetFloat("Horizontal", targetX);
        towerAnimator.SetFloat("Vertical", targetY);

    }

    IEnumerator EvolveTower(TowerEvolutions nextEvolution)
    {
        if (currentEvolution == nextEvolution)
            yield break;

        currentEvolution = nextEvolution;
        currentSettings = GetTowerSettings(nextEvolution);
        health.RestoreHealth();
        modifier.SetTintColor(new Color(1, 1, 0, 1f), 0.75f);

        yield return new WaitForSeconds(0.75f);

        switch (nextEvolution)
        {
            case TowerEvolutions.Evolution2:
                towerAnimator.SetBool("EvolvedTo2", true);
                break;
            case TowerEvolutions.Evolution3:
                towerAnimator.SetBool("EvolvedTo3", true);
                break;

        }


    }

    Tower GetTowerSettings(TowerEvolutions evolution)
    {
        foreach (var settings in towersSettings)
        {
            if (settings.evolution == evolution)
                return settings;
        }
        return null;
    }

    Transform CheckTarget()
    {

        RaycastHit2D explosionHit = Physics2D.CircleCast(transform.position,
                                                        currentSettings.shootRadius,
                                                        Vector2.up,
                                                        0,
                                                        currentSettings.targetMask);

        return explosionHit ? explosionHit.transform : null;
    }

    //innstantiante bullet and gives it force
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
        GameObject bullet = FindObjectsOfType<AmmoPooler>().First(pooler => pooler.ammoType == currentSettings.ammoType).GetPooledObject(); ;
        bullet.GetComponent<Bullet>().SetupBullet(ignoreMask, currentSettings.targetMask, currentSettings.damageModifier);

        //Change bullet direction and rotation
        bullet.transform.position = firePoint.position;
        bullet.transform.rotation = firePoint.rotation;
        bullet.SetActive(true);

        //Add force to bullet
        bullet.GetComponent<Rigidbody2D>().AddForce(firePoint.right * currentSettings.ammoForce, ForceMode2D.Impulse);

        shootTimer = currentSettings.cooldown;
        timerActive = true;

    }

    //Called when health drops down to 0
    IEnumerator OnDeath()
    {
        if (isDead) yield break;
        isDead = true;
        StartCoroutine(TiltColor());
        GameObject sfxInstance = FindObjectsOfType<EffectPooler>()
                    .First(pooler => pooler.effectType == deathSFX)
                    .GetPooledObject();
        sfxInstance.transform.position = transform.position;
        sfxInstance.SetActive(true);

        yield return new WaitForSeconds(1f);
        sfxInstance.SetActive(false);

        GameManager.Instance.OnMobKilled.Invoke(EnemyType.Tower);
        //Drop Timer Powerup
        ItemPooler itemPooler = FindObjectsOfType<ItemPooler>().ToList().First(pooler => pooler.itemType == ItemType.TimerAddOn);
        if (itemPooler != null)
        {
            var item = itemPooler.GetPooledObject();
            item.transform.position = transform.position;
            item.SetActive(true);

        }
        gameObject.SetActive(false);
    }
    //Tilts material color each 0.2s
    IEnumerator TiltColor()
    {
        while (true)
        {
            modifier.SetTintColor(new Color(1, 1, 1, 1f), 4f);
            yield return new WaitForSeconds(0.2f);
        }
    }


    //Remove in PROD
    void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.red;
        //Gizmos.DrawSphere(transform.position, currentSettings.shootRadius);
    }

}
