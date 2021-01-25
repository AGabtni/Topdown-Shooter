using UnityEngine;
using System.Collections;
using System.Linq;

public class SlimeController : EnemyBehaviour
{
    [SerializeField] public Slime enemySettings;
    [SerializeField] LayerMask targetsMask;

    void Start()
    {

        OnDeath.AddListener(Death);

        if (enemySettings.mobType == MobType.Patroller)
        {
            agent.destination = patrolPoints[0];
            agent.SearchPath();
        }


    }

    public override void UpdateGFX()
    {
        base.UpdateGFX();

    }

    void Update()
    {


        if (isDead)
            return;


        //Check if still has health otherwise explode
        if (CanDetonate()) OnDeath.Invoke();

        switch (enemySettings.mobType)
        {
            case MobType.Patroller:
                if (playerInView)
                {
                    if (playerLastPosition != null)
                        Chase(enemySettings.chasingSpeed, playerLastPosition);
                }
                else
                    Patrol(enemySettings.normalSpeed);

                break;
            case MobType.Chaser:
                if (FindObjectOfType<CharacterController2D>())
                    Chase(enemySettings.chasingSpeed, FindObjectOfType<CharacterController2D>().transform.position);
                break;
        }


        UpdateGFX();

    }

    bool CanDetonate()
    {
        if (!health.IsAlive()) return true;

        if (!agent.reachedDestination || currentState != EnemyState.Chasing) return false;


        if (playerLastPosition != null)
            if (Vector2.Distance(transform.position, playerLastPosition) < enemySettings.distanceBeforeAttack) return true;


        return false;
    }



    public override void Death()
    {

        base.Death();
        enemyAnim.SetTrigger("Explode");

        StartCoroutine(CastExplosion());

    }

    //Apply explosion effect and cast ripple that deal damage to players inside the radisu
    IEnumerator CastExplosion()
    {
        //This time corresponds to the duration of the explosion animation 
        yield return new WaitForSeconds(.9f);
        GameObject rippleEffect = FindObjectsOfType<EffectPooler>()
                    .First(pooler => pooler.effectType == EffectType.Shockwave)
                    .GetPooledObject();
        rippleEffect.transform.position = transform.position;
        rippleEffect.SetActive(true);
        rippleEffect.GetComponent<DesactivateObject>().Desactivate(.45f);
        CinemachineShake.Instance.ShakeCamera(10, 0.25f);

        RaycastHit2D[] explosionHits = Physics2D.CircleCastAll(transform.position, enemySettings.explosionRadius, Vector2.up, enemySettings.explosionRadius, targetsMask);
        foreach (RaycastHit2D hit in explosionHits)
        {
            Health health = hit.transform.GetComponent<Health>();
            if (health && health.IsAlive())
            {
                MaterialModifier modifier = hit.transform.root.GetComponentInChildren<MaterialModifier>();
                if (modifier)
                    modifier.SetTintColor(new Color(1, 0, 0, 1f));
                health.ChangeHealth(-enemySettings.explosionDamage);
            }
        }

        yield return new WaitForSeconds(0.5f);
        GameManager.Instance.OnMobKilled.Invoke(EnemyType.Slime);


        //Drop Health Potion 
        ItemPooler itemPooler = FindObjectsOfType<ItemPooler>().ToList().First(pooler => pooler.itemType == ItemType.Health);
        if (Random.Range(0f, 1f) > 0.5f)
        {
            if (itemPooler != null )
            {
                var healthPotion = itemPooler.GetPooledObject();
                healthPotion.transform.position = transform.position;
                healthPotion.SetActive(true);

            }
        }


        gameObject.SetActive(false);



    }

    void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            agent.isStopped = true;
            OnDeath.Invoke();
        }

    }


}