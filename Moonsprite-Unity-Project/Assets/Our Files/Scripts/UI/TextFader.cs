using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextFader : MonoBehaviour
{
    [Header("Required References")]
    public TMPro.TextMeshPro tmpElement;

    bool fadeDirection = false; // false = fading out; true = fading in
    float fadeTimer = -1;
    float fadeTime = -1;

    void Update()
    {
        if (fadeTimer >= 0 && fadeTimer <= fadeTime)
        {
            Fade();
        }
    }

    void Fade()
    {
        if (fadeDirection == true)
        {
            fadeTimer = fadeTimer + Time.deltaTime;
        }
        if (fadeDirection == false)
        {
            fadeTimer = fadeTimer - Time.deltaTime;
        }

        float newAlpha = Mathf.Lerp(0, 1, fadeTimer / fadeTime);
        tmpElement.color = new Color(tmpElement.color.r, tmpElement.color.g, tmpElement.color.b, newAlpha);
    }

    public void FadeOut(float fadeDuration)
    {
        fadeDirection = false;
        fadeTimer = fadeDuration;
        fadeTime = fadeDuration;
    }

    public void FadeIn(float fadeDuration)
    {
        fadeDirection = true;
        fadeTimer = 0;
        fadeTime = fadeDuration;
    }
}
