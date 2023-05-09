using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryController : MonoBehaviour
{
    public bool active = true;

    [Header("Required References")]
    public AudioSource inventorySwapLeftAudioSource;  
    public AudioSource inventorySwapRightAudioSource;

    [Header("Parameters")]
    public KeyCode itemActionKey;

    int itemActionsCheckedIn = 0;

    void Update()
    {
        if(itemActionsCheckedIn > 0)
        {
            return;
        }

        if (active == true)
        {
            ItemSelectInput();
            ItemActionInput();
        }
    }

    void ItemSelectInput()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            inventorySwapLeftAudioSource.Play(); 
            Inventory.instance.ShiftSelectedSlot(-1);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            inventorySwapRightAudioSource.Play();
            Inventory.instance.ShiftSelectedSlot(1);
        }
    }

    void ItemActionInput()
    {
        if (Input.GetKeyDown(itemActionKey) && itemActionsCheckedIn <= 0)
        {
            bool triggerSuccess = Inventory.instance.TriggerItemActionOfSelectedItem(this);
            if(triggerSuccess == true)
            {
                itemActionsCheckedIn++;
            }
        }
    }


    public void ItemActionExit()
    {
        itemActionsCheckedIn--;
    }
}
