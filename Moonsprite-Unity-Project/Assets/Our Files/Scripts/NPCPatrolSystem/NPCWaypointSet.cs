using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NPCWaypointSet 
{
    // Attribution: Vasco F + Dominic R

    // the best way for this to work would be somekind of share sets system
    // scriptable objects are usually used for that, but you can't use them here because scriptable objects can't have references to objects that're in scenes

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
