using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryController : MonoBehaviour
{
    void Update()
    {
        ItemSelectInput();
    }

    void ItemSelectInput()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ToolBarUIScript.Instance.ShiftSelectedSlot(-1);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            ToolBarUIScript.Instance.ShiftSelectedSlot(1);
        }
    }
}
