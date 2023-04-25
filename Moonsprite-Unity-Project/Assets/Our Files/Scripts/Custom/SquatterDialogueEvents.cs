using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquatterDialogueEvents : MonoBehaviour, IDialogueEvent
{
    [Header("Required References")]
    public NPCPatrolScript patrolScript;

    void IDialogueEvent.DialogueEvent(TagList tagList)
    {
        Debug.Log("E");
        patrolScript.StartFollowingNewSetViaIndex(0, NPCPatrolScript.NPCPatrolState.FollowingWaypointSet);
    }
}
