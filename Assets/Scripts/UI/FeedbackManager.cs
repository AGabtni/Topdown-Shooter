using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FeedbackManager : MonoBehaviour
{
    [SerializeField] FeedbackGenerator feedbackGenerator;

    [SerializeField] GameObject failPanel;
    [SerializeField] TextMeshProUGUI feedbackText;
    [SerializeField] Image feedbackIcon;

    [SerializeField] GameObject successPanel;
    [SerializeField] TextMeshProUGUI newTimeHeader;
    [SerializeField] TextMeshProUGUI newTimeText;
    [SerializeField] GameObject oldRecordPanel;
    [SerializeField] TextMeshProUGUI oldRecordText;

    // Start is called before the first frame update
    void Start()
    {
        successPanel.SetActive(false);
        failPanel.SetActive(false);
        //GameManager.Instance.OnFail.AddListener();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TriggerFailPanel(GameStats stats)
    {


        FeedbackMessage newMessage = feedbackGenerator.GenerateMessage(stats);
        feedbackText.text = newMessage.message;
        feedbackIcon.sprite = newMessage.failIcon;

        failPanel.SetActive(true);
        failPanel.GetComponent<Animator>().SetBool("Open", true);


    }

    public void TriggerSuccessPanel(GameStats stats, float oldRecordTime)
    {

        float playerTime = stats.playerTime;
        int minutes = Mathf.FloorToInt(playerTime / 60);
        int seconds = Mathf.FloorToInt(playerTime % 60);
        newTimeText.text = minutes + ":" + seconds;
        if (oldRecordTime == 0)
            oldRecordPanel.SetActive(false);
        else
        {
            minutes = Mathf.FloorToInt(oldRecordTime / 60);
            seconds = Mathf.FloorToInt(oldRecordTime % 60);
            oldRecordText.text = minutes + ":" + seconds;
        }

        if (oldRecordTime < stats.playerTime)
            newTimeHeader.text = "Your time";
        else
            newTimeHeader.text = "New record";


        //Fetch last player last record if there is any . If not hide the oldRecordPanel gameobject

        successPanel.SetActive(true);
        successPanel.GetComponent<Animator>().SetBool("Open", true);

    }
}
