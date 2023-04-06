using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerScreenTransitioner : MonoBehaviour
{
    [Header("Required Components")]
    public ImageFader doorTransitionImageFader;
    public ImageFader miscTransitionImageFader;
    public ImageFader sceneTransitionImageFader;

    [Header("Parameters")]
    public float doorTransitionTime;

    [HideInInspector] public static PlayerScreenTransitioner instance = null;

    void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("Error, two instances of PlayerScreenTransitioner in the scene");
        }

        instance = this;
    }

    public IEnumerator DoorTransition(float _doorTransitionTime)
    {
        // these number values have to be declared with a f otherwise it breaks

        doorTransitionImageFader.FadeIn(_doorTransitionTime * (1f / 3f)); 
        yield return new WaitForSeconds(_doorTransitionTime * (2f / 3f));
        doorTransitionImageFader.FadeOut(_doorTransitionTime * (1f / 3f));
    }

    public void FadeInMiscTransition(float transitionTime)
    {
        miscTransitionImageFader.FadeIn(transitionTime);
    }

    public void FadeOutMiscTransition(float transitionTime)
    {
        miscTransitionImageFader.FadeOut(transitionTime);
    }

    public IEnumerator FadeOutToNewScene(float _transitionTime, string _sceneName)
    {
        Scene sceneBeingLoaded = SceneManager.GetSceneByName(_sceneName);
        if(sceneBeingLoaded == null)
        {
            Debug.LogError("Was unable to get a scene by that name  (" + _sceneName + ")");
            yield break;
        }

        miscTransitionImageFader.FadeIn(_transitionTime * (1f / 3f));

        yield return new WaitForSeconds(_transitionTime * (1f / 3f));
        SceneManager.MoveGameObjectToScene(gameObject, sceneBeingLoaded);
        SceneManager.LoadScene(_sceneName, LoadSceneMode.Single);

        yield return new WaitForSeconds(_transitionTime * (1f / 3f));
        miscTransitionImageFader.FadeOut(_transitionTime * (1f / 3f));

        yield return new WaitForSeconds(_transitionTime * (1f / 3f));
        Destroy(gameObject);
    }
}
