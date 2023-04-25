using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerScreenTransitioner : MonoBehaviour
{
    [Header("Required References")]
    public ImageFader doorTransitionImageFader;
    [Space]
    public ImageFader ambulanceTransitionImageFader;
    public TextFader ambulanceTextFader;
    [Space]
    public ImageFader miscTransitionImageFader;

    [Header("Parameters")]
    public float doorTransitionTime;
    [Space]
    public float ambulanceImageFadeDuration;
    public float ambulanceTextFadeDuration;
    public float ambulanceTextFadeInDelay;

    [HideInInspector] public static PlayerScreenTransitioner instance = null;

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Error, two instances of PlayerScreenTransitioner in the scene");
        }

        instance = this;
    }

    public IEnumerator DoorTransition()
    {
        // these number values have to be declared with a f otherwise it breaks

        doorTransitionImageFader.FadeIn(doorTransitionTime * (1f / 3f));
        yield return new WaitForSeconds(doorTransitionTime * (2f / 3f));
        doorTransitionImageFader.FadeOut(doorTransitionTime * (1f / 3f));
    }

    public IEnumerator SquatterAmbulanceTransition()
    {
        // these number values have to be declared with a f otherwise it breaks

        ambulanceTransitionImageFader.FadeIn(ambulanceImageFadeDuration * (1f / 2f));
        yield return new WaitForSeconds(ambulanceImageFadeDuration * (1f / 2f));
        ambulanceTextFader.FadeIn(ambulanceTextFadeDuration * (1f / 2f));
        yield return new WaitForSeconds(ambulanceTextFadeInDelay + (ambulanceTextFadeDuration * (1f / 2f)));
        ambulanceTextFader.FadeOut(ambulanceTextFadeDuration * (1f / 2f));
        yield return new WaitForSeconds(ambulanceTextFadeDuration * (1f / 2f));
        ambulanceTransitionImageFader.FadeOut(ambulanceImageFadeDuration * (1f / 2f));
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
