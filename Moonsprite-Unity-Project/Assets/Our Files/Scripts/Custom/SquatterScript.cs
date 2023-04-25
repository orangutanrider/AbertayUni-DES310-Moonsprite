using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquatterScript : MonoBehaviour, IInteractable
{
    [Header("Required References")]
    public Animator squatterAnimator;
    public InteractableDialogueGiver squatterDialogueGiver;
    public GameObject debrisPilesPrefab;
    public Transform playerSceneObject;

    [Header("Parameters")]
    [SerializeField] int interactionPriority = -1;
    [Space]
    public float debrisCollisionShakeAmplitude;
    public float debrisCollisionShakeFrequency;
    public float debrisCollisionShakeDuration;
    public AnimationCurve debrisCollisionShakeDistanceFalloffCurve;
    [Space]
    public Transform ambulanceExitPosition;

    bool debrisHasCollided = false;

    public void DebrisCollision()
    {
        debrisHasCollided = true;

        // devestation tracker logbook entry
        DevestationTracker.DevestationEventEntry squatterInjuredEvent = DevestationTracker.instance.GetDevestationEventByName("Squatter Injured");
        DevestationTracker.instance.ConfirmDevestationEventHappened(squatterInjuredEvent);

        // screenshake
        float playerDistance = Vector3.Distance(transform.position, playerSceneObject.position);
        float amplitude = debrisCollisionShakeAmplitude * debrisCollisionShakeDistanceFalloffCurve.Evaluate(playerDistance);
        float frequency = debrisCollisionShakeFrequency * debrisCollisionShakeDistanceFalloffCurve.Evaluate(playerDistance);
        float duration = debrisCollisionShakeDuration * debrisCollisionShakeDistanceFalloffCurve.Evaluate(playerDistance);
        CinemachineScreenShaker.Instance.ShakeCamera(amplitude, frequency, duration);

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

        PlayerScreenTransitioner.instance.StartCoroutine(PlayerScreenTransitioner.instance.SquatterAmbulanceTransition());
        StartCoroutine(AmbulanceExit(PlayerScreenTransitioner.instance.ambulanceImageFadeDuration, playerInteractionController)); 

        // Transition to black
    }
    int IInteractable.InteractionPriority
    {
        get { return interactionPriority; }
        set { }
    }

    IEnumerator AmbulanceExit(float _delay, PlayerInteractionController _playerInteractionController)
    {
        yield return new WaitForSeconds(_delay);
        PlayerCameraManager.instance.FreeCameraFromConfiner();
        _playerInteractionController.ExitInteraction();
        playerSceneObject.position = ambulanceExitPosition.position;
    }
}
