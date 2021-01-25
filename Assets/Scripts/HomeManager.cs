using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;
using System.Linq;
public class HomeManager : MonoBehaviour
{
    // Start is called before the first frame update

    Difficulty currentDifficulty;
    CharacterName playerCharacter;

    public Texture2D lobbyCursor;

    [Header("UI elements")]
    public HorizontalScrollSnap characterScroll;
    public HorizontalScrollSnap difficultyScroll;

    [SerializeField] private OverlayController overlayController;


    void Start()
    {

        Cursor.SetCursor(lobbyCursor, Vector2.zero, CursorMode.ForceSoftware);

        difficultyScroll.StartingScreen = PlayerPrefs.HasKey("DIFFICULTY") ? (int)Enum.Parse(typeof(Difficulty), PlayerPrefs.GetString("DIFFICULTY")) : 0;
        characterScroll.StartingScreen = PlayerPrefs.HasKey("CHARACTER") ? (int)Enum.Parse(typeof(CharacterName), PlayerPrefs.GetString("CHARACTER")) : 0;

        currentDifficulty = difficultyScroll.StartingScreen < Enum.GetValues(typeof(Difficulty)).Length ? (Difficulty)difficultyScroll.StartingScreen : Difficulty.Easy;
        playerCharacter = characterScroll.StartingScreen < Enum.GetValues(typeof(CharacterName)).Length ? (CharacterName)characterScroll.StartingScreen : CharacterName.Character1;
       
        StartCoroutine(overlayController.FadeColor(1f,new Color(0,0,0,0)));
        StartCoroutine(overlayController.FadeValue(1f,0));

    }




    //UI callbacks
    public void OnPlay()
    {

        PlayerPrefs.SetString("DIFFICULTY", currentDifficulty.ToString());
        PlayerPrefs.SetString("CHARACTER", playerCharacter.ToString());
        StopAllCoroutines();
        StartCoroutine(Play());

    }

      IEnumerator Play()
    {
        StartCoroutine(overlayController.FadeColor(1f,new Color(0,0,0,1)));
        yield return  StartCoroutine(overlayController.FadeValue(1f,1f));
        SceneManager.LoadScene(1);

    }

    public void OnQuit()
    {
        StopAllCoroutines();
        StartCoroutine(Quit());
    }
    IEnumerator Quit()
    {
        StartCoroutine(overlayController.FadeColor(1f, new Color(0,0,0,1)));
        yield return StartCoroutine(overlayController.FadeValue(1f,1f));

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif

    }

    public void OnCharacterSelected()
    {
        playerCharacter = characterScroll.CurrentPage < Enum.GetValues(typeof(CharacterName)).Length ? (CharacterName)characterScroll.CurrentPage : playerCharacter;

    }

    public void OnDifficultySelected()
    {

        currentDifficulty = difficultyScroll.CurrentPage < Enum.GetValues(typeof(Difficulty)).Length ? (Difficulty)difficultyScroll.CurrentPage : currentDifficulty;

    }


}
