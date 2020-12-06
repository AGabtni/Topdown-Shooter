using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections;


public class SlimeController : EnemyBehaviour
{
    [SerializeField] protected Slime enemy;

    [SerializeField] LayerMask targetsMask;
    void Start()
    {

        OnDeath.AddListener(Death);
        StartPatrol();
    }

    void StartPatrol()
    {
        agent.destination = patrolPoints[0];
        agent.SearchPath();
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

        if (!playerInView)
            Patrol(enemy.normalSpeed);

        else
            Chase(enemy.chasingSpeed);



        UpdateGFX();

    }

    bool CanDetonate()
    {
        if (!health.IsAlive()) return true;

        if (!agent.reachedDestination || currentState != EnemyState.Chasing) return false;


        if (playerLastPosition != null)
            if (Vector2.Distance(transform.position, playerLastPosition) < enemy.distanceBeforeAttack) return true;


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
        GameObject rippleEffect = effectPooler.GetPooledObject();
        rippleEffect.transform.position = transform.position;   
        rippleEffect.SetActive(true);
        rippleEffect.GetComponent<DesactivateObject>().Desactivate(.45f);
        CinemachineShake.Instance.ShakeCamera(10, 0.25f);

        RaycastHit2D[] explosionHits = Physics2D.CircleCastAll(transform.position, enemy.explosionRadius, Vector2.up, enemy.explosionRadius, targetsMask);
        foreach (RaycastHit2D hit in explosionHits)
        {
            Health health = hit.transform.GetComponent<Health>();
            if (health)
            {
                health.ChangeHealth(-enemy.explosionDamage);
                health.OnHealtedChange.Invoke();
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