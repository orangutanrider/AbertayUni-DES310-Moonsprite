using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 1;

    public AudioSource ButtonAudio;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // transition.SetTrigger("TransitionStart");
            //Invoke("LoadNextLevel", 1f);
        }

    }

    public void StartTransition()
    {
        PlayButtonAudio();
        transition.SetTrigger("TransitionStart");
        Invoke("LoadNextLevel", 1f);
        Debug.Log("pressed");
    }
    void LoadNextLevel()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }
    IEnumerator LoadLevel(int levelIndex)
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(levelIndex);
    }
    public void PlayButtonAudio()
    {
        ButtonAudio.Play();
    }
}
