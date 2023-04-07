using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestructingFadeOutScript : MonoBehaviour
{
    [Header("Required References")]
    public ImageFader imageFader;

    [Header("OnStart")]
    public bool fadeOnstart = false;
    public float fadeDuration = 1;
    public float delay = 1;

    void Start()
    {
        if(fadeOnstart == true)
        {
            StartCoroutine(FadeMeOutIn(delay, fadeDuration));
        }
    }

    public IEnumerator FadeMeOutIn(float seconds, float fadeDuration)
    {
        yield return new WaitForSeconds(seconds);
        FadeMeOut(fadeDuration);
    }

    public void FadeMeOut(float duration)
    {
        imageFader.FadeOut(duration);
        StartCoroutine(DestroyInX(duration));
    }

    IEnumerator DestroyInX(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Destroy(gameObject);
    }
}
