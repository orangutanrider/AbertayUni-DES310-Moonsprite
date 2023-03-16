using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericCollectibleItem : MonoBehaviour, ICollectible
{
    public ItemData sampleItemData;

    public static event HandleItemPickup OnItemCollected;
    public delegate void HandleItemPickup(ItemData itemData);

    public void Collect()
    {
        Destroy(gameObject);
        OnItemCollected?.Invoke(sampleItemData);
    }
}
