using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TimelineEvent
{
    public string name;
    [Space]
    public float triggerTimeSeconds;
    public GameObject gameObjectWithTimelineEvents;
    [Space]
    public bool active = true;
    public bool passed = false;

    // interfaces can't be serialized in the inspector (which is why we have to use a gameobject reference)
    public ITimelineEvent[] timelineEventInterfaces;
    [HideInInspector] public bool interfacesLoaded = false;
}
