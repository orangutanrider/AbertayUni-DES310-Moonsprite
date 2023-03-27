using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(IMover))]
public class NPCPatrolScript : MonoBehaviour
{
    // Attribution: Vasco F + Dominic R

    [Header("(by default NPCs will follow the first waypointSet in the list)")]
    public NPCPatrolState patrolState = NPCPatrolState.NotFollowingAnyWaypointSet;
    public List<NPCWaypointSet> waypointSets = new List<NPCWaypointSet>();

    IMover mover;

    int currentSetIndex = 0;
    int currentWaypointIndex = 0;

    public enum NPCPatrolState
    {
        FollowingWaypointSet,
        WaitingAtWaypoint,
        NotFollowingAnyWaypointSet
    }

    //========
    void Start()
    {
        mover = gameObject.GetComponent<IMover>();
    }

    void FixedUpdate()
    {
        Patrol();
    }

    #region Core
    void Patrol() // happens every frame on fixed update
    {
        if (patrolState == NPCPatrolState.FollowingWaypointSet)
        {
            MoveTowardsCurrentWaypoint();
        }
        else
        {
            StopMoving();
        }

        bool hasArrivedAtCurrentWaypoint = CheckIfInRangeOfCurrentWaypoint();

        if(hasArrivedAtCurrentWaypoint == true)
        {
            NextWaypoint();
        }
    }

    void NextWaypoint()
    {
        if(CurrentWaypoint().waitTime > 0 && CurrentWaypoint().waitedAt == false)
        {
            CurrentWaypoint().waitedAt = true;
            StartCoroutine(WaitAtCurrentWaypoint(CurrentWaypoint().waitTime));
            return;
        }
        CurrentWaypoint().waitedAt = false;

        currentWaypointIndex++;

        if(CurrentWaypoint().exitWaypoint == true)
        {
            ExitCurrentWaypointSet();
        }

        if(currentWaypointIndex > CurrentSet().waypoints.Count - 1)
        {
            currentWaypointIndex = 0;
        }
    }

    IEnumerator WaitAtCurrentWaypoint(float waitTime)
    {
        patrolState = NPCPatrolState.WaitingAtWaypoint;
        yield return new WaitForSeconds(waitTime);
        NextWaypoint();
        yield break;
    }

    bool CheckIfInRangeOfCurrentWaypoint()
    {
        float distance = Vector3.Distance(transform.position, CurrentWaypoint().waypointTransform.position);
        if (distance <= CurrentWaypoint().wayPointRadius)
        {
            return true;
        }
        return false;
    }
    #endregion

    #region Change Set Functions
    void ExitCurrentWaypointSet()
    {
        bool switchedToANewSet = false;
        if(CurrentSet().startFollowingNewSetOnExit == true) 
        {
            switchedToANewSet = SwitchToSetByIndex(CurrentSet().newSetIndex);
        }

        if (switchedToANewSet == false)
        {
            patrolState = NPCPatrolState.NotFollowingAnyWaypointSet;
            CurrentSet().active = false;
        }
    }

    public bool SwitchToSetByIndex(int newSetIndex) // returns true if succesful
    {
        if(newSetIndex > waypointSets.Count - 1 || newSetIndex < 0)
        {
            Debug.LogWarning("Index given was outside the bounds of the list");
            return false;
        }

        CurrentSet().active = false;
        currentSetIndex = newSetIndex;
        CurrentSet().active = true;
        return true;
    }

    public bool SwitchToSetByReference(NPCWaypointSet newWaypointSet) // returns true if succesful
    {
        if (newWaypointSet == null)
        {
            Debug.Log("Set recieved was null");
            return false;
        }

        int newSetIndex = GetSetIndex(newWaypointSet);
        if (newSetIndex < 0) // if it isn't in this script's list of sets (then add it)
        {
            waypointSets.Add(newWaypointSet);
            newSetIndex = waypointSets.Count - 1;
        }

        CurrentSet().active = false;
        currentSetIndex = newSetIndex;
        CurrentSet().active = true;
        return true;
    }

    int GetSetIndex(NPCWaypointSet waypointSet) // returns negative numbers if un-succesful
    {
        if (waypointSet == null)
        {
            Debug.LogWarning("Set recieved was null");
            return -2;
        }

        int setIndex = -1;
        for (int loop = 0; loop < waypointSets.Count; loop++)
        {
            if(setIndex > 0)
            {
                continue;
            }

            if(waypointSets[loop] == waypointSet)
            {
                setIndex = loop;
            }
        }

        if(setIndex == -1)
        {
            Debug.Log("This set doesn't exist in this script's list");
            return setIndex;
        }

        return setIndex;
    }
    #endregion

    #region Mover Functions
    void MoveTowardsCurrentWaypoint()
    {
        mover.Move(TowardsCurrentWaypoint());
    }

    void StopMoving()
    {
        mover.Move(Vector2.zero);
    }
    #endregion

    #region Misc data functions
    Vector3 TowardsCurrentWaypoint()
    {
        return (transform.position - CurrentWaypoint().waypointTransform.position).normalized;
    }

    NPCWaypointSet CurrentSet()
    {
        return waypointSets[currentSetIndex];
    }

    NPCWaypoint CurrentWaypoint()
    {
        return waypointSets[currentSetIndex].waypoints[currentWaypointIndex];
    }
    #endregion
}
