using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayParticleSystemOnTimelineEvent : MonoBehaviour, ITimelineEvent
{
    [Header("Required References")]
    public ParticleSystem ps;

    void ITimelineEvent.TimelineEvent()
    {
        ps.Play();
    }
}
