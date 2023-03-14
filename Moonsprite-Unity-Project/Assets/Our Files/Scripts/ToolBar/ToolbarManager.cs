using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolbarManager : MonoBehaviour
{
    //https://www.youtube.com/watch?v=DUDmsFmKw8E&list=PL4PNgDjMajPN51E5WzEi7cXzJ16BCHZXl&index=14 
    //this should give me sme ideas how to continue

    public GameObject slotPrefab;
    public Inventory playerInventory;
    public List<InventoryItem> inventoryItems = new List<InventoryItem>();
    public List<InventorySlots> inventorySlots = new List<InventorySlots>();

    public int selectedSlot = 0;


    private void OnEnable()
    {

        Inventory.OnInventoryChange += DrawnInventory;

    }

    private void OnDisable()
    {

        Inventory.OnInventoryChange -= DrawnInventory;

    }

    void ResetInventory()
    {
        foreach (Transform childTransform in transform)
        {
            Destroy(childTransform.gameObject);
        }
        inventorySlots = new List<InventorySlots>(1);
    }

    void DrawnInventory(List<InventoryItem> inventory)
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

    private void Update()
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

    private void FixedUpdate()
    {

        

        

            inventoryItems = FindObjectOfType<Inventory>().GetListOfItems();
            //playerInventory.GetComponent<Inventory>;
            DrawnInventory(inventoryItems);
        

        

    }

}
