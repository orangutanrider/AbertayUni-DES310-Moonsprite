using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class AbandonedHouseFloorScript : MonoBehaviour
{
    [Header("Required References")]
    public GameObject debrisPilesPrefab;
    public AudioSource collisionAudSource;

    [Header("Parameters")]
    public ScreenshakeParameters screenshakeParameters;

    public void DebrisCollision()
    {
        // devestation tracker logbook entry
        DevestationTracker.DevestationEventEntry squatterInjuredEvent = DevestationTracker.instance.GetDevestationEventByName(DevestationTracker.squatterInjuredLogName);
        DevestationTracker.instance.ConfirmDevestationEventPrevented(squatterInjuredEvent);

        // screenshake
        CinemachineScreenShaker.Instance.ShakeCameraFromScriptableObjectWithDistanceFalloff(screenshakeParameters, transform.position);

        // audio
        collisionAudSource.Play();

        Instantiate(debrisPilesPrefab, transform.position, transform.rotation);
    }

}
