using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KazooItemActionScript : MonoBehaviour, IItemAction
{
    public float soundRange = 0;
    public bool gizmosEnabled = false;

    const string sleepingCitizenTag = "SleepingCitizen";

    void OnDrawGizmos()
    {
        if(gizmosEnabled == false) { return; }
        Gizmos.DrawWireSphere(transform.position, soundRange);
    }

    void IItemAction.TriggerItemAction(PlayerInventoryController playerInventoryController)
    {
        // play kazoo sound here

        playerInventoryController.ItemActionExit();
        RaycastHit2D[] soundCast = Physics2D.CircleCastAll(transform.position, soundRange, Vector2.zero);
        if(soundCast == null) { return; }
        foreach(RaycastHit2D soundHit in soundCast)
        {
            if(soundHit.collider.tag != sleepingCitizenTag) { continue; }

            soundHit.collider.GetComponent<SleepingCitizenScript>().WakeUp();
        }
    }
}
