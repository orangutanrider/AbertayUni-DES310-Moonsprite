using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointGizmoLinesDisplayer : MonoBehaviour
{
    public bool displayingGizmos = false;
    [Space]
    public bool waypointSetLoops = true;
    public float radiusDisplay = 0.25f;
    [Space]
    public bool setWaypointsToChildObjectsBUTTON = false;
    public List<Transform> waypoints = new List<Transform>();

    void OnDrawGizmos()
    {
        if(displayingGizmos == false)
        {
            return;
        }

        DrawCircleGizmosOnWaypoints();
        DrawLineGizmosBetweenWaypoints();
    }

    void DrawCircleGizmosOnWaypoints()
    {
        foreach(Transform waypoint in waypoints)
        {
            Gizmos.DrawWireSphere(waypoint.position, radiusDisplay);
        }
    }

    void DrawLineGizmosBetweenWaypoints()
    {
        for (int loop = 0; loop < waypoints.Count; loop++)
        {
            if(loop + 1 < waypoints.Count)
            {
                Gizmos.DrawLine(waypoints[loop].position, waypoints[loop + 1].position);
            }
            else if(waypointSetLoops == true)
            {
                Gizmos.DrawLine(waypoints[loop].position, waypoints[0].position);
            }
        }
    }

    void OnValidate()
    {
        SetWaypointsToChildObjects();
    }

    void SetWaypointsToChildObjects()
    {
        if (setWaypointsToChildObjectsBUTTON == false)
        {
            return;
        }
        setWaypointsToChildObjectsBUTTON = false;

        waypoints.Clear();

        Transform[] childObjects = gameObject.GetComponentsInChildren<Transform>();
        foreach (Transform child in childObjects)
        {
            if (child == gameObject.transform)
            {
                continue;
            }

            waypoints.Add(child);
        }
    }
}
