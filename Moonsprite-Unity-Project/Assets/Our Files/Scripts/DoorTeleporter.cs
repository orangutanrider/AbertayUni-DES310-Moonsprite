using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTeleporter : MonoBehaviour, IInteractable
{
    public GameObject DestinationObject;
    public Vector2 exitDirection;
    [Space]
    [SerializeField] int interactionPriority = 0;
    int IInteractable.InteractionPriority 
    {
        get { return interactionPriority; }
        set { interactionPriority = value; }
    }

    PlayerInteractionController player = null;

    void IInteractable.InteractionEvent(PlayerInteractionController playerInteractionController, TagList activeItemTagList)
    {
        player = playerInteractionController;
        StartCoroutine(PlayerScreenTransitioner.instance.DoorTransition(PlayerScreenTransitioner.instance.doorTransitionTime));
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
        player.gameObject.transform.position = DestinationObject.transform.position;
    }
}
