using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(IMover))]
[RequireComponent(typeof(INPCMoveAnimator))]
public class NPCPatrolScript : MonoBehaviour
{
    // Attribution: Vasco F + Dominic R

    [Header("(TOOL) Get Waypoints")]
    public Transform transformToGetWaypointsFrom = null;
    public bool BUTTONGetWaypoints = false;

    [Header("(TOOL) Multi-Edit")]
    public int selectListIndex = 0;
    [Space]
    public bool BUTTONClearSelectedIndex = false;
    [Space]
    public bool BUTTONSetAllInSelectedIndexToWaitTime = false;
    public float allWaitTimeValue = 1;
    [Space]
    public bool BUTTONSetAllInSelectedIndexToRandomWaitTime = false;
    public float allRandomWaitTimeValue = 0;

    [Header("System")]
    public bool animationsEnabled = false;
    public NPCPatrolState patrolState = NPCPatrolState.FollowingWaypointSet;
    public List<NPCWaypointSet> waypointSets = new List<NPCWaypointSet>();

    IMover mover;
    INPCMoveAnimator moveAnimator;

    int currentSetIndex = 0;
    int currentWaypointIndex = 0;

    public enum NPCPatrolState
    {
        FollowingWaypointSet,
        WaitingAtWaypoint,
        NotFollowingAnyWaypointSet
    }

    enum IndexSelectValidate
    {
        InvalidNegativeIndex,
        PositiveIndexIsOutsideTheBoundsOfTheArray,
        IndexIsValid
    }

    //EXECUTION
    void Start()
    {
        mover = gameObject.GetComponent<IMover>();
        moveAnimator = gameObject.GetComponent<INPCMoveAnimator>();
    }

    void FixedUpdate()
    {
        Patrol();
    }

    #region Tools
    // this is called whenever the component is interacted with via the inspector
    // it runs during edit mode
    // i use it as a hack to quickly make tools
    void OnValidate()
    {
        ClearSelectedIndex();

        GetWaypointsFromTransform();

        MultiEditSetWaitTime();
        MultiEditSetRandomWaitTime();
    }

    void ClearSelectedIndex()
    {
        if (BUTTONClearSelectedIndex == false)
        {
            return;
        }
        BUTTONClearSelectedIndex = false;

        #region index validation
        IndexSelectValidate indexValidation = ValidateIndexSelection();
        int indexToWriteTo = selectListIndex;

        if (indexValidation == IndexSelectValidate.InvalidNegativeIndex)
        {
            Debug.LogError("Will not write to a negative index");
            return;
        }

        if (indexValidation == IndexSelectValidate.PositiveIndexIsOutsideTheBoundsOfTheArray)
        {
            Debug.Log("Creating a new list entry, because index is greater than list bounds");
            waypointSets.Add(new NPCWaypointSet("new set", 0, new List<NPCWaypoint>()));
            indexToWriteTo = waypointSets.Count - 1;
        }
        #endregion

        if (waypointSets[selectListIndex].waypoints == null)
        {
            Debug.Log("No waypoints to clear");
            return;
        }

        if (waypointSets[selectListIndex].waypoints.Count == 0)
        {
            Debug.Log("No waypoints to clear");
            return;
        }

        waypointSets[selectListIndex].waypoints.Clear();
    }

