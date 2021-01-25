using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
[System.Serializable]
public struct FeedbackMessage
{
    public int minKilledEnemies;
    public string message;
    public Sprite failIcon;
}
public class FeedbackGenerator : MonoBehaviour
{


    [SerializeField] List<FeedbackMessage> countdownFailMessages = new List<FeedbackMessage>();
    [SerializeField] List<FeedbackMessage> playerFailMessages = new List<FeedbackMessage>();

    void Start()
    {
    }

   

    public FeedbackMessage GenerateMessage(GameStats stats)
    {
        float statsTime = stats.GetTimer();
        int killedEnemies = stats.DestroyedSpawners + stats.DestroyedTowers;
        
        if (statsTime == 0)
        {
            return countdownFailMessages.OrderBy(x => Mathf.Abs(killedEnemies - x.minKilledEnemies)).First();
        }
        else
        {
            return playerFailMessages.OrderBy(x => Mathf.Abs(killedEnemies - x.minKilledEnemies)).First();

        }

    }



}