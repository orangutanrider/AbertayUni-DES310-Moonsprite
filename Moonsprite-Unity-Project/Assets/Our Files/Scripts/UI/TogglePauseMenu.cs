using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TogglePauseMenu : MonoBehaviour
{
    public GameObject pauseMenuObject;

    public void ShowPauseMenu()
    {
        pauseMenuObject.SetActive(true);
    }

    public void HidePauseMenu()
    {
        pauseMenuObject.SetActive(false);
    }
}
