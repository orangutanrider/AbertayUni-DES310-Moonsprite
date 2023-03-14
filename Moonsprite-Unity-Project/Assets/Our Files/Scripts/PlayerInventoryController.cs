using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryController : MonoBehaviour
{
    [Header("Required References")]
    public GameObject inventoryPanel;
    public GameObject toolbar;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (inventoryPanel.activeInHierarchy)
            {

                inventoryPanel.SetActive(false);
            }
            else
            {
                inventoryPanel.SetActive(true);

            }
        }
    }
}
