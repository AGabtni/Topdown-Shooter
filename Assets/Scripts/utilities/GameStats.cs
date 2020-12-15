using UnityEngine;






public class GameStats{

    int _totalMobs;
    public int TotalMobs{
        get{return _totalMobs;}
    }
    int _totalTowers;
    public int TotalTowers{
        get{return _totalTowers;}
    }
    int _killedMobs;
    public int KilledMobs{
        get{return _killedMobs;}
    }
    int _destroyedTowers;
    public int DestroyedTowers{
        get{return _destroyedTowers;}
    }

    public GameStats(int totalMobs, int totalTowers){
        this._totalMobs = totalMobs;
        this._totalTowers = totalTowers;
        _killedMobs = 0;
        _destroyedTowers = 0;
    }

    public void UpdateMobStats(){
        _killedMobs ++;
    }

    
    public void UpdateTowerStats(){
        _destroyedTowers++;
    }


}