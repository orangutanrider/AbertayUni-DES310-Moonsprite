using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PsNoiseStrengthLerper : MonoBehaviour, ITimelineEvent
{
    public ParticleSystem ps;

    [Header("Settings")]
    public bool startLerpingOnStart = false;
    public bool startLerpingOnTimelineEvent = false;
    [Space]
    public AnimationCurve noiseStrengthLerpCurve;
    public float changeTime = 10;

    [Header("These are exposed so you can test your changes, they're set to 0 and false, when the game starts")]
    public float changeTimer = 0;
    public bool active = false;

    void Start()
    {
        if (startLerpingOnStart == false)
        {
            return;
        }
        active = true;
    }

    void Update()
    {
        if (active == false)
        {
            return;
        }

        if (changeTimer > changeTime)
        {
            return;
        }

        changeTimer = changeTimer + Time.deltaTime;



        float newNoiseStrength = noiseStrengthLerpCurve.Evaluate(changeTimer / changeTime);
        var noise = ps.noise;
        noise.strength = newNoiseStrength;
    }

    void ITimelineEvent.TimelineEvent(int eventIndex)
    {
        if (startLerpingOnTimelineEvent == false)
        {
            return;
        }
        active = true;
    }
}
