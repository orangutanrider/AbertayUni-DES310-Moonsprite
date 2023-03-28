using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(IMover))]
[RequireComponent(typeof(INPCMoveAnimator))]
public class NPCPatrolScript : MonoBehaviour
{
    // Attribution: Vasco F + Dominic R

    public bool animationsEnabled = false;

    [Header("Tools (multi-edit and get all waypoints from parent object)")]
    public int indexSelection = 0;
    [Space]
    public bool clearIndexButton = false;
    [Space]
    public Transform transformToGetWaypointsFrom = null;
    public bool getWaypointsButton = false;
    [Space]
    public float setAllRadiusTo = 0.25f;
    public bool setRadiusButton = false;
    [Space]
    public float setAllWaitTimeTo = 1;
    public bool setWaitTimeButton = false;
    [Space]
    public float setAllRandomWaitTimeTo = 0;
    public bool setRandomWaitTimeButton = false;

    [Header("(by default NPCs will follow the first waypointSet in the list)")]
    public NPCPatrolState patrolState = NPCPatrolState.NotFollowingAnyWaypointSet;
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

    //========
    void Start()
    {
        mover = gameObject.GetComponent<IMover>();
        moveAnimator = gameObject.GetComponent<INPCMoveAnimator>();
    }

    void FixedUpdate()
    {
        Patrol();
    }

    #region Tool
    void OnValidate()
    {
        
    }

    void GetWaypointsFromTransform()
    {
        if(getWaypointsButton == false)
        {
            return;
        }
        getWaypointsButton = false;

        if(transformToGetWaypointsFrom == null)
        {
            Debug.Log("no transform selected, select a parent object that has children that're supposed to be waypoints");
            return;
        }

        IndexSelectValidate indexValidation = ValidateIndexSelection();
        int indexToWriteTo = indexSelection;

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

        Transform[] childObjects = transformToGetWaypointsFrom.GetComponentsInChildren<Transform>();
    }

    IndexSelectValidate ValidateIndexSelection()
    {
        if (indexSelection < 0)
        {
            Debug.Log("selected index is negative");
            return IndexSelectValidate.InvalidNegativeIndex;
        }

        if (indexSelection > waypointSets.Count - 1)
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
        float distance = Vector3.Distance(transform.position, CurrentWaypoint().wa.position);
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
