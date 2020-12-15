using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
public class HUDManager : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI towerStatsText;
    [SerializeField] TextMeshProUGUI mobStatsText;
    [SerializeField] TextMeshProUGUI countdownText;
    [SerializeField] GameObject pausePanel;
    [SerializeField] GameObject statsPanel;

    void Start()
    {
        pausePanel.SetActive(false);
        statsPanel.SetActive(false);

    }



    public void UpdateStats(GameStats stats)
    {
        towerStatsText.text = (stats.TotalTowers - stats.DestroyedTowers).ToString();
        mobStatsText.text = (stats.TotalMobs - stats.KilledMobs).ToString();


    }

    public void UpdateCountdown(float timer)
    {

        int minutes = Mathf.FloorToInt(timer / 60);
        int seconds = Mathf.FloorToInt(timer % 60);


        countdownText.text = minutes + ":" + seconds;


    }


    public void TriggerPauseMenu()
    {
        Time.timeScale = Time.timeScale > 0 ? 0 : 1;
        pausePanel.SetActive(!pausePanel.activeSelf);
        statsPanel.SetActive(false);


    }

    public void TriggerStatsPanel()
    {
        statsPanel.SetActive(!statsPanel.activeSelf);

    }





}