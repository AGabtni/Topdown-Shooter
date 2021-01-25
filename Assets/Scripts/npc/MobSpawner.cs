using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Health))]
public class MobSpawner : MonoBehaviour
{


    [Header("Slimes settings")]

    [Tooltip("Slime scriptable for chaser type")]
    [SerializeField] Slime chaserSettings;

    [Tooltip("Slime scriptable for patroller type")]
    [SerializeField] Slime patrollerSettings;


    [Tooltip("Map of tiles where mobs can patrol to")]
    [SerializeField] Tilemap patrolMap;
    List<Vector3> tilemapLocations = new List<Vector3>();



    [Header("Spawning settings")]
    [SerializeField] int maxSpawnedMobs = 10;
    [SerializeField] float spawningInnerRadius = 2f;
    [SerializeField] float spawningOuterRadius = 4f;
    [SerializeField] float slimeSpawningDelay = 1f;
    [Header("SFX settings")]
    [SerializeField] EffectType deathSFX;
    float spawningTimer = float.PositiveInfinity;
    int _spawnedSlimes = 0;
    List<GameObject> instantiatedSlimes = new List<GameObject>();

    Health health;
    MaterialModifier modifier;
    bool isDead = false;
    EnemyPooler mobPooler;
    void OnEnable()
    {

        isDead = false;
        GetComponent<Health>().RestoreHealth();
    }
    void Start()
    {
        Camera.main.FadeIn(5f, Easing.Type.CircularIn);

        mobPooler = FindObjectOfType<EnemyPooler>();
        health = GetComponent<Health>();
        modifier = GetComponentInChildren<MaterialModifier>();

        //Collect all tiles locations from patrol tilemap
        BoundsInt mapBounds = patrolMap.cellBounds;
        foreach (var pos in mapBounds.allPositionsWithin)
        {

            Vector3Int localPlace = new Vector3Int(pos.x, pos.y, pos.z);
            Vector3 place = patrolMap.CellToWorld(localPlace);
            if (patrolMap.HasTile(localPlace))
            {
                tilemapLocations.Add(place);
            }

        }

    }


    void Update()
    {

        if (!health.IsAlive())
            StartCoroutine(OnDeath());


        if (Time.time >= spawningTimer)
            spawningTimer = float.PositiveInfinity;

        if (float.IsPositiveInfinity(spawningTimer) &&
                instantiatedSlimes.FindAll(slime => slime.activeSelf).Count < maxSpawnedMobs)
        {

            GameObject newSlime = SpawnSlime(Random.Range(0, 1f) >= 0.5 ? chaserSettings : patrollerSettings);
            newSlime.GetComponent<SlimeController>().patrolPoints = GetTilesLocations(3);
            spawningTimer = Time.time + slimeSpawningDelay;

        }

    }


    public GameObject SpawnSlime(Slime slimeSettings)
    {

        GameObject mob = mobPooler.GetPooledObject();
        mob.GetComponent<SlimeController>().patrolPoints = GetTilesLocations(3);
        mob.GetComponent<SlimeController>().enemySettings = slimeSettings;


        //Vector2 randomPosition = tilemapLocations[Random.Range(0, tilemapLocations.Count)];
        Vector3 randomPosition = Random.insideUnitCircle.normalized * Random.Range(spawningInnerRadius, spawningOuterRadius);
        mob.transform.position = transform.position + randomPosition;
        mob.SetActive(true);
        if (instantiatedSlimes.Count < maxSpawnedMobs)
        {
            _spawnedSlimes++;
            instantiatedSlimes.Add(mob);
        }
        //update graph :
        return mob;


    }


    //Return an array of random points on the tilemap
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

        GameManager.Instance.OnMobKilled.Invoke(EnemyType.Spawner);
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

    void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, spawningInnerRadius);

    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, spawningOuterRadius);

    }
}
