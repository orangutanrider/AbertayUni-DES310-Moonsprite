using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolBarUIScript : MonoBehaviour
{
    //https://www.youtube.com/watch?v=DUDmsFmKw8E&list=PL4PNgDjMajPN51E5WzEi7cXzJ16BCHZXl&index=14 
    //this should give me sme ideas how to continue

    [Header("Required References")]
    public GameObject slotPrefab;
    public GameObject slotParentObject;
    public Inventory playerInventory;

    [Header("DO NOT EDIT - For Viewing Purposes Only")]
    [SerializeField] List<InventoryItem> inventoryItems = new List<InventoryItem>();
    [SerializeField] List<InventorySlots> inventorySlots = new List<InventorySlots>();
    [SerializeField] int selectedSlot = 0;

    [HideInInspector] public static ToolBarUIScript Instance = null;

    #region Execution
    private void OnEnable()
    {

        Inventory.OnInventoryChange += DrawInventory;

    }

    private void OnDisable()
    {

        Inventory.OnInventoryChange -= DrawInventory;

    }

    private void Start()
    {
        Instance = this;
    }

    #endregion

    [Tooltip("Call this whenever the inventory is updated")]
    public void UpdateInventoryVisualDisplay()
    {
        //inventoryItems = playerInventory.GetListOfItems();
        //playerInventory.GetComponent<Inventory>;

        inventoryItems = playerInventory.GetListOfItems();
        if (inventoryItems.Count > 0)
        {
            DrawInventory(inventoryItems);
        }
    }

    void ResetInventory()
    {
        foreach (Transform childTransform in slotParentObject.transform)
        {
            Destroy(childTransform.gameObject);
        }
        inventorySlots = new List<InventorySlots>(1);
    }

    void DrawInventory(List<InventoryItem> inventory)
    {


        ResetInventory();

        for (int i = 0; i < inventorySlots.Capacity; i++)
        {

            CreateInventorySlot();

        }

        // for (int x = 0; x < inventory.Count; x++)
        //{

        inventorySlots[0].DrawSlot(inventory[selectedSlot]);

        //}


    }

    void CreateInventorySlot()
    {

        GameObject newSlot = Instantiate(slotPrefab, slotParentObject.transform);

        newSlot.transform.SetParent(transform, false);

        InventorySlots newSlotComponent = newSlot.GetComponent<InventorySlots>();

        newSlotComponent.ClearSlot();

        inventorySlots.Add(newSlotComponent);

    }

    public bool ShiftSelectedSlot(int moveSelectionBy)
    {
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
    }

    public TagList GetTagListOfActiveItem()
    {
        if (inventoryItems.Count == 0)
        {
            return null;
        }

        return inventoryItems[selectedSlot].itemData.tagList;
    }
}
