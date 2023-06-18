using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraMovement : MonoBehaviour
{
    public static CameraMovement instance;
    private CinemachineVirtualCamera virtualCamera;
    private CinemachineBasicMultiChannelPerlin channelPerlin;
    private float timeMovement;
    private float totalTime;
    private float startIntensity;

    private void Awake()
    {
        instance = this;
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        channelPerlin = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }
    public void Movement(float intensity, float frecuency, float time)
    {
        channelPerlin.m_AmplitudeGain = intensity;
        channelPerlin.m_FrequencyGain = frecuency;
        startIntensity = intensity;
        totalTime = time;
        timeMovement = time;
    }
    private void Update()
    {
        if(timeMovement > 0)
        {
            timeMovement -= Time.deltaTime;

            channelPerlin.m_AmplitudeGain = Mathf.Lerp(startIntensity, 0, 1 - (timeMovement / totalTime));
        }
    }
}
