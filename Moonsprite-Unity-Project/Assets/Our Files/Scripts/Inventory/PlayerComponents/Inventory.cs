using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    // there are still two bugs
    // one possibly with the squish function 
    // and one possibly with adding new items
    // it happens when you manually try to preserve an empty slot via having the knowledge that the add inserts at index 2

    // Attribution: Maceij Wolski + Dominic Rooney

    public GameObject itemActionContainerSceneObject;
    public List<InventoryItem> storedItemList = new List<InventoryItem>();
    public List<ToolbarSlotScript> slotList = new List<ToolbarSlotScript>();

    public static Inventory instance = null;

    bool firstFrame = true;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        // initialize item list
        InitializeItemList(ref storedItemList);
    }

    void Update()
    {
        // initialize toolbar display
        if(firstFrame == true)
        {
            firstFrame = false;
            UpdateUIDisplays();
        }
    }

    #region Data Functions
    void InitializeItemList(ref List<InventoryItem> itemList)
    {
        for (int loop = 0; loop < itemList.Count; loop++)
        {
            itemList[loop].previousIndex = loop;
        }
    }

    InventoryItem GetSelectedItem()
    {
        // error handling
        if(storedItemList == null) 
        {
            storedItemList = new List<InventoryItem>();
            Squish(storedItemList);
            UpdateUIDisplays();
        }
        if(storedItemList.Count < 5)
        {
            Squish(storedItemList);
            UpdateUIDisplays();
        }

        // return code
        return storedItemList[2];
    }

    int GetActualNumberOfItems(List<InventoryItem> itemList)
    {
        int numOfItems = 0;
        foreach(InventoryItem item in itemList)
        {
            if(item.empty == true) { continue; }
            numOfItems++;
        }
        return numOfItems;
    }

    List<InventoryItem> ShiftItemListRight(List<InventoryItem> itemList)
    {
        // create a new list to write to
        // and use the old list in the loop
        // replace the old list with the new list

        List<InventoryItem> squishedItemList = Squish(itemList);
        List<InventoryItem> newItemList = new List<InventoryItem>();
        for (int loop = 0; loop <= squishedItemList.Count - 1; loop++)
        {
            newItemList.Add(NewEmptyItem);
        }
        newItemList[0] = squishedItemList[squishedItemList.Count - 1];

        for (int loop = 1; loop <= squishedItemList.Count - 1; loop++)
        {
            newItemList[loop] = squishedItemList[loop - 1];
        }

        InitializeItemList(ref newItemList);
        return newItemList;
    }

    List<InventoryItem> ShiftItemListLeft(List<InventoryItem> itemList)
    {
        // create a new list to write to
        // and use the old list in the loop
        // replace the old list with the new list

        List<InventoryItem> squishedItemList = Squish(itemList);

        List<InventoryItem> newItemList = new List<InventoryItem>();
        for (int loop = squishedItemList.Count - 1; loop >= 0; loop--)
        {
            newItemList.Add(NewEmptyItem);
        }
        newItemList[squishedItemList.Count - 1] = squishedItemList[0];

        for (int loop = squishedItemList.Count - 1; loop >= 1; loop--)
        {
            newItemList[loop - 1] = squishedItemList[loop];
        }

        InitializeItemList(ref newItemList);
        return newItemList;
    }

    List<InventoryItem> Squish(List<InventoryItem> itemList)
    {
        if(itemList.Count <= 5) { return itemList; }

        int firstItemPreviousIndex = -1;
        List<InventoryItem> newItemList = new List<InventoryItem>();
        foreach (InventoryItem item in itemList)
        {
            if (item.empty == true) { continue; }
            newItemList.Add(item);
            if (firstItemPreviousIndex != -1) { continue; }
            firstItemPreviousIndex = item.previousIndex;
        }

        if(newItemList.Count >= 5) 
        {
            InitializeItemList(ref newItemList);
            return newItemList; 
        }

        // add empty slots to the start
        for (int loop = firstItemPreviousIndex; loop >= 0; loop--)
        {
            newItemList.Insert(0, NewEmptyItem);

            if (newItemList.Count >= 5)
            {
                InitializeItemList(ref newItemList);
                return newItemList;
            }
        }

        // add empty slots to the end
        for (int loop = newItemList.Count - 1; loop <= 5; loop++)
        {
            newItemList.Add(NewEmptyItem);
        }

        InitializeItemList(ref newItemList);
        return newItemList;
    }

    List<InventoryItem> ReconstructiveSquish(List<InventoryItem> itemList)
    {
        int firstItemPreviousIndex = -1;
        List<InventoryItem> newItemList = new List<InventoryItem>();
        foreach (InventoryItem item in itemList)
        {
            if (item.empty == true) { continue; }
            newItemList.Add(item);
            if (firstItemPreviousIndex != -1) { continue; }
            firstItemPreviousIndex = item.previousIndex;
        }

        if (newItemList.Count >= 5)
        {
            InitializeItemList(ref newItemList);
            return newItemList;
        }

        // add empty slots to the start
        for (int loop = firstItemPreviousIndex; loop >= 0; loop--)
        {
            newItemList.Insert(0, NewEmptyItem);

            if (newItemList.Count >= 5)
            {
                InitializeItemList(ref newItemList);
                return newItemList;
            }
        }

        // add empty slots to the end
        for (int loop = newItemList.Count - 1; loop <= 5; loop++)
        {
            newItemList.Add(NewEmptyItem);
        }

        InitializeItemList(ref newItemList);
        return newItemList;
    }

    List<InventoryItem> SquishWithoutAddingEmpties(List<InventoryItem> itemList)
    {
        List<InventoryItem> newItemList = new List<InventoryItem>();
        foreach (InventoryItem item in itemList)
        {
            if (item.empty == true) { continue; }
            newItemList.Add(item);
        }

        return newItemList;
    }

    InventoryItem NewEmptyItem
    {
        get
        {
            return new InventoryItem(null, true);
        }
    }

    void SetPreviousIndexesRelativeToItem(ref List<InventoryItem> itemList, int itemIndex)
    {
        int originItemPreviousIndex = itemList[itemIndex].previousIndex;

        int loopIterations = 1;
        for (int loop = originItemPreviousIndex + 1; loop <= itemList.Count - 1; loop++)
        {
            itemList[loop].previousIndex = originItemPreviousIndex + loopIterations;
            loopIterations++;
        }

        loopIterations = 1;
        for (int loop = originItemPreviousIndex - 1; loop >= 0; loop--)
        {
            itemList[loop].previousIndex = originItemPreviousIndex - loopIterations;
            loopIterations++;
        }
    }

    // Un-used
    /*
    int FindPositiveRelativeIndex(List<InventoryItem> itemList, InventoryItem item)
    {
        // this thing goes through the loop one by one until it reaches the item given
        // it returns a number for that item based on the indexes stored in the item list
        // positive refers to the direction of the search
        // it looks through the list from 0 to it's end

        if (itemList.Contains(item) == false)
        {
            Debug.LogWarning("Cannot find relative index for an item that is not in the list");
            return -1;
        }

        bool targetedItemReached = false;
        int previousIndexOfPreviousItem = itemList[0].previousIndex;
        for (int loop = 0; loop > itemList.Count - 1; loop++)
        {
            if (targetedItemReached == true) { continue; }
            if (itemList[loop] == item)
            {
                targetedItemReached = true;
                continue;
            }
            previousIndexOfPreviousItem = itemList[loop].previousIndex;
        }
        return previousIndexOfPreviousItem + 1;
    }

    int FindNegativeRelativeIndex(List<InventoryItem> itemList, InventoryItem item)
    {
        // this thing goes through the loop one by one until it reaches the item given
        // it returns a number for that item based on the indexes stored in the item list
        // negative refers to the direction of the search
        // it looks through the list from 0 to it's end

        if (itemList.Contains(item) == false)
        {
            Debug.LogWarning("Cannot find relative index for an item that is not in the list");
            return -1;
        }

        bool targetedItemReached = false;
        int previousIndexOfPreviousItem = itemList[0].previousIndex;
        for (int loop = itemList.Count - 1; loop < 0; loop--)
        {
            if (targetedItemReached == true) { continue; }
            if (itemList[loop] == item)
            {
                targetedItemReached = true;
                continue;
            }
            previousIndexOfPreviousItem = itemList[loop].previousIndex;
        }
        return previousIndexOfPreviousItem - 1;
    }
    */
    #endregion

    void UpdateUIDisplays()
    {
        storedItemList = Squish(storedItemList);
        Debug.Log(storedItemList.Count);
        for (int loop = 0; loop <= 4; loop++)
        {
            if (storedItemList[loop].empty == true)
            {
                slotList[loop].ShowEmptySlot();
                continue;
            }

            if (storedItemList[loop].empty == true)
            {
                slotList[loop].ShowEmptySlot();
                continue;
            }

            if(storedItemList[loop].itemData == null)
            {
                slotList[loop].ShowEmptySlot();
                storedItemList[loop].empty = true;
                continue;
            }

            slotList[loop].ShowItemInSlot(storedItemList[loop]);
        }
    }

    public void ShiftSelectedSlot(int shiftValue)
    {
        if(shiftValue == 0)
        {
            storedItemList = Squish(storedItemList);
        }
        if(shiftValue >= 1)
        {
            storedItemList = ShiftItemListRight(storedItemList);
        }
        if(shiftValue <= -1)
        {
            storedItemList = ShiftItemListLeft(storedItemList);
        }
        UpdateUIDisplays();
    }

    public TagList TagListOfCurrentItem()
    {
        InventoryItem item = GetSelectedItem();
        if (item.itemData == null)
        {
            return null;
        }
        return GetSelectedItem().itemData.tagList;
    }

    public void Add(ItemData itemData)
    {
        InventoryItem newItem = new InventoryItem(itemData, false);
        newItem.previousIndex = 2;

        InitializeItemList(ref storedItemList);
        List<InventoryItem> justItemsList = SquishWithoutAddingEmpties(storedItemList);

        int insertPoint = 0;
        insertPoint = Mathf.FloorToInt((justItemsList.Count - 1) / 2);
        insertPoint = Mathf.Clamp(insertPoint, 0, 2);
        justItemsList.Insert(insertPoint, newItem);

        SetPreviousIndexesRelativeToItem(ref justItemsList, insertPoint);

        storedItemList = ReconstructiveSquish(justItemsList);
        UpdateUIDisplays();

        // flip flop pattern of adding items
        /*
        int actualNumberOfItems = GetActualNumberOfItems(newItemList); 
         
        if (actualNumberOfItems >= 5)
        {
            newItemList.Add(newItem);
            InitializeItemList(ref newItemList);
            storedItemList = newItemList;
            return;
        }

        int relativeIndex = 2;
        if(newItemFlipFlop == true)
        {
            newItemList.Add(newItem);
            relativeIndex = FindPositiveRelativeIndex(newItemList, newItem);
            newItem.previousIndex = relativeIndex;
        }
        else
        {
            newItemList.Insert(0, newItem);
            relativeIndex = FindNegativeRelativeIndex(newItemList, newItem);
            newItem.previousIndex = relativeIndex;
        }
        newItemFlipFlop = !newItemFlipFlop;
        */
    }

    public void Remove(InventoryItem item)
    {
        if(item.empty == true) 
        {
            Debug.Log("This function is built for removing items, not empty slots");
            return; 
        }

        if (storedItemList.Contains(item) == false)
        {
            Debug.Log("Item wasn't found in the stored item list");
            return;
        }

        storedItemList.Remove(item);
        Squish(storedItemList);
    }

    // Item Actions
    public bool LoadItemsActions(InventoryItem inventoryItem)
    {
        #region Error Handling
        if (inventoryItem.itemActionsFailLoaded == true)
        {
            Debug.Log("Load aborted because item action has fail loaded before");
            return false;
        }
        if (inventoryItem.itemActionsLoaded == true)
        {
            Debug.Log("Item's actions are already loaded");
            return true;
        }
        if (inventoryItem.itemData.itemActionPrefab ?? null){}
        else
        {
            inventoryItem.itemActionsFailLoaded = true;
            Debug.Log("No itemAction Prefab");
            return false;
        }
        #endregion

        // load actions
        GameObject instantiatedItemActionObject = Instantiate(inventoryItem.itemData.itemActionPrefab, itemActionContainerSceneObject.transform.position, itemActionContainerSceneObject.transform.rotation, itemActionContainerSceneObject.transform);
        inventoryItem.itemActions = instantiatedItemActionObject.GetComponents<IItemAction>();
        inventoryItem.itemActionsLoaded = true;

        #region Error Handling
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
        #endregion

        return true;
    }

    public bool TriggerItemActionOfSelectedItem(PlayerInventoryController playerInventoryController)
    {
        int actualNumberOfItems = GetActualNumberOfItems(storedItemList);
        if(actualNumberOfItems <= 1)
        {
            // (player hands are an item, hence 1 item is still no items)
            Debug.Log("No items to trigger");
            return false;
        }

        InventoryItem selectedItem = GetSelectedItem();
        if(selectedItem.empty == true)
        {
            Debug.Log("Selected item is empty");
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
            // (it is already loaded)
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
