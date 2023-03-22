using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestItemActionScript : MonoBehaviour, IItemAction
{
    void IItemAction.TriggerItemAction(PlayerInventoryController playerInventoryController )
    {
        Debug.Log("Item action test succesful");
        playerInventoryController.ItemActionExit();
    }
}
