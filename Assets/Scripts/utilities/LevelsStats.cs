using System.Collections.Generic;
using System;
public class LevelsStats
{
    public List<GameStats> Levels_Stats = new List<GameStats>();

    public GameStats GetStats(string levelName, string difficulty)
    {
        GameStats statsCopy = null;
        if (Levels_Stats.Count > 0)
        {
            foreach (var stat in Levels_Stats)
            {

                if (stat.levelName == levelName && stat.difficulty.Equals(difficulty))
                {
                    statsCopy = stat;
                    break;
                }

            }
        }
        return statsCopy;

    }

    public GameStats GetStatsByLevelname(string levelName)
    {
        GameStats statsCopy = null;
        if (Levels_Stats.Count > 0)
        {
            foreach (var stat in Levels_Stats)
            {

                if (stat.levelName == levelName)
                {
                    statsCopy = stat;
                    break;
                }

            }
        }
        return statsCopy;

    }

    public float GetOldRecord(string levelName)
    {
        GameStats stats = GetStatsByLevelname(levelName);
        if (stats != null)
            return stats.playerTime;

        return 0;
    }

    public float GetPlayerTime(string levelName,string difficulty){

        
        GameStats stats = GetStats(levelName,difficulty);
        if (stats != null)
            return stats.playerTime;

        return 0;
    }
}