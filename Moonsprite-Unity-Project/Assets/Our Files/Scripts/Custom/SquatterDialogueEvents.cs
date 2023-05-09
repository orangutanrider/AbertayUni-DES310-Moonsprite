using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquatterDialogueEvents : MonoBehaviour, IDialogueEvent
{
    [Header("Required References")]
    public NPCPatrolScript patrolScript;
    [Space]
    public Transform floorSpawnLocation;
    public GameObject floorPrefab;

    void IDialogueEvent.DialogueEvent(TagList tagList)
    {
        patrolScript.StartFollowingNewSetViaIndex(0, NPCPatrolScript.NPCPatrolState.FollowingWaypointSet);
        Instantiate(floorPrefab, floorSpawnLocation.position, transform.rotation);
    }
}
