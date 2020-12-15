using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(ObjectPooler))]
public class MobSpawner : MonoBehaviour
{
    [Tooltip("Slime scriptable for chaser type")]
    [SerializeField] Slime chaserSettings;

    [Tooltip("Slime scriptable for patroller type")]
    [SerializeField] Slime patrollerSettings;


    [Tooltip("Map of tiles where mobs can spawn on")]
    [SerializeField] Tilemap targetMap;
    ObjectPooler mobPooler;

    List<Vector3> tilemapLocations = new List<Vector3>();

    int _spawnedChasers = 0;
    int _spawnedPatrollers = 0;

    public int SpawnedChasers
    {
        get { return _spawnedChasers; }
    }
    public int SpawnedPatrollers
    {
        get { return _spawnedPatrollers; }
    }
    void Start()
    {

        mobPooler = GetComponent<ObjectPooler>();



        //Collect all tiles locations on target tilemap
        BoundsInt mapBounds = targetMap.cellBounds;
        foreach (var pos in mapBounds.allPositionsWithin)
        {

            Vector3Int localPlace = new Vector3Int(pos.x, pos.y, pos.z);
            Vector3 place = targetMap.CellToWorld(localPlace);
            if (targetMap.HasTile(localPlace))
            {
                tilemapLocations.Add(place);
            }

        }

    }



    public bool SpawnChaser(int limit)
    {
        if (_spawnedChasers == limit)
            return false;
        GameObject mob = mobPooler.GetPooledObject();
        mob.GetComponent<SlimeController>().patrolPoints = GetTilesLocations(3);
        mob.GetComponent<SlimeController>().enemySettings = chaserSettings;
        Vector2 randomPosition = tilemapLocations[Random.Range(0, tilemapLocations.Count)];
        mob.transform.position = randomPosition;
        mob.SetActive(true);
        _spawnedChasers++;

        return true;


    }
    public bool SpawnPatroller(int limit)
    {

        if (_spawnedPatrollers == limit)
            return false;

        GameObject mob = mobPooler.GetPooledObject();
        mob.GetComponent<SlimeController>().patrolPoints = GetTilesLocations(3);
        mob.GetComponent<SlimeController>().enemySettings = patrollerSettings;
        Vector2 randomPosition = tilemapLocations[Random.Range(0, tilemapLocations.Count)];
        mob.transform.position = randomPosition;
        mob.SetActive(true);
        _spawnedPatrollers++;
        return true;


    }


    //Return an array of random point on the tilemap
    Vector3[] GetTilesLocations(int maxPoints)
    {

        Vector3[] patrolPoints = new Vector3[maxPoints];
        int lastIndex = 0;

        //Pickup random and different patrol points
        for (int i = 0; i < maxPoints; i++)
        {
            int newIndex = Random.Range(0, tilemapLocations.Count);
            if (i > 0)
            {
                while (lastIndex == newIndex)
                {
                    newIndex = Random.Range(0, tilemapLocations.Count);

                }
            }


            patrolPoints[i] = tilemapLocations[newIndex];
            lastIndex = newIndex;

        }

        return patrolPoints;

    }





}
