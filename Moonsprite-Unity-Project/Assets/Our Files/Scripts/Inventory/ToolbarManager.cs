using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolbarManager : MonoBehaviour
{
    //https://www.youtube.com/watch?v=DUDmsFmKw8E&list=PL4PNgDjMajPN51E5WzEi7cXzJ16BCHZXl&index=14 
    //this should give me sme ideas how to continue

    [Header("Required References")]
    public GameObject slotPrefab;
    public Inventory playerInventory;

    [Header("DO NOT EDIT - For Viewing Purposes Only")]
    public List<InventoryItem> inventoryItems = new List<InventoryItem>();
    public List<InventorySlots> inventorySlots = new List<InventorySlots>();
    public int selectedSlot = 0;

    [HideInInspector] public static ToolbarManager Instance = null;

    #region Execution
    private void OnEnable()
    {

        Inventory.OnInventoryChange += DrawInventory;

    }

    private void OnDisable()
    {

        Inventory.OnInventoryChange -= DrawInventory;

    }

    private void Update()
    {
        ItemSelectInput();
    }

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("There should only be one toolbar manager per scene");
        }
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
        foreach (Transform childTransform in transform)
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

        GameObject newSlot = Instantiate(slotPrefab);

        newSlot.transform.SetParent(transform, false);

        InventorySlots newSlotComponent = newSlot.GetComponent<InventorySlots>();

        newSlotComponent.ClearSlot();

        inventorySlots.Add(newSlotComponent);

    }

    void ItemSelectInput()
    {
        if (inventorySlots.Count > 0)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {


                if (selectedSlot >= inventoryItems.Count - 1)
                    selectedSlot = 0;
                else
                    selectedSlot += 1;

            }

            if (Input.GetKeyDown(KeyCode.E))
            {


                if (selectedSlot <= 0)
                    selectedSlot = inventoryItems.Count - 1;
                else
                    selectedSlot -= 1;
            }
        }
        else
        {
            selectedSlot = 0;
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
