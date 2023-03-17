using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    // Attribution: Maceij Wolski + Dominic Rooney

    [Header("Required References")]
    public ToolBarUIScript toolBarUIScript;

    [Header("Item List")]
    public List<InventoryItem> itemList = new List<InventoryItem>();

    private Dictionary<ItemData, InventoryItem> itemDictionary = new Dictionary<ItemData, InventoryItem>();
    public static event Action<List<InventoryItem>> OnInventoryChange;

    public static Inventory instance = null;

    private void Start()
    {
        GenericCollectibleItem.OnItemCollected += Add;

        instance = this;
    }

    private void OnEnable()
    {
        GenericCollectibleItem.OnItemCollected += Add;

        toolBarUIScript.UpdateSlots();
    }

    private void OnDisable()
    {
        GenericCollectibleItem.OnItemCollected -= Add;

        toolBarUIScript.UpdateSlots();
    }

    public void Add(ItemData itemData)
    {
        InventoryItem newItem = new InventoryItem(itemData);
        itemList.Add(newItem);
        itemDictionary.Add(itemData, newItem);

        toolBarUIScript.UpdateSlots();
    }


    public void Remove(InventoryItem inventoryItem)
    {
        if(itemList.Count == 0)
        {
            return;
        }

        bool contains = false;
        if(itemList.Contains(inventoryItem) == true)
        {
            itemList.Remove(inventoryItem);
            contains = true;
        }

        if (contains == true && itemDictionary.TryGetValue(inventoryItem.itemData, out InventoryItem item))
        {
            itemList.Remove(item);
            itemDictionary.Remove(inventoryItem.itemData);
        }
    }
}
