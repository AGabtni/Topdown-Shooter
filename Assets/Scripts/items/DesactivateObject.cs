using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesactivateObject : MonoBehaviour
{
    [Tooltip("If the timeBeforeDesactivation member is above 0 object will desactivate automatically after the time set")]
    [SerializeField] float timeBeforeDesactivation = 0f;
    float timeSaved = 0f;


    void Awake()
    {
        timeSaved = timeBeforeDesactivation;
    }
    void Update()
    {
        if (timeBeforeDesactivation > 0)
        {
            timeBeforeDesactivation = 0f;
            StartCoroutine(DesactivateAfter(timeBeforeDesactivation));
        }
    }


    void OnEnable()
    {
        if (timeSaved > timeBeforeDesactivation)
            timeBeforeDesactivation = timeSaved;
    }
    public void Desactivate(float time)
    {
        StartCoroutine(DesactivateAfter(time));
    }


    IEnumerator DesactivateAfter(float time)
    {

        yield return new WaitForSeconds(time);
        gameObject.SetActive(false);
    }



}
