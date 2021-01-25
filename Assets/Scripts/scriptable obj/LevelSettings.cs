using UnityEngine;
using UnityEngine.SceneManagement;


[CreateAssetMenu(fileName = "new LevelSettings", menuName = "Game Level")]
public class LevelSettings : ScriptableObject
{
    public Difficulty difficulty;
    public Scenes sceneName;
    public int levelMaxTime;

    [Header("NPC settings")]
    public TowerEvolutions towersMaxEvolution;

    
}
