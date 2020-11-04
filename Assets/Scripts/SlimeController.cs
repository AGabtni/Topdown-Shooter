using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections;


[RequireComponent(typeof(AstarAgent))]
public class SlimeController : EnemyBehaviour
{
    [SerializeField] protected Slime enemy;

    [SerializeField] LayerMask targetsMask;
    void Start()
    {

        OnDeath.AddListener(Death);

    }
    public override void UpdateGFX()
    {
        base.UpdateGFX();
    }

    void LateUpdate()
    {

        if (isDead)
            return;

        UpdateGFX();
        if (aIAgent.reachedDestination || !health.IsAlive())
            OnDeath.Invoke();
    }





    public override void Death()
    {

        base.Death();
        enemyAnim.SetTrigger("Explode");
        StartCoroutine(CastExplosion());

    }

    IEnumerator CastExplosion()
    {

        yield return new WaitForSeconds(1f);

        RaycastHit2D[] explosionHits = Physics2D.CircleCastAll(transform.position, enemy.explosionRadius, Vector2.up, enemy.explosionRadius, targetsMask);
        foreach (RaycastHit2D hit in explosionHits)
        {
            Debug.Log("Target hit is : " + hit.transform.name);
            Health health = hit.transform.GetComponent<Health>();
            if (health)
            {
                health.ChangeHealth(-enemy.explosionDamage);
                health.OnHealtedChange.Invoke();
            }
        }

        gameObject.SetActive(false);



    }



    void OnDrawGizmos()
    {

        Color color = Color.red;
        Gizmos.DrawSphere(transform.position, enemy.explosionRadius);
    }
}