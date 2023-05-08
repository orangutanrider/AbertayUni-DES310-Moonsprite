using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryController : MonoBehaviour
{
    public bool active = true;
    [Space]
    public KeyCode itemActionKey;

    int itemActionsCheckedIn = 0;

    public AudioSource inventorySwapLeftAudioSource;    // Audio source for the inventory left on the hot bar
    public AudioSource inventorySwapRightAudioSource;    // Audio source for the inventory Right on the hot bar

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
