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

    const float fadeTime = 1;
    float fadeTimer = fadeTime;

    PlayerInteractionController player = null;

    void IInteractable.InteractionEvent(PlayerInteractionController playerInteractionController, TagList activeItemTagList)
    {
        player = playerInteractionController;

    }

    void TeleportPlayerToDestinationObject()
    {
        player.playerMovement.AnimatorUpdate(exitDirection);
        player.gameObject.transform.position = DestinationObject.transform.position;
        player.playerCameraObject.transform.position = DestinationObject.transform.position;
    }
}
