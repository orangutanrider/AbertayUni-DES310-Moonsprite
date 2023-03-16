using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolbarManager : MonoBehaviour
{
    // this script handles the question of what item is selected
    // and the toolbar ui
    // it doesn't store the items

    [Header("Required References")]
    public GameObject slotPrefab;

    int selectedSlot;

    [HideInInspector] public static ToolbarManager Instance = null;

    #region Execution
    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    #endregion

    public bool ShiftSelectedSlot(int moveSelectionBy)
    {
        return false;
        /*
        if (inventorySlots.Count == 0)
        {
            return false;
        }

        int newSlotIndex = selectedSlot + moveSelectionBy;
        if (newSlotIndex >= inventoryItems.Count - 1)
        {
            selectedSlot = 0;
            return true;
        }
        else if (selectedSlot <= 0)
        {
            selectedSlot = inventoryItems.Count - 1;
            return true;
        }
        else
        {
            selectedSlot = newSlotIndex;
            return true;
        }
        */
    }
    
    public TagList GetTagListOfActiveItem()
    {
        if (Inventory.Instance.inventory.Count == 0)
        {
            return null;
        }

        return Inventory.Instance.inventory[selectedSlot].tagList;
    }
}
