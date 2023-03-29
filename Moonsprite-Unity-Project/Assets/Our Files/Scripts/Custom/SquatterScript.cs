using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquatterScript : MonoBehaviour
{
    public Animator squatterAnimator;
    public GameObject debrisPilesPrefab;

    public void DebrisCollision()
    {
        DevestationTracker.DevestationEventEntry squatterInjuredEvent = DevestationTracker.instance.GetDevestationEventByName("Squatter Injured");
        DevestationTracker.instance.ConfirmDevestationEventHappened(squatterInjuredEvent);

        Instantiate(debrisPilesPrefab, transform.position, transform.rotation);
        squatterAnimator.SetBool("DebrisCollision", true);
    }
}
