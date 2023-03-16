using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryController : MonoBehaviour
{
    [Header("Required References")]
    public GameObject toolBarUIObject;

    // Update is called once per frame
    void Update()
    {
        OpenCloseInventoryInput();
        ItemSelectInput();
    }

    void OpenCloseInventoryInput()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (toolBarUIObject.activeInHierarchy)
            {

                toolBarUIObject.SetActive(false);
            }
            else
            {
                toolBarUIObject.SetActive(true);

            }
        }
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
