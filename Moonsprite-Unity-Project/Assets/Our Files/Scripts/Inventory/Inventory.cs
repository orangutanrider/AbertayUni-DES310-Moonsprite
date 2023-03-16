using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [Header("Required References")]
    public ToolbarManager toolbarManager;

    [Header("Item List")]
    public List<InventoryItem> inventory = new List<InventoryItem>();

    private Dictionary<ItemData, InventoryItem> itemDictionary = new Dictionary<ItemData, InventoryItem>();
    public static event Action<List<InventoryItem>> OnInventoryChange;

    private void OnEnable()
    {
        SamplePickupItem.OnItemCollected += Add;

        toolbarManager.UpdateInventoryVisualDisplay();
    }

    private void OnDisable()
    {
        SamplePickupItem.OnItemCollected -= Add;

        toolbarManager.UpdateInventoryVisualDisplay();
    }

    public void Add(ItemData itemData)
    {
        if (itemDictionary.TryGetValue(itemData, out InventoryItem item))
        {

            item.AddToStack();
            Debug.Log("Sample Item Added");
            //Debug.Log($"{item.itemData.name} total stack is currently {item.stacksize}");
            OnInventoryChange?.Invoke(inventory);
        }
        else
        {
            InventoryItem newItem = new InventoryItem(itemData);
            inventory.Add(newItem);
            itemDictionary.Add(itemData, newItem);
            Debug.Log("Sample Item Collected");
            //Debug.Log($"Added {itemData.name} to the inventory");
            OnInventoryChange?.Invoke(inventory);
        }

        toolbarManager.UpdateInventoryVisualDisplay();
    }


    public void Remove(ItemData itemData)
    {
        if (itemDictionary.TryGetValue(itemData, out InventoryItem item))
        {

            item.RemoveFromStack();
            if(item.stacksize == 0)
            {
                inventory.Remove(item);
                itemDictionary.Remove(itemData);
            }
            OnInventoryChange?.Invoke(inventory);
        }

        toolbarManager.UpdateInventoryVisualDisplay();
    }

    public List<InventoryItem> GetListOfItems()
    {

        return inventory;
    }

}
