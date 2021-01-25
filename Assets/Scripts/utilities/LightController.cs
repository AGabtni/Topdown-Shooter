using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class LightController : MonoBehaviour
{
    // Start is called before the first frame update
    private Light2D globalLight;
    private float lightIntensity;
    private float fadeSpeed = 6f;
    void Start()
    {
        globalLight = GetComponent<Light2D>();
        lightIntensity = globalLight.intensity;
    }

    // Update is called once per frame
    void Update()
    {
        //Only if target intensity is less than original intensity
        if (globalLight.intensity > lightIntensity)
        {
            globalLight.intensity -= Time.deltaTime * fadeSpeed;
        }
        else
        {
            globalLight.intensity = lightIntensity;
        }
    }


    public void SetTargetIntensity(float lightIntensity, float fadeSpeed = 6f)
    {
        SetFadeSpeed(fadeSpeed);
        this.lightIntensity = lightIntensity;

    }

    public void SetFadeSpeed(float fadeSpeed)
    {
        this.fadeSpeed = fadeSpeed;
    }
}
