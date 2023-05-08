using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PsVelocityVectorLerper : MonoBehaviour, ITimelineEvent
{
    public ParticleSystem ps;

    [Header("Settings")]
    public bool startLerpingOnStart = false;
    public bool startLerpingOnTimelineEvent = false;
    [Space]
    public Vector3 randomMinVectorMin = Vector3.zero;
    public Vector3 randomMinVectorMax = new Vector3(1, 1, 0);
    [Space]
    public Vector3 randomMaxVectorMin = Vector3.zero;
    public Vector3 randomMaxVectorMax = new Vector3(1, 1, 0);
    [Space]
    public AnimationCurve vectorLerpCurve;
    public float changeTime = 10;

    [Header("These are exposed so you can test your changes, they're set to 0 and false, when the game starts")]
    public float changeTimer = 0;
    public bool active = false;

    [Header("ReadMe")]
    [TextArea(10, 100)]
    public string ReadMeText = "If you want to trigger this via a timeline event you have to manually add the game object (that this script is attached to) to the timeline event master to register it."
        +System.Environment.NewLine + "Also, use 0 as the start and 1 as the end, for the time values on the vectorLerpCurve.";

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

        float curveValue = vectorLerpCurve.Evaluate(changeTimer / changeTime);
        Vector3 newMinVelocity = Vector3.LerpUnclamped(randomMinVectorMin, randomMinVectorMax, curveValue);
        Vector3 newMaxVelocity = Vector3.LerpUnclamped(randomMaxVectorMin, randomMaxVectorMax, curveValue);

        var xVelocity = ps.velocityOverLifetime.x;
        var yVelocity = ps.velocityOverLifetime.y;

        xVelocity.constantMin = newMinVelocity.x;
        xVelocity.constantMax = newMaxVelocity.x;
        yVelocity.constantMin = newMinVelocity.y;
        yVelocity.constantMax = newMaxVelocity.y;

        var velocityOverLifetime = ps.velocityOverLifetime;
        velocityOverLifetime.x = xVelocity;
        velocityOverLifetime.y = yVelocity;
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
