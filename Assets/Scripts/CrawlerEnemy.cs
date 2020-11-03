using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections;


[RequireComponent(typeof(AstarAgent))]
public class CrawlerEnemy : EnemyBehaviour
{



    void Start()
    {

        aIAgent = GetComponent<AstarAgent>();
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
        if (aIAgent.reachedDestination)
            OnDeath.Invoke();
    }





    public override void Death()
    {

        base.Death();
        enemyAnim.SetTrigger("Explode");
        StartCoroutine(DeathEffect());

    }

    IEnumerator DeathEffect()
    {

        yield return new WaitForSeconds(1f);
        Debug.Log("DEATH HERE");
        gameObject.SetActive(false);



    }
}