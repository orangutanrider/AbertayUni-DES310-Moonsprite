using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ItemData : ScriptableObject
{
    // Start is called before the first frame update
    public TagList tagList;
    public Sprite icon;
    [Space]
    public GameObject itemActionPrefab;

    public ItemData(ItemData itemData)
    {
        tagList = itemData.tagList;
        icon = itemData.icon;
        itemActionPrefab = itemData.itemActionPrefab;
    }
}
