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
            ToolBarUIScript.Instance.ShiftSelectedSlot(-1);
            inventorySwapLeftAudioSource.Play(); 
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            ToolBarUIScript.Instance.ShiftSelectedSlot(1);
            inventorySwapRightAudioSource.Play();
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
