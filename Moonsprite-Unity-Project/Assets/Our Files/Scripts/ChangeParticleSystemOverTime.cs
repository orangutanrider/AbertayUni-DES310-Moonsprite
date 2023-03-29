using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeParticleSystemOverTime : MonoBehaviour, ITimelineEvent
{
    [Header("Required Reference")]
    public ParticleSystem[] particleSystems;

    [Header("Settings")]
    public bool startChangingOnStart = false;
    public bool startChangingOnTimelineEvent = false;
    [Space]
    public AnimationCurve rateOverTimeCurve;
    public AnimationCurve velocityOverTimeCurve;
    public float changeTime = 10;

    [Header("these are exposed so you can test your changes, they get set to 0 and false when the game starts")]
    public float changeTimer = 0;
    public bool active = false;

    [Header("You have to manually add this script's host gameObject to the timeline event master to register it (if you're triggering it on a timeline event)")]
    [Header("Also, use 1 as the end time value for the intensity curve")]

    bool e = false; // this is here so it lets these other header exist

    // Start is called before the first frame update
    void Start()
    {
        active = false;
        changeTimer = 0;
        if (startChangingOnStart == false)
        {
            return;
        }
        active = true;
    }

    // Update is called once per frame
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

        float newEmissionRate = rateOverTimeCurve.Evaluate(changeTimer / changeTime);
        float newVelocityOverTime = velocityOverTimeCurve.Evaluate(changeTimer / changeTime);

        foreach(ParticleSystem particleSystem in particleSystems)
        {
            var emission = particleSystem.emission;
            emission.rateOverTime = newEmissionRate;

            var velocity = particleSystem.velocityOverLifetime;
            velocity.speedModifierMultiplier = newVelocityOverTime;
        }
    }

    void ITimelineEvent.TimelineEvent()
    {
        if (startChangingOnTimelineEvent == false)
        {
            return;
        }
        active = true;
    }
}
