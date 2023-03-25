using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class InteractableDialogueGiver : MonoBehaviour, IInteractable
{
    public DialogueObject startDialogueObject;
    [Space]
    [SerializeField] int interactionPriority = 0;

    int IInteractable.InteractionPriority
    {
        get { return interactionPriority; }
        set { interactionPriority = value; }
    }

    void IInteractable.InteractionEvent(PlayerInteractionController playerInteractionController, TagList activeItemTagList)
    {
        DialogueManager.instance.StartNewDialogue(startDialogueObject, gameObject);
        playerInteractionController.ExitInteraction();
    }
}
