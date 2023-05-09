using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbandonedHouseFloorScript : MonoBehaviour
{
    [Header("Required References")]
    public GameObject debrisPilesPrefab;

    [Header("Parameters")]
    public float debrisCollisionShakeAmplitude;
    public float debrisCollisionShakeFrequency;
    public float debrisCollisionShakeDuration;
    public AnimationCurve debrisCollisionShakeDistanceFalloffCurve;

    public void DebrisCollision()
    {
        Transform playerSceneObject = PlayerStateMachine.instance.transform;

        // devestation tracker logbook entry
        DevestationTracker.DevestationEventEntry squatterInjuredEvent = DevestationTracker.instance.GetDevestationEventByName(DevestationTracker.squatterInjuredLogName);
        DevestationTracker.instance.ConfirmDevestationEventPrevented(squatterInjuredEvent);

        // screenshake
        float playerDistance = Vector3.Distance(transform.position, playerSceneObject.position);
        float amplitude = debrisCollisionShakeAmplitude * debrisCollisionShakeDistanceFalloffCurve.Evaluate(playerDistance);
        float frequency = debrisCollisionShakeFrequency * debrisCollisionShakeDistanceFalloffCurve.Evaluate(playerDistance);
        float duration = debrisCollisionShakeDuration * debrisCollisionShakeDistanceFalloffCurve.Evaluate(playerDistance);
        CinemachineScreenShaker.instance.ShakeCamera(amplitude, frequency, duration);

        Instantiate(debrisPilesPrefab, transform.position, transform.rotation);
    }

}
