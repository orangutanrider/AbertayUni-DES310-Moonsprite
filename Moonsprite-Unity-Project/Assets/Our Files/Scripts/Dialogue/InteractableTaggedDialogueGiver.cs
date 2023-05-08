using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableTaggedDialogueGiver : MonoBehaviour, IInteractable
{
    public class TaggedDialogueOrigin
    {
        public CustomTag tag;
        public DialogueObject startingDialogueObject;
    }

    public DialogueObject baseDialogueOrigin;
    public List<TaggedDialogueOrigin> taggedDialogueOriginsList = new List<TaggedDialogueOrigin>();

    [SerializeField] int interactionPriority = 0;

    int IInteractable.InteractionPriority
    {
        get { return interactionPriority; }
        set { interactionPriority = value; }
    }

    void IInteractable.InteractionEvent(PlayerInteractionController playerInteractionController, TagList activeItemTagList)
    {
        if (activeItemTagList.tags == null)
        {
            DialogueManager.instance.StartNewDialogue(baseDialogueOrigin, gameObject, playerInteractionController);
            return;
        }
        if (activeItemTagList.tags.Count == 0)
        {
            DialogueManager.instance.StartNewDialogue(baseDialogueOrigin, gameObject, playerInteractionController);
            return;
        }

        foreach (TaggedDialogueOrigin taggedDialogue in taggedDialogueOriginsList)
        {
            if (activeItemTagList.HasTag(taggedDialogue.tag) == true)
            {
                DialogueManager.instance.StartNewDialogue(taggedDialogue.startingDialogueObject, gameObject, playerInteractionController);
                return;
            }
        }

        DialogueManager.instance.StartNewDialogue(baseDialogueOrigin, gameObject, playerInteractionController);
    }
}
