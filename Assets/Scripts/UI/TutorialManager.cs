using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Cinemachine;
using System;
using UnityEngine.Experimental.Rendering.Universal;
public class TutorialManager : MonoBehaviour
{
    [Header("Tutorial camera")]
    [SerializeField] private CinemachineVirtualCamera tutorialCamera;

    [Header("Messages panel animator")]
    [SerializeField] private Animator messagesAnimator;


    //The number of elements in the dialogues list MUST match the number of pinpointed locations
    [Header("Tutorial Dialogue")]
    [SerializeField] private List<Dialogue> dialogues;

    [Header("Tutorial pinpointed locations")]
    [SerializeField] private List<Transform> cameraLocations;

    private float zoomedOutSize = 10f;
    private float zoomedInSize = 8.5f;

    // Start is called before the first frame update
    void Awake()
    {
        tutorialCamera.gameObject.SetActive(false);

        //PlayerPrefs.DeleteAll();//TEST ONLY


    }

    public IEnumerator PlayTutorial()
    {
        Camera.main.GetComponent<PixelPerfectCamera>().enabled = false;
        cameraLocations.Add(FindObjectsOfType<TowerController>().First().transform);
        cameraLocations.Add(FindObjectsOfType<MobSpawner>().First().transform);
        tutorialCamera.gameObject.SetActive(true);


        messagesAnimator.SetTrigger("FadeIn");

        for (int i = 0; i < cameraLocations.Count; i++)
        {

            tutorialCamera.Follow = cameraLocations[i];
            StartCoroutine(ZoomIn());
            DialogueManager.Instance.StartDialogue(dialogues[i]);
            while (DialogueManager.Instance.IsDialogueOpen()) yield return null;

            if(i > 2)yield return StartCoroutine(ZoomOut());




        }



        tutorialCamera.gameObject.SetActive(false);
        DialogueManager.Instance.StartDialogue(dialogues[dialogues.Count - 1]);
        while (DialogueManager.Instance.IsDialogueOpen()) yield return null;
        Camera.main.GetComponent<PixelPerfectCamera>().enabled = true;

        messagesAnimator.SetTrigger("FadeOut");


    }

    float zoomSpeed = 4f;
    IEnumerator Zoom(float zoomValue)
    {
        bool isBigger = zoomValue > tutorialCamera.m_Lens.OrthographicSize ? true : false;

        while (Mathf.Abs(tutorialCamera.m_Lens.OrthographicSize - zoomValue) > 0)
        {
            tutorialCamera.m_Lens.OrthographicSize += isBigger ? Time.deltaTime * zoomSpeed : -Time.deltaTime * zoomSpeed;
            yield return null;
        }


    }


    IEnumerator ZoomIn()
    {

        StopCoroutine(ZoomOut());

        while (tutorialCamera.m_Lens.OrthographicSize > zoomedInSize)
        {
            tutorialCamera.m_Lens.OrthographicSize -= Time.deltaTime * zoomSpeed;
            yield return null;
        }
    }

    IEnumerator ZoomOut()
    {

        StopCoroutine(ZoomIn());

        while (tutorialCamera.m_Lens.OrthographicSize < zoomedOutSize)
        {
            tutorialCamera.m_Lens.OrthographicSize += Time.deltaTime * zoomSpeed;
            yield return null;
        }
    }



}
