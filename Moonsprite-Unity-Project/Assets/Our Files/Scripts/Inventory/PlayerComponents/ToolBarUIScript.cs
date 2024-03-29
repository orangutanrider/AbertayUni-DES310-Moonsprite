using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolBarUIScript : MonoBehaviour
{
    /*
    // https://www.youtube.com/watch?v=DUDmsFmKw8E&list=PL4PNgDjMajPN51E5WzEi7cXzJ16BCHZXl&index=14 

    // Attribution: Maceij Wolski + Dominic Rooney

    [Header("Required References")]
    public List<ToolbarSlotScript> toolbarSlots = new List<ToolbarSlotScript>(); [Tooltip("Slots should be inserted into list like this: [0 1 2 3 4], with 0 being the slot furthest to the left, and 4 being the slot furthest to the right")]

    [Header("For Viewing Purposes")]
    [SerializeField] int selectedItem = 0;

    [HideInInspector] public static ToolBarUIScript instance = null;

    //bool firstOnEnable = true;

    #region Execution
    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        if(toolbarSlots.Count != 5)
        {
            Debug.LogWarning("There must be 5 toolbarSlots");
        }

        UpdateSlots();
    }

    #endregion

    [Tooltip("Indexes must be given in order (e.g. -3, -2, -1, 0, 1, 2, 3, ect.)")]
    List<int> LoopListOfIndex(List<int> indexList, int maxIndex)
    {
        List<int> valid = new List<int>();
        List<int> invalidNegatives = new List<int>();
        List<int> invalidPositives = new List<int>();
        foreach (int index in indexList)
        {
            if (index < 0)
            {
                invalidNegatives.Add(index);
            }
            else if (index > maxIndex)
            {
                invalidPositives.Add(index);
            }
            else
            {
                valid.Add(index);
            }
        }

        invalidNegatives.Reverse();

        foreach (int negativeIndex in invalidNegatives)
        {
            valid.Insert(0, negativeIndex + maxIndex + 1);
        }

        foreach (int positiveIndex in invalidPositives)
        {
            valid.Add(positiveIndex - (maxIndex + 1));
        }

        return valid;
    }

    #region Public Functions
    public bool ShiftSelectedSlot(int moveSelectionBy)
    {
        if (toolbarSlots.Count <= 1)
        {
            UpdateSlots();
            return false;
        }

        int newSelectedIndex = selectedItem + moveSelectionBy;
        if (newSelectedIndex > Inventory.instance.itemList.Count - 1)
        {
            selectedItem = 0;
            UpdateSlots();
            return true;
        }
        else if (newSelectedIndex < 0)
        {
            selectedItem = Inventory.instance.itemList.Count - 1;
            UpdateSlots();
            return true;
        }
        else
        {
            selectedItem = newSelectedIndex;
            UpdateSlots();
            return true;
        }
    }

    public void UpdateSlots()
    {
        #region 1 or less item
        if (Inventory.instance.itemList.Count == 0)
        {
            // if there are no items, then hide all slots
            HideAllSlots(false);
            return;
        }
        if (Inventory.instance.itemList.Count == 1)
        {
            // if there is only one item then don't do anything other than display that one item
            HideAllSlots(true);
            toolbarSlots[2].ShowItemInSlot(Inventory.instance.itemList[selectedItem]);
            return;
        }
        #endregion

        // calculate the bounds of the current toolbar
        int hiddenSlotsRight = 2;
        int shownSlotsRight = 0;
        int hiddenSlotsLeft = 2;
        int shownSlotsLeft = 0;
        for (int loop = 1; loop < Inventory.instance.itemList.Count; loop++)
        {
            if (loop % 2 != 0 && hiddenSlotsRight != 0) // if num is odd
            {
                hiddenSlotsRight--;
                shownSlotsRight++;
            }
            if (loop % 2 == 0 && hiddenSlotsLeft != 0) // if num is even
            {
                hiddenSlotsLeft--;
                shownSlotsLeft++;
            }
        }

        // calculate the surrounding indexes of the selected item
        List<int> indexList = new List<int>();
        int totalLoopIterations = 0;
        for (int loop = hiddenSlotsLeft; loop <= 4 - hiddenSlotsRight; loop++)
        {
            int currentItemIndex = (selectedItem - shownSlotsLeft) + totalLoopIterations;
            indexList.Add(currentItemIndex);
            totalLoopIterations++;
        }

        // loop the indexes
        List<int> loopedIndexList = LoopListOfIndex(indexList, Inventory.instance.itemList.Count - 1);

        // display the items
        totalLoopIterations = 0;
        for (int loop = hiddenSlotsLeft; loop <= 4 - hiddenSlotsRight; loop++)
        {
            toolbarSlots[loop].ShowItemInSlot(Inventory.instance.itemList[loopedIndexList[totalLoopIterations]]);
            totalLoopIterations++; 
        }
    }

    public void HideAllSlots(bool otherThanCenter = false)
    {
        for (int loop = 0; loop < toolbarSlots.Count; loop++)
        {
            if(otherThanCenter == true && loop == 2)
            {
                toolbarSlots[loop].Active = true;
                continue;
            }
            toolbarSlots[loop].Active = false;
        }
    }

    public TagList GetTagListOfSelectedItem()
    {
        if (Inventory.instance.itemList.Count == 0)
        {
            return null;
        }

        return Inventory.instance.itemList[selectedItem].itemData.tagList;
    }

    public bool TriggerItemActionOfSelectedItem(PlayerInventoryController playerInventoryController)
    {
        if (Inventory.instance.itemList.Count == 0)
        {
            Debug.Log("No items to trigger");
            return false;
        }

        bool loadSuccess = false;
        if(Inventory.instance.itemList[selectedItem].itemActionsLoaded == false)
        {
            Debug.Log("Loading selected item's actions");
            loadSuccess = Inventory.instance.LoadItemsActions(Inventory.instance.itemList[selectedItem]);
        }
        else
        {
            loadSuccess = true;
        }

        if(loadSuccess == false)
        {
            Debug.Log("Failed to load selected item's actions");
            return false;
        }
        if(Inventory.instance.itemList[selectedItem].itemActions == null)
        {
            Debug.Log("Item has no list of actions to trigger");
            return false;
        }
        if (Inventory.instance.itemList[selectedItem].itemActions.Length == 0)
        {
            Debug.Log("action list has no actions in it");
            return false;
        }

        // Trigger the item's actions
        foreach(IItemAction itemAction in Inventory.instance.itemList[selectedItem].itemActions)
        {
            itemAction.TriggerItemAction(playerInventoryController);
        }
        return true;
    }
    #endregion
    */
}
