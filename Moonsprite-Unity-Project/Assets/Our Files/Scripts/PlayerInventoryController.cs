using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryController : MonoBehaviour
{
    [Header("Required References")]
    public GameObject toolBarUIObject;

    // Start is called before the first frame update
    void Start()
    {
        
    }

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
