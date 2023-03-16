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
}