    void GetWaypointsFromTransform()
    {
        if(BUTTONGetWaypoints == false)
        {
            return;
        }
        BUTTONGetWaypoints = false;

        if(transformToGetWaypointsFrom == null)
        {
            Debug.Log("no transform selected, select a parent object that has children that're supposed to be waypoints");
            return;
        }

        #region index validation
        IndexSelectValidate indexValidation = ValidateIndexSelection();
        int indexToWriteTo = selectListIndex;

        if(indexValidation == IndexSelectValidate.InvalidNegativeIndex)
        {
            Debug.LogError("Will not write to a negative index");
            return;
        }

        if(indexValidation == IndexSelectValidate.PositiveIndexIsOutsideTheBoundsOfTheArray)
        {
            Debug.Log("Creating a new list entry, because index is greater than list bounds");
            waypointSets.Add(new NPCWaypointSet("new set", 0, new List<NPCWaypoint>()));
            indexToWriteTo = waypointSets.Count - 1;
        }
        #endregion

        waypointSets[selectListIndex].waypoints.Clear();

        Transform[] childObjects = transformToGetWaypointsFrom.GetComponentsInChildren<Transform>();
        foreach(Transform child in childObjects)
        {
            if(child == transformToGetWaypointsFrom)
            {
                continue;
            }

            NPCWaypoint newWaypoint = new NPCWaypoint(child, false, 1, 0);
            waypointSets[indexToWriteTo].waypoints.Add(newWaypoint);
        }
    }

    void MultiEditSetWaitTime()
    {
        if (BUTTONSetAllInSelectedIndexToWaitTime == false)
        {
            return;
        }
        BUTTONSetAllInSelectedIndexToWaitTime = false;

        #region index validation
        IndexSelectValidate indexValidation = ValidateIndexSelection();
        if (indexValidation == IndexSelectValidate.PositiveIndexIsOutsideTheBoundsOfTheArray || indexValidation == IndexSelectValidate.InvalidNegativeIndex)
        {
            Debug.LogError("Selected index is outside the bounds of the list");
            return;
        }
        #endregion

        if (waypointSets[selectListIndex].waypoints == null)
        {
            Debug.Log("No waypoints to write to");
            return;
        }

        if (waypointSets[selectListIndex].waypoints.Count == 0)
        {
            Debug.Log("No waypoints to write to");
            return;
        }

        foreach (NPCWaypoint waypoint in waypointSets[selectListIndex].waypoints)
        {
            waypoint.waitTime = allWaitTimeValue;
        }
    }

    void MultiEditSetRandomWaitTime()
    {
        if (BUTTONSetAllInSelectedIndexToRandomWaitTime == false)
        {
            return;
        }
        BUTTONSetAllInSelectedIndexToRandomWaitTime = false;

        #region index validation
        IndexSelectValidate indexValidation = ValidateIndexSelection();
        if (indexValidation == IndexSelectValidate.PositiveIndexIsOutsideTheBoundsOfTheArray || indexValidation == IndexSelectValidate.InvalidNegativeIndex)
        {
            Debug.LogError("Selected index is outside the bounds of the list");
            return;
        }
        #endregion

        if (waypointSets[selectListIndex].waypoints == null)
        {
            Debug.Log("No waypoints to write to");
            return;
        }

        if (waypointSets[selectListIndex].waypoints.Count == 0)
        {
            Debug.Log("No waypoints to write to");
            return;
        }

        foreach (NPCWaypoint waypoint in waypointSets[selectListIndex].waypoints)
        {
            waypoint.randomWaitTimePlus = allRandomWaitTimeValue;
        }
    }

    IndexSelectValidate ValidateIndexSelection()
    {
        if (selectListIndex < 0)
        {
            Debug.Log("selected index is negative");
            return IndexSelectValidate.InvalidNegativeIndex;
        }

        if (selectListIndex > waypointSets.Count - 1)
        {
            Debug.Log("selected index is greater than list size");
            return IndexSelectValidate.PositiveIndexIsOutsideTheBoundsOfTheArray;
        }

        return IndexSelectValidate.IndexIsValid;
    }

    #endregion

    #region Core
    void Patrol() // happens every frame on fixed update
    {
        if (patrolState == NPCPatrolState.WaitingAtWaypoint || patrolState == NPCPatrolState.NotFollowingAnyWaypointSet)
        {
            StopMoving();
            return;
        }

        MoveTowardsCurrentWaypoint();
        bool hasArrivedAtCurrentWaypoint = CheckIfInRangeOfCurrentWaypoint();

        if(hasArrivedAtCurrentWaypoint == true)
        {
            NextWaypoint();
        }
    }

