using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolBarUIScript : MonoBehaviour
{
    // https://www.youtube.com/watch?v=DUDmsFmKw8E&list=PL4PNgDjMajPN51E5WzEi7cXzJ16BCHZXl&index=14 

    // Attribution: Maceij Wolski + Dominic Rooney

    [Header("Required References")]
    public GameObject slotParentObject;
    public ToolbarSlotScript[] toolbarSlots; [Tooltip("Slots should be inserted into list like this: [0 1 2 3 4], with 0 being the slot furthest to the left, and 4 being the slot furthest to the right")]

    [Header("DO NOT EDIT - For Viewing Purposes Only")]
    [SerializeField] int selectedItem = 0;

    [HideInInspector] public static ToolBarUIScript Instance = null;

    #region Execution
    private void OnEnable()
    {
        UpdateSlots();
    }

    private void OnDisable()
    {
        UpdateSlots();
    }

    private void Start()
    {
        if(toolbarSlots.Length != 5)
        {
            Debug.LogError("There must be 5 toolbarSlots");
        }

        Instance = this;

        UpdateSlots();
    }

    #endregion

    #region Public Functions
    public bool ShiftSelectedSlot(int moveSelectionBy)
    {
        if (toolbarSlots.Length == 0)
        {
            return false;
        }

        int newSelectedIndex = selectedItem + moveSelectionBy;
        if (newSelectedIndex >= Inventory.instance.itemList.Count - 1)
        {
            selectedItem = 0;
            return true;
        }
        else if (selectedItem <= 0)
        {
            selectedItem = Inventory.instance.itemList.Count - 1;
            return true;
        }
        else
        {
            selectedItem = newSelectedIndex;
            return true;
        }
    }

    public void UpdateSlots()
    {
        if(Inventory.instance.itemList.Count == 0)
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

        // display selected item in the middle item slot
        toolbarSlots[2].ShowItemInSlot(Inventory.instance.itemList[selectedItem]);

        int numOfItemsShown = 1;
        int numOfNegativeLoops = 0;
        int numOfPositiveLoops = 0;

        // loop through the item slots on the left
        for (int slotIndex = 2; slotIndex > 0; slotIndex--)
        {
            if (numOfItemsShown >= Inventory.instance.itemList.Count)
            {
                // if there aren't anymore items to display, then hide the slot and skip
                toolbarSlots[slotIndex].Active = false;
                continue;
            }

            int loopAppendedSlotIndex = slotIndex;
            if (slotIndex + selectedItem < 0)
            {
                loopAppendedSlotIndex = Inventory.instance.itemList.Count - 1 - numOfNegativeLoops;
                numOfNegativeLoops++;
            }

            toolbarSlots[slotIndex].ShowItemInSlot(Inventory.instance.itemList[selectedItem - loopAppendedSlotIndex]);
            numOfItemsShown++;
        }

        // loop through the item slots on the right
        for (int slotIndex = 2; slotIndex < 4; slotIndex++)
        {
            if (numOfItemsShown >= Inventory.instance.itemList.Count)
            {
                // if there aren't anymore items to display, then hide the slot and skip
                toolbarSlots[slotIndex].Active = false;
                continue;
            }

            int loopAppendedSlotIndex = slotIndex;
            if (slotIndex + selectedItem > Inventory.instance.itemList.Count - 1)
            {
                loopAppendedSlotIndex = 0 + numOfPositiveLoops;
                numOfPositiveLoops++;
            }

            toolbarSlots[slotIndex].ShowItemInSlot(Inventory.instance.itemList[selectedItem + loopAppendedSlotIndex]);
            numOfItemsShown++;
        }
    }

    public void HideAllSlots(bool otherThanCenter = false)
    {
        for (int loop = 0; loop < toolbarSlots.Length; loop++)
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
    #endregion
}
