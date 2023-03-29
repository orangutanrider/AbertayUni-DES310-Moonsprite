using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeParticleSystemOverTime : MonoBehaviour, ITimelineEvent
{
    [System.Serializable]
    public class ParticleSystemBurstParameters
    {
        public int burstIndexOnParticleSystem = 0;
        [Space]
        public bool burstCountOverTimeEnabled = false;
        public AnimationCurve burstCountOverTimeCurve;
        [Space]
        public bool burstIntervalOverTimeEnabled = false;
        public AnimationCurve burstIntervalOverTimeCurve;
        [Space]
        public bool burstProbabilityOverTimeEnabled = false;
        public AnimationCurve burstProbabilityOverTimeCurve;

        public ParticleSystemBurstParameters(int burstIndexOnParticleSystem, bool burstCountOverTimeEnabled, AnimationCurve burstCountOverTimeCurve, bool burstIntervalOverTimeEnabled, AnimationCurve burstIntervalOverTimeCurve, bool burstProbabilityOverTimeEnabled, AnimationCurve burstProbabilityOverTimeCurve)
        {
            this.burstIndexOnParticleSystem = burstIndexOnParticleSystem;
            this.burstCountOverTimeEnabled = burstCountOverTimeEnabled;
            this.burstCountOverTimeCurve = burstCountOverTimeCurve;
            this.burstIntervalOverTimeEnabled = burstIntervalOverTimeEnabled;
            this.burstIntervalOverTimeCurve = burstIntervalOverTimeCurve;
            this.burstProbabilityOverTimeEnabled = burstProbabilityOverTimeEnabled;
            this.burstProbabilityOverTimeCurve = burstProbabilityOverTimeCurve;
        }   
    }

    [Header("Required Reference")]
    public ParticleSystem[] particleSystems;

    [Header("Trigger Settings")]
    public bool startChangingOnStart = false;
    public bool startChangingOnTimelineEvent = false;

    [Header("Particle Settings")]
    public bool rateOverTimeEnabled = false;
    public AnimationCurve rateOverTimeCurve;
    [Space]
    public bool velocityMultiplyOverTimeEnabled = false;
    public AnimationCurve velocityMultiplyOverTimeCurve;
    [Space]
    public bool velocityOverTimeEnabled = false;
    public Vector2 maxVelocity;
    public Vector2 minVelocity;
    public AnimationCurve velocityOverTimeLerpShape;
    [Space]
    public bool burstChangeOverTimeEnabled = false;
    public List<ParticleSystemBurstParameters> burstsToChangeOverTime = new List<ParticleSystemBurstParameters>();
    [Space]
    public float sharedDuration = 10;

    [Header("these are exposed so you can test your changes, they get set to 0 and false when the game starts")]
    public float timer = 0;
    public bool active = false;

    /*
    [Header("You have to manually add this script's host gameObject to the timeline event master to register it (if you're triggering it on a timeline event)")]
    [Header("Also, use 1 as the end time value for the intensity curve")]

    bool e = false; // this is here so it lets these other header exist
    */

    // Start is called before the first frame update
    void Start()
    {
        active = false;
        timer = 0;
        if (startChangingOnStart == false)
        {
            return;
        }
        active = true;
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (active == false)
        {
            return;
        }

        if (timer > sharedDuration)
        {
            return;
        }

        timer = timer + Time.deltaTime;

        float newEmissionRate = rateOverTimeCurve.Evaluate(timer / sharedDuration);
        float newVelocityOverTime = velocityOverTimeCurve.Evaluate(timer / sharedDuration);

        foreach(ParticleSystem particleSystem in particleSystems)
        {
            var emission = particleSystem.emission;
            emission.rateOverTime = newEmissionRate;

            var velocity = particleSystem.velocityOverLifetime;
            velocity.speedModifierMultiplier = newVelocityOverTime;
        }
        */
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
