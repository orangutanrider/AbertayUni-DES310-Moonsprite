using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPrefabOnTimelineEvent : MonoBehaviour, ITimelineEvent
{
    public GameObject prefab;

    void ITimelineEvent.TimelineEvent(int eventIndex)
    {
        Instantiate(prefab, transform.position, transform.rotation);
    }
}
