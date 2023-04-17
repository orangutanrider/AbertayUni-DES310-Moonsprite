using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Obsolete("Replaced by 'LightLerper'")]
public class ChangeGlobalLightOverTime : MonoBehaviour //, ITimelineEvent
{
    /*
    [Header("Required Reference")]
    public Light globalLight;

    [Header("Settings")]
    public bool startChangingOnStart = false;
    public bool startChangingOnTimelineEvent = false;
    [Space]
    public Gradient colourChangeGradient;
    public AnimationCurve intensityCurve;
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
        if(startChangingOnStart == false)
        {
            return;
        }
        active = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(active == false)
        {
            return;
        }

        if(changeTimer > changeTime)
        {
            return;
        }

        changeTimer = changeTimer + Time.deltaTime;

        Color newColor = colourChangeGradient.Evaluate(changeTimer / changeTime);
        float newIntensity = intensityCurve.Evaluate(changeTimer / changeTime);

        globalLight.color = newColor;
        globalLight.intensity = newIntensity;
    }

    void ITimelineEvent.TimelineEvent()
    {
        if(startChangingOnTimelineEvent == false)
        {
            return;
        }
        active = true;
    }
    */
}
