using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DoorTeleporter : MonoBehaviour, IInteractable
{
    public DoorParameters doorParameters;

    [Header("Settings")]
    public GameObject destinationObject;
    public Vector2 exitDirection;
    [Space]
    public bool destinationHasCameraCollider = false;
    public Collider2D cameraCollider = null;
    public float cameraColliderDamping = 0.05f;
    [Space]
    [SerializeField] int interactionPriority = 0;

    int IInteractable.InteractionPriority 
    {
        get { return interactionPriority; }
        set { interactionPriority = value; }
    }

    const float gizmoSize = 1f;
    PlayerInteractionController player = null;

    void IInteractable.InteractionEvent(PlayerInteractionController playerInteractionController, TagList activeItemTagList)
    {
        player = playerInteractionController;
        StartCoroutine(PlayerScreenTransitioner.instance.DoorTransition());
        StartCoroutine(WaitForFadeThenTeleport(PlayerScreenTransitioner.instance.doorTransitionTime));
    }

    IEnumerator WaitForFadeThenTeleport(float _doorTransitionTime)
    {
        // these number values have to be declared with a f otherwise it breaks

        yield return new WaitForSeconds(_doorTransitionTime * (1f / 3f));
        player.playerMovement.AnimatorUpdate(exitDirection);
        yield return new WaitForEndOfFrame();
        player.playerMovement.AnimatorUpdate(Vector2.zero);
        TeleportPlayerToDestinationObject();
        yield return new WaitForSeconds(_doorTransitionTime * (1f / 3f));
        player.ExitInteraction(); 
    }

    void TeleportPlayerToDestinationObject()
    {
        player.gameObject.transform.position = destinationObject.transform.position;

        if(destinationHasCameraCollider == true)
        {
            PlayerCameraManager.instance.ConfineCameraWith(cameraCollider, cameraColliderDamping);
        }
        else
        {
            PlayerCameraManager.instance.FreeCameraFromConfiner();
        }
    }

    void OnDrawGizmos()
    {
        if(doorParameters.gizmosEnabled == false)
        {
            return;
        }

        Gizmos.DrawWireCube(transform.position, new Vector3(gizmoSize, gizmoSize));

        if(destinationObject ?? null) // you have to use this '??' thing to check if something is null cause unity overrides '!=' and '==' for game objects
        {
        }
        else
        {
            return;
        }

        Gizmos.DrawWireSphere(destinationObject.transform.position, gizmoSize / 4f);
        Gizmos.DrawLine(transform.position, destinationObject.transform.position);
    }
}
