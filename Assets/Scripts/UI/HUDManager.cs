using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using TMPro;
using System.Linq;
public class HUDManager : MonoBehaviour
{

    

    [Header("Cursor textures")]
    [SerializeField] private Texture2D crosshairIdle;
    [SerializeField] private Texture2D crosshairPressed;
    [SerializeField] private Texture2D pointer;

    [Header("UI elements")]
    [SerializeField] private GameObject hudCanvas;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private GameObject messagesPanel;

    [SerializeField] private Button zoomIn;
    [SerializeField] private Button zoomOut;

    [SerializeField] private TextMeshProUGUI towerStatsText;
    [SerializeField] private TextMeshProUGUI spawnerText;
    [SerializeField] private TextMeshProUGUI timerText;

    [SerializeField] private Image playerProfile;

    Animator pausePanelAnimator;


    void Start()
    {
        Cursor.SetCursor(crosshairIdle, Vector2.zero, CursorMode.ForceSoftware);

        //pausePanel.SetActive(false);
        pausePanelAnimator = pausePanel.GetComponent<Animator>();
        pausePanelAnimator.SetBool("Open", false);
        Minimap.Minimap.Init();
        zoomIn.onClick.AddListener(Minimap.Minimap.ZoomIn);
        zoomOut.onClick.AddListener(Minimap.Minimap.ZoomOut);


    }

    void Update()
    {

        if (pausePanelAnimator.GetBool("Open")) Cursor.SetCursor(pointer, Vector2.zero, CursorMode.ForceSoftware);
        else
        {
            if (Input.GetMouseButtonDown(0)) Cursor.SetCursor(crosshairPressed, Vector2.zero, CursorMode.ForceSoftware);

            else if (Input.GetMouseButtonUp(0)) Cursor.SetCursor(crosshairIdle, Vector2.zero, CursorMode.ForceSoftware);
        }


    }
    public void StartCountdown()
    {
        messagesPanel.GetComponent<Animator>().SetTrigger("ActivateCountdown");



    }
  

    float fadeValue = 0;
    public IEnumerator FadeHud(float targetFade)
    {
        fadeValue = 1 - targetFade;

        List<MaskableGraphic> images = hudCanvas.GetComponentsInChildren<MaskableGraphic>(true).ToList();
        MaterialModifier modifier = playerProfile.GetComponent<MaterialModifier>();
        if (modifier)
            modifier.SetMainColor(new Color(1, 1, 1, fadeValue), 2.5f);

        if (images.Count == 0) yield break;
        while (Mathf.Abs(fadeValue - targetFade) > 0.01f)
        {

            fadeValue = Mathf.Lerp(fadeValue, targetFade, 0.125f);
            foreach (var img in images)
            {
                Color color = img.color;
                color.a = fadeValue;
                img.color = color;


            }

            yield return new WaitForSeconds(0.005f);

        }


    }

    public void UpdateStats(GameStats stats)
    {
        towerStatsText.text = (stats.TotalTowers - stats.DestroyedTowers).ToString();
        spawnerText.text = (stats.TotalSpawners - stats.DestroyedSpawners).ToString();


    }

    public void UpdateCountdown(float timer)
    {

        string minutes = Mathf.FloorToInt(timer / 60).ToString().PadLeft(2, '0');
        string seconds = Mathf.FloorToInt(timer % 60).ToString().PadLeft(2, '0');
        timerText.text = minutes + ":" + seconds;


    }

    public void TriggerPauseMenu()
    {

        StopAllCoroutines();

        Time.timeScale = 1f;
        bool Open = !pausePanelAnimator.GetBool("Open");
        StartCoroutine(FadeHud(Convert.ToInt32(!Open)));

        pausePanelAnimator.SetBool("Open", Open);
        if (Open)
        {
            inventoryPanel.SetActive(false);
            StartCoroutine(Pause());

        }

    }

    public void UpdatePlayerProfile(Sprite sprite)
    {
        playerProfile.sprite = sprite;
    }

    //Event handler for health change
    public void OnPlayerHit()
    {
        MaterialModifier modifier = playerProfile.GetComponent<MaterialModifier>();
        if (modifier)
            modifier.SetTintColor(new Color(1, 0, 0, 1f));
    }

    IEnumerator Pause()
    {
        yield return new WaitForSeconds(0.5f);
        Time.timeScale = 0;

    }



    public void HideHUD()
    {
        hudCanvas.SetActive(false);
        pausePanel.SetActive(false);
        inventoryPanel.SetActive(false);
    }

    public void ShowHUD()
    {

        hudCanvas.SetActive(true);
        pausePanel.SetActive(true);
        //inventoryPanel.SetActive(true);

    }


}