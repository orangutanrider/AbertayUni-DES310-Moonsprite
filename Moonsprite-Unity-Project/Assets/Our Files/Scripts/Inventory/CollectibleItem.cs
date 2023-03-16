using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleItem : Collectible
{
    public ItemData sampleItemData;

    public static event HandleItemPickup OnItemCollected;
    public delegate void HandleItemPickup(ItemData itemData);

    public override void Collect()
    {
        Destroy(gameObject);
        OnItemCollected?.Invoke(sampleItemData);
    }
}
