using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    // Attribution: Maceij Wolski + Dominic Rooney

    public GameObject itemActionContainerSceneObject;
    public List<InventoryItem> itemList = new List<InventoryItem>();
    public List<ToolbarSlotScript> slotList = new List<ToolbarSlotScript>();

    [Header("for viewing purposes")]
    public int itemSelectionCursor = 0;

    public static Inventory instance = null;

    bool newItemFlipFlop = false;

    void Awake()
    {
        instance = this;
    }

    // get data
    InventoryItem FindSelectedItem()
    {
        InventoryItem selectedItem = null;
        foreach (InventoryItem item in itemList)
        {
            if(item.slotIndex != 2) { continue; }
            selectedItem = item;
        }
        return selectedItem;
    }

    InventoryItem GetSelectedItem()
    {
        // error handling
        if(itemList == null) { return null; }
        if(itemList.Count == 0) { return null; }
        if(itemSelectionCursor < 0 || itemSelectionCursor > itemList.Count - 1)
        {
            ShiftSelectedSlot(0);
        }
        if (itemSelectionCursor < 0 || itemSelectionCursor > itemList.Count - 1)
        {
            return null;
        }

        // return code
        return itemList[itemSelectionCursor];
    }

    public TagList TagListOfCurrentItem()
    {
        return GetSelectedItem().itemData.tagList;
    }

    // select item
    public bool ShiftSelectedSlot(int moveSelectionBy)
    {
        if (itemList.Count <= 1)
        {
            UpdateUI(0);
            return false;
        }

        int newSelectedIndex = itemSelectionCursor + moveSelectionBy;
        if (newSelectedIndex > itemList.Count - 1)
        {
            newSelectedIndex = 0;
        }
        else if (newSelectedIndex < 0)
        {
            newSelectedIndex = itemList.Count - 1;
        }
        itemSelectionCursor = newSelectedIndex;

        UpdateUI(itemSelectionCursor);
        return true;
    }

    void UpdateUI(int selectedItemIndex)
    {
        // update slot indexes
        for(int loop = 0; loop < itemList.Count; loop++)
        {
            itemList[loop].slotIndex = 2 + (loop - selectedItemIndex);
        }

        // make slots show 
        List<int> displayingSlotsIndexes = new List<int>();
        for(int loop = 0; loop < itemList.Count; loop++)
        {
            int slotIndex = itemList[loop].slotIndex;
            if (slotIndex < 0 || slotIndex > 4) { continue; }

            slotList[slotIndex].ShowItemInSlot(itemList[loop]);
            displayingSlotsIndexes.Add(slotIndex);
        }

        // hide slots that don't have any item to show
        for(int loop = 0; loop < slotList.Count; loop++)
        {
            if(displayingSlotsIndexes.Contains(loop) == true) { continue; }

            slotList[loop].ShowEmptySlot();
        }
    }

    // Add + Remove items
    public void Add(ItemData itemData)
    {
        InventoryItem newItem = new InventoryItem(itemData);

        newItemFlipFlop = !newItemFlipFlop;
        if (newItemFlipFlop == false)
        {
            itemList.Add(newItem);
        }
        else
        {
            itemList.Insert(0, newItem);
        }
    }

    public void Remove(InventoryItem inventoryItem)
    {
        if(itemList.Count == 0)
        {
            return;
        }

        if(itemList.Contains(inventoryItem) == true)
        {
            itemList.Remove(inventoryItem);
        }

    }

    // Item Actions
    public bool LoadItemsActions(InventoryItem inventoryItem)
    {
        if(inventoryItem.itemActionsFailLoaded == true)
        {
            Debug.Log("Load aborted because item action has fail loaded before");
            return false;
        }

        if (inventoryItem.itemActionsLoaded == true)
        {
            Debug.Log("Item's actions are already loaded");
            return true;
        }

        if (inventoryItem.itemData.itemActionPrefab ?? null)
        {
        }
        else
        {
            inventoryItem.itemActionsFailLoaded = true;
            Debug.Log("No itemAction Prefab");
            return false;
        }

        GameObject instantiatedItemActionObject = Instantiate(inventoryItem.itemData.itemActionPrefab, itemActionContainerSceneObject.transform.position, itemActionContainerSceneObject.transform.rotation, itemActionContainerSceneObject.transform);
        inventoryItem.itemActions = instantiatedItemActionObject.GetComponents<IItemAction>();
        inventoryItem.itemActionsLoaded = true;

        if (inventoryItem.itemActions == null)
        {
            inventoryItem.itemActionsFailLoaded = true;
            Debug.Log("itemActions list is null, no interfaced components to get from GameObject");
            return false;
        }

        if (inventoryItem.itemActions.Length == 0)
        {
            inventoryItem.itemActionsFailLoaded = true;
            Debug.Log("itemActions list's length is 0, no interfaced components to get from GameObject");
            return false;
        }

        return true;
    }

    public bool TriggerItemActionOfSelectedItem(PlayerInventoryController playerInventoryController)
    {
        // error handling
        if (itemList.Count == 0)
        {
            Debug.Log("No items to trigger");
            return false;
        }

        InventoryItem selectedItem = GetSelectedItem();
        // error handling
        if(selectedItem == null)
        {
            UpdateUI(0);
            selectedItem = GetSelectedItem();
            Debug.Log("No selected item, updating item selection");
        }
        if (selectedItem == null)
        {
            Debug.LogWarning("Failed to get selected item after updating item selection");
            return false;
        }

        // try load
        bool loadSuccess = false;
        if (selectedItem.itemActionsLoaded == false)
        {
            Debug.Log("Loading selected item's actions");
            loadSuccess = LoadItemsActions(selectedItem);
        }
        else
        {
            loadSuccess = true;
        }

        // error handling
        if (loadSuccess == false)
        {
            Debug.Log("Failed to load selected item's actions");
            return false;
        }
        if (selectedItem.itemActions == null)
        {
            Debug.Log("Item has no list of actions to trigger");
            return false;
        }
        if (selectedItem.itemActions.Length == 0)
        {
            Debug.Log("action list has no actions in it");
            return false;
        }

        // Trigger the item's actions
        foreach (IItemAction itemAction in selectedItem.itemActions)
        {
            itemAction.TriggerItemAction(playerInventoryController);
        }
        return true;
    }
    
}
