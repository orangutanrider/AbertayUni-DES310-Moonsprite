using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))] // needs a collider to be detected by interaction raycasts
public class GenericCollectibleItem : MonoBehaviour, IInteractable
{
    [Header("Parameters")]
    public ItemData itemData;
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
        playerInteractionController.ExitInteraction();
        Collect();
    }

    void Collect()
    {
        Inventory.Instance.PickUpItem(itemData);

        Destroy(gameObject);
    }
}