    void NextWaypoint()
    {
        bool isNpcToWait = CheckIfNPCIsToWaitAtCurrentWaypoint();
        if(isNpcToWait == true)
        {
            return;
        }

        if (CurrentWaypoint().exitWaypoint == true)
        {
            ExitCurrentWaypointSet();
            return;
        }

        bool isToTeleport = false;
        if(CurrentWaypoint().teleportToNextWaypoint == true)
        {
            isToTeleport = true;
        }

        currentWaypointIndex++;

        if (currentWaypointIndex > CurrentSet().waypoints.Count - 1)
        {
            currentWaypointIndex = 0;
        }

        if(isToTeleport == true)
        {
            transform.position = CurrentWaypoint().waypointTransform.position;
        }
    }

    bool CheckIfNPCIsToWaitAtCurrentWaypoint()
    {
        if (CurrentWaypoint().waitTime > 0 && CurrentWaypoint().waitedAt == false)
        {
            CurrentWaypoint().waitedAt = true;

            float randomWaitTimePlus = 0;
            if (CurrentWaypoint().randomWaitTimePlus > 0)
            {
                randomWaitTimePlus = Random.Range(0, CurrentWaypoint().randomWaitTimePlus);
            }

            StartCoroutine(WaitAtCurrentWaypoint(CurrentWaypoint().waitTime + randomWaitTimePlus));
            return true;
        }
        CurrentWaypoint().waitedAt = false;
        return false;
    }

    IEnumerator WaitAtCurrentWaypoint(float waitTime)
    {
        patrolState = NPCPatrolState.WaitingAtWaypoint;
        yield return new WaitForSeconds(1f * waitTime);
        patrolState = NPCPatrolState.FollowingWaypointSet;
        NextWaypoint();
        yield break;
    }

    bool CheckIfInRangeOfCurrentWaypoint()
    {
        float distance = Vector3.Distance(transform.position, CurrentWaypoint().waypointTransform.position);
        if (distance <= NPCWaypoint.wayPointRadius)
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

    bool SwitchToSetByIndex(int newSetIndex) // returns true if succesful
    {
        if(newSetIndex > waypointSets.Count - 1 || newSetIndex < 0)
        {
            Debug.LogWarning("Index given was outside the bounds of the list");
            return false;
        }

        CurrentSet().active = false;
        currentSetIndex = newSetIndex;
        CurrentSet().active = true;

        currentWaypointIndex = CurrentSet().startingWaypointIndex;

        return true;
    }

    public bool StartFollowingNewSetViaIndex(int index, NPCPatrolState newPatrolState)
    {
        bool succesfulSwitch = SwitchToSetByIndex(index);
        if(succesfulSwitch == true)
        {
            patrolState = newPatrolState;
            return true;
        }
        return false;
    }

    public bool StartFollowingNewSet(NPCWaypointSet newWaypointSet, NPCPatrolState newPatrolState) // returns true if succesful
    {
        if (newWaypointSet == null)
        {
            Debug.Log("Set recieved was null");
            return false;
        }

        int newSetIndex = GetSetIndex(newWaypointSet);
        if (newSetIndex < 0) // if it isn't in this script's list of sets (then add it)
        {
            Debug.Log("Adding the new set to the list");
            waypointSets.Add(newWaypointSet);
            newSetIndex = waypointSets.Count - 1;
        }

        patrolState = newPatrolState;

        CurrentSet().active = false;
        currentSetIndex = newSetIndex;
        CurrentSet().active = true;

        currentWaypointIndex = CurrentSet().startingWaypointIndex;

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

        if (animationsEnabled == true)
        {
            moveAnimator.UpdateWalkCycleAnimation(TowardsCurrentWaypoint());
        }
    }

    void StopMoving()
    {
        mover.Move(Vector2.zero);

        if (animationsEnabled == true)
        {
            moveAnimator.UpdateWalkCycleAnimation(Vector2.zero);
        }
    }
    #endregion

    #region Misc data functions
    Vector3 TowardsCurrentWaypoint()
    {
        return (CurrentWaypoint().waypointTransform.position - transform.position).normalized;
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
