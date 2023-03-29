using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class InteractableDialogueGiver : MonoBehaviour, IInteractable
{
    public DialogueObject startingDialogueObject;
    [Space]
    [SerializeField] int interactionPriority = 0;

    int IInteractable.InteractionPriority
    {
        get { return interactionPriority; }
        set { interactionPriority = value; }
    }

    void IInteractable.InteractionEvent(PlayerInteractionController playerInteractionController, TagList activeItemTagList)
    {
        DialogueManager.instance.StartNewDialogue(startingDialogueObject, gameObject, playerInteractionController);
    }
}
