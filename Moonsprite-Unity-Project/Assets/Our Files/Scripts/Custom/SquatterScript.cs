using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class SquatterScript : MonoBehaviour, IInteractable
{
    [Header("Required References")]
    public Animator squatterAnimator;
    public InteractableDialogueGiver squatterDialogueGiver;
    public GameObject debrisPilesPrefab;
    public Transform playerSceneObject;
    public AudioSource DebrisCollisionAudio; // <- audio source for debris

    [Header("Parameters")]
    [SerializeField] int interactionPriority = -1;
    public ScreenshakeParameters screenshakeParameters;
    public Transform ambulanceExitPosition;

    bool debrisHasCollided = false;

    public void DebrisCollision()
    {
        debrisHasCollided = true;

        // devestation tracker logbook entry
        DevestationTracker.DevestationEventEntry squatterInjuredEvent = DevestationTracker.instance.GetDevestationEventByName(DevestationTracker.squatterInjuredLogName);
        DevestationTracker.instance.ConfirmDevestationEventHappened(squatterInjuredEvent);

        // screenshake
        CinemachineScreenShaker.Instance.ShakeCameraFromScriptableObjectWithDistanceFalloff(screenshakeParameters, transform.position);

        DebrisCollisionAudio.Play();  // <-play sudio

        Instantiate(debrisPilesPrefab, transform.position, transform.rotation);
        squatterAnimator.SetBool("DebrisCollision", true);        
        Destroy(squatterDialogueGiver);
    }

    void IInteractable.InteractionEvent(PlayerInteractionController playerInteractionController, TagList activeItemTagList)
    {
        if(debrisHasCollided == false) 
        {
            playerInteractionController.ExitInteraction();
            return; 
        }

        // transition
        PlayerScreenTransitioner.instance.StartCoroutine(PlayerScreenTransitioner.instance.SquatterAmbulanceTransition());
        StartCoroutine(AmbulanceExit(PlayerScreenTransitioner.instance.ambulanceImageFadeDuration + PlayerScreenTransitioner.instance.ambulanceImageFadeInDelay, playerInteractionController));
        
        // play ambulance audio
        // (@audiomaster placeholder)
    }
    int IInteractable.InteractionPriority
    {
        get { return interactionPriority; }
        set { }
    }

    IEnumerator AmbulanceExit(float _delay, PlayerInteractionController _playerInteractionController)
    {
        yield return new WaitForSecondsRealtime(_delay);
        PlayerCameraManager.instance.FreeCameraFromConfiner();
        _playerInteractionController.ExitInteraction();
        playerSceneObject.position = ambulanceExitPosition.position;
        StormAmbienceManager.Instance.TransitionToOutdoorsTrigger();
        Destroy(gameObject);
    }
}
