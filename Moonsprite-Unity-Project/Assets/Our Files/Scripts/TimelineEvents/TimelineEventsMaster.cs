using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimelineEventsMaster : MonoBehaviour
{
    public float masterTimer = 0;
    public List<TimelineEvent> timelineEvents;


    TimelineEvent nextTimelineEvent;

    [HideInInspector] public TimelineEventsMaster instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        nextTimelineEvent = FindNextEvent();
    }

    private void Update()
    {
        masterTimer = masterTimer + Time.deltaTime;

        WaitForNextEvent();
    }

    void WaitForNextEvent()
    {
        if (nextTimelineEvent == null)
        {
            nextTimelineEvent = FindNextEvent();
            return;
        }

        if (nextTimelineEvent.passed == true)
        {
            nextTimelineEvent = FindNextEvent();
            return;
        }

        if (masterTimer >= nextTimelineEvent.triggerTimeSeconds)
        {
            TriggerEvent(nextTimelineEvent);
        }
    }

    void TriggerEvent(TimelineEvent timelineEvent)
    {
        timelineEvent.passed = true;

        if (timelineEvent.active == false)
        {
            nextTimelineEvent = FindNextEvent();
            return;
        }

        bool loadSuccesful = LoadEventInterfaces(timelineEvent);
        if (loadSuccesful == false)
        {
            nextTimelineEvent = FindNextEvent();
            return;
        }

        foreach (ITimelineEvent timelineEventInterface in timelineEvent.timelineEventInterfaces)
        {
            timelineEventInterface.TimelineEvent();
        }
    }

    TimelineEvent FindNextEvent()
    {
        if (timelineEvents.Count == 0)
        {
            return null;
        }

        float soonestTime = float.MaxValue;
        TimelineEvent soonestEvent = timelineEvents[0];
        foreach (TimelineEvent timelineEvent in timelineEvents)
        {
            if (timelineEvent.triggerTimeSeconds < soonestTime && timelineEvent.passed == false)
            {
                soonestTime = timelineEvent.triggerTimeSeconds;
                soonestEvent = timelineEvent;
            }
        }

        return soonestEvent;
    }

    bool LoadEventInterfaces(TimelineEvent timelineEvent)
    {
        if (timelineEvent.interfacesLoaded == true)
        {
            return true;
        }

        timelineEvent.timelineEventInterfaces = timelineEvent.gameObjectWithTimelineEvents.GetComponents<ITimelineEvent>();

        if (timelineEvent.timelineEventInterfaces.Length == 0)
        {
            Debug.LogError("Error with timeLineEvent " + timelineEvent.name + " the object has no interfaced timelineEvent components");
            return false;
        }

        timelineEvent.interfacesLoaded = true;
        return true;
    }
}
