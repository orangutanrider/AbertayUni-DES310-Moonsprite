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

    bool firstOnEnable = true;

    #region Execution
    private void OnEnable()
    {
        if (firstOnEnable == false)
        {
            UpdateSlots();
        }
        firstOnEnable = false;
    }

    private void OnDisable()
    {
        UpdateSlots();
    }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if(toolbarSlots.Length != 5)
        {
            Debug.LogError("There must be 5 toolbarSlots");
        }

        UpdateSlots();
    }

    #endregion

    #region Public Functions
    public bool ShiftSelectedSlot(int moveSelectionBy)
    {
        if (toolbarSlots.Length <= 1)
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
        List<int> loopedIndexList = loopListOfIndex(indexList, Inventory.instance.itemList.Count - 1);

        // display the items
        totalLoopIterations = 0;
        for (int loop = hiddenSlotsLeft; loop <= 4 - hiddenSlotsRight; loop++)
        {
            toolbarSlots[loop].ShowItemInSlot(Inventory.instance.itemList[loopedIndexList[totalLoopIterations]]);
            totalLoopIterations++; 
        }
    }

    [Tooltip("Indexes must be given in order (e.g. -3, -2, -1, 0, 1, 2, 3, ect.)")]
    List<int> loopListOfIndex(List<int> indexList, int maxIndex) 
    {
        List<int> valid = new List<int>();
        List<int> invalidNegatives = new List<int>();
        List<int> invalidPositives = new List<int>();
        foreach (int index in indexList)
        {
            if(index < 0)
            {
                invalidNegatives.Add(index);
            }
            else if(index > maxIndex)
            {
                invalidPositives.Add(index);
            }
            else
            {
                valid.Add(index);
            }
        }

        invalidNegatives.Reverse();

        foreach(int negativeIndex in invalidNegatives)
        {
            valid.Insert(0, negativeIndex + maxIndex + 1);
        }

        foreach(int positiveIndex in invalidPositives)
        {
            valid.Add(positiveIndex - (maxIndex + 1));
        }

        return valid;
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

    [System.Obsolete()]
    public void OldUpdateSlots()
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

        // display selected item in the middle item slot
        toolbarSlots[2].ShowItemInSlot(Inventory.instance.itemList[selectedItem]);

        int evenLoop = 0;
        int oddLoop = 0;
        for (int loop = 0; loop < 4; loop++)
        {
            if (loop >= Inventory.instance.itemList.Count - 1)
            {
                Debug.Log("skip");
                continue;
            }

            if (loop % 2 == 0) // if num is even
            {
                evenLoop++;

                int loopedItemSelect = selectedItem + evenLoop;
                if (loopedItemSelect > Inventory.instance.itemList.Count - 1)
                {
                    loopedItemSelect = loopedItemSelect - (Inventory.instance.itemList.Count - 1);
                }

                Debug.Log("even " + loopedItemSelect);

                toolbarSlots[2 + evenLoop].ShowItemInSlot(Inventory.instance.itemList[loopedItemSelect]);
            }
            else // if num is odd
            {
                oddLoop++;

                int loopedItemSelect = selectedItem - oddLoop;
                if (loopedItemSelect < 0)
                {
                    loopedItemSelect = Inventory.instance.itemList.Count + loopedItemSelect;
                }

                Debug.Log("odd " + loopedItemSelect);

                toolbarSlots[2 - oddLoop].ShowItemInSlot(Inventory.instance.itemList[loopedItemSelect]);
            }
        }
    }
}
