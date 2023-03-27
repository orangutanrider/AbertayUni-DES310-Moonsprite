using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NPCWaypoint 
{
    public Transform waypointTransform;
    [Tooltip("if an Npc reaches an exit waypoint, it'll terminate the waypoint set")] public bool exitWaypoint = false;
    [Space]
    public float wayPointRadius = 0.25f;
    [Space]
    public float waitTime = 1;
    public float randomWaitTimePlus = 0;

    [HideInInspector] public bool waitedAt = false;

    public NPCWaypoint(Transform _waypointTransform, bool _exitWaypoint, float _wayPointRadius, float _waitTime, float _randomWaitTimePlus)
    {
        waypointTransform = _waypointTransform;
        exitWaypoint = _exitWaypoint;
        wayPointRadius = _wayPointRadius;
        waitTime = _waitTime;
        randomWaitTimePlus = _randomWaitTimePlus;
    }
}
