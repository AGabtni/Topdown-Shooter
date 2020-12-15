using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public enum EnemyType
{
    Tower,
    Slime
}
public class GameManager : GenericSingletonClass<GameManager>
{
    GameStats levelStats;
    [SerializeField] MobSpawner spawner;
    [SerializeField] HUDManager hudManager;
    public LevelSettings settings;



    float spawningTimer = float.PositiveInfinity;
    float levelTimer;
    bool isGameOver = false;
    [HideInInspector] public UnityEvent<EnemyType> OnMobKilled = new UnityEvent<EnemyType>();


    void Start()
    {

        levelStats = new GameStats(settings.maxChaserMobs + settings.maxPatrollerMobs, FindObjectsOfType<TowerController>().Length);
        hudManager.UpdateStats(levelStats);
        OnMobKilled.AddListener(MobKilledHandler);


        //Start Timer 
        levelTimer = settings.levelMaxTime;
    }

    void Update()
    {
        if (!isGameOver)
        {
            if (levelTimer > 0)
            {

                levelTimer -= Time.deltaTime;

            }
            else
            {
                Debug.Log("Out of time");
                levelTimer = 0;
                isGameOver = true;
            }


            //Update countdown in HUD
            hudManager.UpdateCountdown(levelTimer);
        }
        else
            return;

        if (Time.time >= spawningTimer)
            spawningTimer = float.PositiveInfinity;



        if (float.IsPositiveInfinity(spawningTimer))
        {
            if (spawner.SpawnPatroller(settings.maxPatrollerMobs) || spawner.SpawnChaser(settings.maxChaserMobs))
                spawningTimer = Time.time + settings.mobSpawnDelay;

        }



    }

    //Handler for each time a on mob killed event invocation
    void MobKilledHandler(EnemyType type)
    {


        //Update stats
        switch (type)
        {
            case EnemyType.Tower:
                levelStats.UpdateTowerStats();
                break;
            case EnemyType.Slime:
                levelStats.UpdateMobStats();
                break;


        }
        //Update HUD with stats
        hudManager.UpdateStats(levelStats);
    }


    public void Replay()
    {
        Time.timeScale = 1;
        ScenesManager.Instance.ReloadScene();
    }
}
