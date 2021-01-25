using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Scenes
{

    lobby,
    level0,
    level1,

}
public class ScenesManager : GenericSingletonClass<ScenesManager>
{

    Scenes currentScene;


    public void LoadHomeScene()
    {

        SceneManager.LoadScene(0);
    }


    public void ReloadScene()
    {

        //SceneManager.LoadScene(SceneManager.);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    void LoadNextLevel()
    {

        //SceneManager.LoadScene(settings.sceneName+1.ToString());

    }



    public void SaveLevelData(GameStats newStat)
    {
        //SET SAVE
        LevelsStats totalStats = GetStatsFromJSON();

        //If there is no entries in the json create a new LevelStats 

        if (totalStats == null)
        {
            totalStats = new LevelsStats();
            totalStats.Levels_Stats.Add(newStat);

        }
        //If there is entries in the json check whether the new entry is already added 
        else
        {
            //GameStats copyStats = totalStats.GetStatsByLevelname(newStat.levelName);
            GameStats existingStats = totalStats.GetStats(newStat.levelName, newStat.difficulty);

            //if stats already stored just update the field :
            if (existingStats != null)
            {
                if (newStat.playerTime < existingStats.playerTime)
                    existingStats.playerTime = newStat.playerTime;
            }
            else
            {
                totalStats.Levels_Stats.Add(newStat);
            }
        }


        string statsText = JsonUtility.ToJson(totalStats);
        System.IO.File.WriteAllText(Application.persistentDataPath + "/LevelsStats.json", statsText);

    }
    public LevelsStats GetStatsFromJSON()
    {
        if (!System.IO.File.Exists(Application.persistentDataPath + "/LevelsStats.json"))
            return null;

        string loadedJson = System.IO.File.ReadAllText(Application.persistentDataPath + "/LevelsStats.json");
        if (loadedJson.Length > 0)
            return JsonUtility.FromJson<LevelsStats>(loadedJson);

        return null;

    }

    public float GetPlayerTime(string levelName, string difficulty)
    {
        LevelsStats totalStats = GetStatsFromJSON();
        if (totalStats != null)
            return totalStats.GetPlayerTime(levelName, difficulty);
        return 0;
    }
}
