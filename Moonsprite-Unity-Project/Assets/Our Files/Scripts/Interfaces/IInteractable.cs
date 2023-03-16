using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable 
{
    [Tooltip("When multiple interactables are in interaction range, the ones with higher priority will be what gets interacted with")]
    public int InteractionPriority { get; set; }

    public void InteractionEvent(PlayerInteractionController playerInteractionController, TagList activeItemTagList = null);
}
