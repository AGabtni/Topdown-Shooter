using UnityEngine;
using System.Collections.Generic;




[System.Serializable]
public class GameStats
{
    //Serialized fields : 
    public string levelName;
    public string difficulty;
    public float playerTime;

    //Non Serialized fields
    int _totalSpawners;
    public int TotalSpawners
    {
        get { return _totalSpawners; }
    }
    int _totalTowers;
    public int TotalTowers
    {
        get { return _totalTowers; }
    }
    int _destroyedSpawners;
    public int DestroyedSpawners
    {
        get { return _destroyedSpawners; }
    }
    int _destroyedTowers;
    public int DestroyedTowers
    {
        get { return _destroyedTowers; }
    }
    int _killedSlimes;
    public int KilledSlimes
    {
        get { return _killedSlimes; }
    }
    private float _timer;
    public void SetTimer(float timer)
    {
        _timer = timer;
    }
    public float GetTimer()
    {
        return _timer;
    }



    public GameStats(string levelName, string difficulty, int totalSpawners, int totalTowers)
    {
        this.difficulty = difficulty;
        this.levelName = levelName;
        this._totalSpawners = totalSpawners;
        this._totalTowers = totalTowers;
        _destroyedSpawners = 0;
        _destroyedTowers = 0;
        playerTime = 0;

    }

    public void UpdateSpawnerStats()
    {
        _destroyedSpawners++;
    }


    public void UpdateTowerStats()
    {
        _destroyedTowers++;
    }

    public void UpdateSlimeStats()
    {
        _killedSlimes++;
    }
    public void UpdatePlayerTime(float newTime)
    {
        playerTime = newTime;
    }

    public bool LevelComplete()
    {
        return _destroyedSpawners == TotalSpawners && _destroyedTowers == TotalTowers;
    }
}