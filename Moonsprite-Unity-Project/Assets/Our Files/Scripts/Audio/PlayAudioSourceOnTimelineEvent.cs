using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudioSourceOnTimelineEvent : MonoBehaviour, ITimelineEvent
{
    [Header("Required References")]
    public AudioSource WoodcreakAudio;

    void ITimelineEvent.TimelineEvent()
    {
        WoodcreakAudio.Play();
    }
}
