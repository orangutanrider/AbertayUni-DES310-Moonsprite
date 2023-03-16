using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericCollectibleItem : Collectible, Interactable, IInteractable
{
    [Header("Parameters")]
    public ItemData sampleItemData;
    public int interactionPriotiry = 1;

    //========
    public static event HandleItemPickup OnItemCollected;
    public delegate void HandleItemPickup(ItemData itemData);
    int IInteractable.InteractionPriority 
    {
        get { return interactionPriotiry; } 
        set { interactionPriotiry = value; }
    }

    //========
    void IInteractable.InteractionEvent(PlayerInteractionController playerInteractionController, TagList tagList)
    {
        Collect();
    }

    public override void FinishInteraction()
    {

    }

    public override void Collect()
    {
        Destroy(gameObject);
        OnItemCollected?.Invoke(sampleItemData);
    }
}
