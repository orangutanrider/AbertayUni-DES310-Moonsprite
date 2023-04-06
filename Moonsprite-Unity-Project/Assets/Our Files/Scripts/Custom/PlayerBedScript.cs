using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBedScript : MonoBehaviour, IInteractable
{
    [SerializeField] int interactionPriority = 0;
    int IInteractable.InteractionPriority
    {
        get
        {
            return interactionPriority;
        }
        set
        {
            interactionPriority = value;
        }
    }

    void IInteractable.InteractionEvent(PlayerInteractionController playerInteractionController, TagList activeItemTagList)
    {
        // there should be somekind of confirmation step in this
        // (i.e. asking the player "are you sure")

        PlayerScreenTransitioner.instance.FadeOutToNewScene(2f, "");
    }
}
