using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
                Pause();
            }
        }
        else
        {
            if (Input.GetKeyDown(pauseKey) == true)
            {
                UnPause();
            }
        }
    }

    void Start()
    {
        UnPause();
    }

    void Pause()
    {
        paused = true;
        pauseMenuObject.SetActive(true);
        Time.timeScale = 0;
    }

    void UnPause()
    {
        paused = false;
        pauseMenuObject.SetActive(false);
        Time.timeScale = 1;
    }

    public void ResumeButton()
    {
        UnPause();
    }

    public void MainMenuButton()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneReferenceHolder.mainMenuScene, LoadSceneMode.Single);
    }
}
