using System;
using System.Linq;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Cinemachine;
public enum EnemyType
{
    Tower,
    Spawner,
    Slime,
}
public class GameManager : GenericSingletonClass<GameManager>
{
    [SerializeField] CinemachineVirtualCamera playerCamera;

    [SerializeField] private LevelSettings[] settings;
    [SerializeField] private LightController lightController;
    [SerializeField] private PlayerSpawner playerSpawner;

    [SerializeField] private TutorialManager tutorialManager;

    [SerializeField] private HUDManager hudManager;
    [SerializeField] private FeedbackManager feedbackManager;
    [SerializeField] private TimerController levelTimer;

    bool isGameOver = false;
    [HideInInspector] public UnityEvent<EnemyType> OnMobKilled = new UnityEvent<EnemyType>();
    [HideInInspector] public UnityEvent<bool> OnGameOver = new UnityEvent<bool>();


    private LevelSettings currentSettings;
    GameStats levelStats;

    void Start()
    {
        Difficulty difficulty = (Difficulty)Enum.Parse(typeof(Difficulty), PlayerPrefs.GetString("DIFFICULTY", Difficulty.Easy.ToString()));
        currentSettings = settings.First(s => s.difficulty == difficulty);
        levelStats = new GameStats(
                currentSettings.sceneName.ToString(),
                currentSettings.difficulty.ToString(),
                FindObjectsOfType<MobSpawner>().Length,
                FindObjectsOfType<TowerController>().Length
        );
        hudManager.UpdateStats(levelStats);

        //Add callbacks to events
        OnMobKilled.AddListener(MobKilledHandler);
        OnGameOver.AddListener(GameOverHandler);


        StartCoroutine(StartGame());

    }


    bool gameStarted = false;
    IEnumerator StartGame()
    {



        //Set towers evolution
        foreach (var tower in FindObjectsOfType<TowerController>()) tower.SetLastEvolution(currentSettings.towersMaxEvolution);
        //Set Timer 
        levelTimer.SetTimerLimit(currentSettings.levelMaxTime);
        hudManager.UpdateCountdown(levelTimer.GetTimer());

        //yield return StartCoroutine(hudManager.FadeHud(0));
        hudManager.HideHUD();

        //Check if tutorial is already played else start it
        if (!PlayerPrefs.HasKey("INTRO"))
        {
            PlayerPrefs.SetString("INTRO", "true");
            yield return StartCoroutine(tutorialManager.PlayTutorial());
        }

        hudManager.StartCountdown();
        yield return new WaitForSeconds(5.2f);

        hudManager.ShowHUD();
        yield return StartCoroutine(hudManager.FadeHud(1));


        CharacterName character = (CharacterName)Enum.Parse(typeof(CharacterName), PlayerPrefs.GetString("CHARACTER", CharacterName.Character1.ToString()));
        playerSpawner.InitPlayer(character);
        levelTimer.ResetTimer();
        gameStarted = true;


    }
    void Update()
    {
        if (!gameStarted) return;

        if (!levelTimer.IsTimeOut())
        {
            hudManager.UpdateCountdown(levelTimer.GetTimer());
        }
        else
        {
            if (!isGameOver)
            {
                OnGameOver.Invoke(false);
                isGameOver = true;
            }
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
                levelStats.UpdateSlimeStats();
                break;
            case EnemyType.Spawner:
                levelStats.UpdateSpawnerStats();
                break;

        }
        //Update HUD with stats
        hudManager.UpdateStats(levelStats);
        if (levelStats.LevelComplete())
            OnGameOver.Invoke(true);
    }


    void GameOverHandler(bool success)
    {
        StopAllCoroutines();
        levelStats.SetTimer(Mathf.Clamp(levelTimer.GetTimer(), 0, levelTimer.GetTimer()));
        if (success)
        {
            levelStats.UpdatePlayerTime(levelTimer.GetPlayerTime());
            feedbackManager.TriggerSuccessPanel(levelStats, ScenesManager.Instance.GetPlayerTime(levelStats.levelName, levelStats.difficulty));
            ScenesManager.Instance.SaveLevelData(levelStats);
        }
        else
            feedbackManager.TriggerFailPanel(levelStats);

        lightController.SetTargetIntensity(0);
        StartCoroutine(GameOver());


    }
    IEnumerator GameOver()
    {
        yield return StartCoroutine(hudManager.FadeHud(0));
        hudManager.HideHUD();
        yield return new WaitForSeconds(1f);
        Time.timeScale = 0;

    }

    //OnClick event handles
    public void Replay()
    {
        Time.timeScale = 1;
        gameStarted = false;
        ScenesManager.Instance.ReloadScene();
    }


    public void BackHome()
    {
        Time.timeScale = 1;
        ScenesManager.Instance.LoadHomeScene();

    }
}
