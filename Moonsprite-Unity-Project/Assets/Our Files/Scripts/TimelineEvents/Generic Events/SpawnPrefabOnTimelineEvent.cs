using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPrefabOnTimelineEvent : MonoBehaviour, ITimelineEvent
{
    public GameObject prefab;

    void ITimelineEvent.TimelineEvent()
    {
        Instantiate(prefab, transform.position, transform.rotation);
    }
}
