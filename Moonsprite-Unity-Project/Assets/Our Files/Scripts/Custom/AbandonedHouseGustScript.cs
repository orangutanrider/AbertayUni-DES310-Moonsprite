using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbandonedHouseGustScript : MonoBehaviour, ITimelineEvent
{
    [Header("Required References")]
    public ParticleSystem ps;

    [Header("Parameters")]
    public float emissionDuration;

    void ITimelineEvent.TimelineEvent()
    {
        GetComponent<ParticleSystem>().Play();
    }
}
