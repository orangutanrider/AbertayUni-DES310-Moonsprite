using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCPatrol : MonoBehaviour
{
    public Transform[] waypoints;
    public float speed = 2f;
    public float waypointRadius = 0.1f;

    private int currentWaypoint = 0;
    private bool isMovingForward = true;

    void Update()
    {
        // Check if NPC has reached current waypoint
        float distanceToWaypoint = Vector2.Distance(transform.position, waypoints[currentWaypoint].position);
        if (distanceToWaypoint < waypointRadius)
        {
            // If NPC is moving forward, move to next waypoint. If not, move to previous waypoint
            if (isMovingForward)
            {
                currentWaypoint++;
                if (currentWaypoint >= waypoints.Length)
                {
                    currentWaypoint = waypoints.Length - 2;
                    isMovingForward = false;
                }
            }
            else
            {
                currentWaypoint--;
                if (currentWaypoint < 0)
                {
                    currentWaypoint = 1;
                    isMovingForward = true;
                }
            }
        }

        // Move NPC towards current waypoint
        Vector2 direction = (waypoints[currentWaypoint].position - transform.position).normalized;
        transform.Translate(direction * speed * Time.deltaTime);
    }
}