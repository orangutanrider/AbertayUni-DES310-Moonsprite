using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PsRateOverTimeLerper : MonoBehaviour, ITimelineEvent
{
    public ParticleSystem particleSystem;

    [Header("Settings")]
    public bool startLerpingOnStart = false;
    public bool startLerpingOnTimelineEvent = false;
    [Space]
    public AnimationCurve rateOverTimeLerpCurve;
    public float changeTime = 10;

    [Header("These are exposed so you can test your changes, they're set to 0 and false, when the game starts")]
    public float changeTimer = 0;
    public bool active = false;

    [Header("ReadMe")]
    [TextArea(10, 100)]
    public string ReadMeText = "If you want to trigger this via a timeline event you have to manually add the game object (that this script is attached to) to the timeline event master to register it."
        +System.Environment.NewLine + "Also, use 0 as the start and 1 as the end, for the time values on the rateOverTimeLerpCurve.";

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

        float newEmissionRate = rateOverTimeLerpCurve.Evaluate(changeTimer / changeTime);
        var emission = particleSystem.emission;
        emission.rateOverTime = newEmissionRate;
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
