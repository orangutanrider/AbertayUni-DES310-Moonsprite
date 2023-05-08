using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryController : MonoBehaviour
{
    public bool active = true;
    [Space]
    public KeyCode itemActionKey;

    int itemActionsCheckedIn = 0;

    public AudioSource inventorySwapAudioSource;    // Audio source for the inventory

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
            ToolBarUIScript.Instance.ShiftSelectedSlot(-1);
            inventorySwapAudioSource.Play(); 
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            ToolBarUIScript.Instance.ShiftSelectedSlot(1);
            inventorySwapAudioSource.Play();
        }
    }

    void ItemActionInput()
    {
        if (Input.GetKeyDown(itemActionKey) && itemActionsCheckedIn <= 0)
        {
            bool triggerSuccess = ToolBarUIScript.Instance.TriggerItemActionOfSelectedItem(this);
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
