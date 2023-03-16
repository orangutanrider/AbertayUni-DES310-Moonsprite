using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericCollectibleItem : Collectible, IInteractionInterface
{
    [Header("Parameters")]
    public ItemData sampleItemData;
    public int interactionPriotiry = 1;

    //========
    public static event HandleItemPickup OnItemCollected;
    public delegate void HandleItemPickup(ItemData itemData);
    int IInteractionInterface.InteractionPriority 
    {
        get { return interactionPriotiry; } 
        set { interactionPriotiry = value; }
    }

    //========
    void IInteractionInterface.InteractionEvent(PlayerInteractionController playerInteractionController, TagList tagList)
    {
        Collect();
    }

    void IInteractionInterface.FinishInteraction()
    {

    }

    public override void Collect()
    {
        Destroy(gameObject);
        OnItemCollected?.Invoke(sampleItemData);
    }
}
