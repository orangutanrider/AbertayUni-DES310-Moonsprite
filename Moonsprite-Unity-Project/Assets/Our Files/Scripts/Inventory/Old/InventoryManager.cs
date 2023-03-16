using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Obsolete("This script is deprecated, replaced by toolbar manager")]
public class InventoryManager : MonoBehaviour
{
    public GameObject slotPrefab;
    public Inventory playerInventory;
    public List<InventoryItem> inventoryItems = new List<InventoryItem>();
    public List<InventorySlots> inventorySlots = new List<InventorySlots>(); // this list is limited, however the inventory space is not, work out how to fix that
                                                                               // public GameObject inventoryPanel;

    public int slotNumber = 6;

    #region Execution
    private void OnEnable()
    {
        
        Inventory.OnInventoryChange += DrawnInventory;

    }

    private void OnDisable()
    {

        Inventory.OnInventoryChange -= DrawnInventory;

    }

    private void FixedUpdate()
    {

        if (Time.frameCount % 60 == 0)
        {

            inventoryItems = FindObjectOfType<Inventory>().GetListOfItems();
            //playerInventory.GetComponent<Inventory>;
            DrawnInventory(inventoryItems);
        }
    }
    #endregion


    void ResetInventory()
    {
        foreach (Transform childTransform in transform)
        {
            Destroy(childTransform.gameObject);
        }
        inventorySlots = new List<InventorySlots>(slotNumber);
    }

    void DrawnInventory(List<InventoryItem> inventory)
    {
        

        ResetInventory();

        for (int i = 0; i < inventorySlots.Capacity; i++)
        {

            CreateInventorySlot();

        }


        for (int x = 0; x < inventory.Count; x++)
        {

            inventorySlots[x].DrawSlot(inventory[x]);

        }


    }

    void CreateInventorySlot()
    {

        GameObject newSlot = Instantiate(slotPrefab);

        newSlot.transform.SetParent(transform, false);

        InventorySlots newSlotComponent = newSlot.GetComponent<InventorySlots>();

        newSlotComponent.ClearSlot();

        inventorySlots.Add(newSlotComponent);

    }
}

