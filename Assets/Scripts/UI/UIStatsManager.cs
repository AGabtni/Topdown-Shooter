using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI.Extensions;
using TMPro;

public enum Difficulty
{


    Easy,
    Medium,
    Hard
}

public class UIStatsManager : MonoBehaviour
{

    [Header("Stats UI references")]
    public TextMeshProUGUI easyModeText;
    public TextMeshProUGUI mediumModeText;
    public TextMeshProUGUI hardModeText;

    private LevelsStats fullStats;
    void Start()
    {
        fullStats = ScenesManager.Instance.GetStatsFromJSON();
       

        FillStatsUI("level1");
    }
    public void FillStatsUI(string levelName)
    {
   
        if (fullStats != null)
        {
            Array difficulties = Enum.GetValues(typeof(Difficulty));

            foreach (Difficulty difficulty in difficulties)
            {
                Debug.Log(difficulty);
                int playerTime = Mathf.FloorToInt(fullStats.GetPlayerTime(levelName, difficulty.ToString()));
                String newText = playerTime > 0 ? playerTime.ToString() +" s" : "No data available";
                newText = newText.PadLeft(newText.Length + 1, ' ');
                switch (difficulty)
                {
                    case Difficulty.Easy:
                        easyModeText.text += newText;
                        break;
                    case Difficulty.Medium:
                        mediumModeText.text += newText;

                        break;
                    case Difficulty.Hard:
                        hardModeText.text += newText;
                        break;
                }
                //if(playerTime > 0 )
                //else

            }
        }
    }


}
