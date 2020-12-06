using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionCollision : MonoBehaviour
{
    // Start is called before the first frame update

    EnemyBehaviour enemyBehaviour;
    void Start()
    {

        enemyBehaviour = GetComponentInParent<EnemyBehaviour>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D collider)
    {

        if (enemyBehaviour != null && collider.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            enemyBehaviour.playerInView = true;
            enemyBehaviour.playerLastPosition = collider.gameObject.transform.position;

        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {


        if (enemyBehaviour != null && collider.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            enemyBehaviour.playerInView = false;
        }
    }
}
