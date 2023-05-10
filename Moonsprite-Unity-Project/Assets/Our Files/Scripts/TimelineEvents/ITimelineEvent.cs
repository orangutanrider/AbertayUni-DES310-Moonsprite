using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITimelineEvent
{
    // the event index is just so that you can have the event master call the same event multiple times but give different results
    public void TimelineEvent(int eventIndex = 0);
}
