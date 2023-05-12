using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfirmationUIManger : MonoBehaviour
{
    public static ConfirmationUIManger Instance
    {
        get
        {
            return instance;
        }
        private set
        {
            instance = value;
        }
    }
    private static ConfirmationUIManger instance = null;

    [Header("Required References")]
    public GameObject uiContainer = null;

    IConfirmationStep scriptAskingForConfirmation = null;
    bool active = false;

    private void Awake()
    {
        instance = this;
    }

    public bool AskForConfirmation(IConfirmationStep yourScript)
    {
        if(active == true)
        {
            Debug.LogWarning("Cannot confirm while another script is asking for confirmation");
            return false;
        }
        if(yourScript == null) 
        {
            Debug.LogWarning("Value given was null");
            return false; 
        }

        active = true;
        scriptAskingForConfirmation = yourScript;
        uiContainer.SetActive(true);
        return true;
    }

    public void ConfirmButtonPress()
    {
        scriptAskingForConfirmation.Confirm();
        active = false;
        uiContainer.SetActive(false);
    }
    public void ReturnButtonPress()
    {
        scriptAskingForConfirmation.Return();
        active = false;
        uiContainer.SetActive(false);
    }
}
