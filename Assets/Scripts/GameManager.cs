using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class GameManager : MonoBehaviour
{   
    [SerializeField] MobSpawner spawner;
    [SerializeField] int maxSpawnedMobs = 10;
    int spawnedMobs = 0;

    [SerializeField] int spawnDelay = 10;
    float spawningTimer = float.PositiveInfinity;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (spawnedMobs < maxSpawnedMobs && float.IsPositiveInfinity(spawningTimer))
        {
            spawningTimer = Time.time + spawnDelay;
            spawner.SpawnMob();
            spawnedMobs++;
        }
        if (Time.time >= spawningTimer)
            spawningTimer = float.PositiveInfinity;
    }
}
