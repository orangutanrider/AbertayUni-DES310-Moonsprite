using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCPatrolScript : MonoBehaviour
{
    [Header ("(by default NPCs will follow the first waypointSet in the list)")]
    public bool followingWaypointSet = true;
    public List<NPCWaypointSet> waypointSets = new List<NPCWaypointSet>();

    int currentSetIndex = 0;
    int currentWaypointIndex = 0;

    void Update()
    {

    }

    IEnumerator WaitForNextWaypoint()
    {
        yield break;
    }

    void Patrol()
    {
        
    }

    void PathTowardsCurrentWaypoint()
    {
        
    }

    Vector3 TowardsCurrentWaypoint()
    {
        return (transform.position - CurrentWaypoint().waypointTransform.position).normalized;
    }

    NPCWaypoint CurrentWaypoint()
    {
        return waypointSets[currentSetIndex].waypoints[currentWaypointIndex];
    }
}
