using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TogglePauseMenu : MonoBehaviour
{
    public GameObject pauseMenuObject;

    bool paused = false;
    const KeyCode pauseKey = KeyCode.Escape;

    void Update()
    {
        if(paused == false)
        {
            if(Input.GetKeyDown(pauseKey) == true)
            {
                paused = true;
                PauseEnabled();
            }
        }
        else
        {
            if (Input.GetKeyDown(pauseKey) == true)
            {
                paused = false;
                PauseDisabled();
            }
        }
    }

    void Start()
    {
        PauseDisabled();
    }

    void PauseEnabled()
    {
        pauseMenuObject.SetActive(true);
        Time.timeScale = 0;
    }

    void PauseDisabled()
    {
        pauseMenuObject.SetActive(false);
        Time.timeScale = 1;
    }
}
