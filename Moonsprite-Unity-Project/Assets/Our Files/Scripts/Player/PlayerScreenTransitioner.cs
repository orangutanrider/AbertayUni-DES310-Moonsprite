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
    public float ambulanceImageFadeInDelay;
    public float ambulanceImageFadeDuration;
    public float ambulanceTextFadeDuration;
    public float ambulanceTextFadeOutDelay;
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

        yield return new WaitForSecondsRealtime(ambulanceImageFadeInDelay);
        ambulanceTransitionImageFader.FadeIn(ambulanceImageFadeDuration);
        yield return new WaitForSecondsRealtime(ambulanceImageFadeDuration + ambulanceTextFadeInDelay);
        ambulanceTextFader.FadeIn(ambulanceTextFadeDuration);
        yield return new WaitForSecondsRealtime(ambulanceTextFadeDuration);
        yield return new WaitForSecondsRealtime(ambulanceTextFadeOutDelay);
        ambulanceTextFader.FadeOut(ambulanceTextFadeDuration);
        yield return new WaitForSecondsRealtime(ambulanceTextFadeDuration);
        ambulanceTransitionImageFader.FadeOut(ambulanceImageFadeDuration);
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
