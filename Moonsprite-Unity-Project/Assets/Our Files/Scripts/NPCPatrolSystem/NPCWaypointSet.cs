using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NPCWaypointSet 
{
    public string setName = "unnamed set";
    public bool active = false;
    [Space]
    public bool startFollowingNewSetOnExit = false;
    public int newSetIndex = -1;
    [Space]
    public int startingWaypointIndex = 0;
    public List<NPCWaypoint> waypoints = null;

    public NPCWaypointSet(string _setName, int _startingWaypointIndex, List<NPCWaypoint> _waypoints)
    {
        setName = _setName;
        startingWaypointIndex = _startingWaypointIndex;
        waypoints = _waypoints;
    }
}
