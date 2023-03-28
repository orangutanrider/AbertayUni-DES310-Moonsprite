using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NPCWaypoint 
{
    // Attribution: Vasco F + Dominic R

    [Tooltip("for doors")] public bool teleportToNextWaypoint = false;
    public Transform waypointTransform;
    [Tooltip("if an Npc reaches an exit waypoint, it'll terminate the waypoint set")] public bool exitWaypoint = false;
    [Space]
    public float waitTime = 1;
    public float randomWaitTimePlus = 0;

    [HideInInspector] public bool waitedAt = false;

    public const float wayPointRadius = 0.25f;

    public NPCWaypoint(Transform _waypointTransform, bool _exitWaypoint, float _waitTime, float _randomWaitTimePlus)
    {
        waypointTransform = _waypointTransform;
        exitWaypoint = _exitWaypoint;
        waitTime = _waitTime;
        randomWaitTimePlus = _randomWaitTimePlus;
    }
}
