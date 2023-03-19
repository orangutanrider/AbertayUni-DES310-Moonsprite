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
        PlayerScreenTransitioner.instance.DoorTransition();
        WaitForFadeThenTeleport();
    }

    IEnumerator WaitForFadeThenTeleport()
    {
        yield return new WaitForSeconds(PlayerScreenTransitioner.instance.doorTransitionTime * (1 / 3));
        TeleportPlayerToDestinationObject();
        yield break;
    }

    void TeleportPlayerToDestinationObject()
    {
        player.playerMovement.AnimatorUpdate(exitDirection);
        player.gameObject.transform.position = DestinationObject.transform.position;
    }
}
