using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScreenTransitioner : MonoBehaviour
{
    [Header("Required Components")]
    public ImageFader doorTransitionImageFader;
    public ImageFader miscTransitionImageFader;

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
}
