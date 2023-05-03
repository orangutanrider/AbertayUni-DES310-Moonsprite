using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PsTrailLifetimeLerper : MonoBehaviour, ITimelineEvent
{
    public ParticleSystem ps;

    [Header("Settings")]
    public bool startLerpingOnStart = false;
    public bool startLerpingOnTimelineEvent = false;
    [Space]
    public float minLifetimeMin;
    public float maxLifetimeMin;
    [Space]
    public float minLifetimeMax;
    public float maxLifetimeMax;
    [Space]
    public AnimationCurve lifetimeCurve;
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

        float curveValue = lifetimeCurve.Evaluate(changeTimer / changeTime);
        float newMinLifetime = Mathf.Lerp(minLifetimeMin, minLifetimeMax, curveValue);
        float newMaxLifetime = Mathf.Lerp(maxLifetimeMin, maxLifetimeMax, curveValue);
        var trails = ps.trails;
        var trailsLifetime = ps.trails.lifetime;
        trailsLifetime.constantMin = newMinLifetime;
        trailsLifetime.constantMax = newMaxLifetime;
        trails.lifetime = trailsLifetime;
    }

    void ITimelineEvent.TimelineEvent()
    {
        if (startLerpingOnTimelineEvent == false)
        {
            return;
        }
        active = true;
    }
}
