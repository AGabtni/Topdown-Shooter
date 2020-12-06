using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;




public class CinemachineShake : GenericSingletonClass<CinemachineShake>
{

    CinemachineVirtualCamera virtualCamera;
    float shakeTimer = float.PositiveInfinity;
    CinemachineBasicMultiChannelPerlin perlinChannel;

    private void Start()
    {


        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        perlinChannel = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

    }

    public void ShakeCamera(float intensity, float duration)
    {

        Debug.Log(Instance.virtualCamera);
        Debug.Log(virtualCamera);

        CinemachineBasicMultiChannelPerlin perlinChannel = Instance.virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        perlinChannel.m_AmplitudeGain = intensity;
        shakeTimer = Time.time + duration;

    }

    void Update()
    {

        if (Time.time >= shakeTimer)
        {
            perlinChannel.m_AmplitudeGain = 0;
            shakeTimer = float.PositiveInfinity;
        }
    }
}