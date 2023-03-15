using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineScreenShaker : MonoBehaviour
{
    [HideInInspector] public static CinemachineScreenShaker Instance { get; private set; }

    public AnimationCurve decayCurve;

    CinemachineBasicMultiChannelPerlin cinemachinePerlin;

    float shakeTime;
    float shakeTimer = 0;
    float amplitudeGain;
    float frequencyGain;

    void Start()
    {
        Instance = this;
        cinemachinePerlin = GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void ShakeCamera(float amplitude, float frequency, float time)
    {
        shakeTime = time;
        shakeTimer = time;
        amplitudeGain = amplitude;
        frequencyGain = frequency;
        cinemachinePerlin.m_FrequencyGain = frequencyGain;
    }

    void Update()
    {
        shakeTimer = shakeTimer - Time.deltaTime;
        if (shakeTimer > 0)
        {
            float amplitude = decayCurve.Evaluate(shakeTimer / shakeTime) * amplitudeGain;
            cinemachinePerlin.m_AmplitudeGain = amplitude;
        }
        else
        {
            cinemachinePerlin.m_AmplitudeGain = 0;
        }
    }
}
