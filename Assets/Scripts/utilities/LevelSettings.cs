using UnityEngine;
using UnityEngine.SceneManagement;


[CreateAssetMenu(fileName = "new LevelSettings", menuName = "Game Level")]
public class LevelSettings : ScriptableObject
{

    public Scenes sceneName;
    public int levelMaxTime;

    [Header("NPC settings")]
    public int maxPatrollerMobs = 10;
    public int maxChaserMobs = 10;
    public float mobSpawnDelay = 1;

    
}
