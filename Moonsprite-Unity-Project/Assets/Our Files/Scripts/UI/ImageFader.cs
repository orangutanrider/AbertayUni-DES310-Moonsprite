using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageFader : MonoBehaviour
{
    [Header("Required References")]
    public Image image;

    bool fadeDirection = false; // false = fading out; true = fading in
    float fadeTimer = 0;
    float fadeTime = 0;

    void Update()
    {
        if(fadeTimer > 0 && fadeTimer <= fadeTime)
        {
            Fade();
        }
    }

    void Fade()
    {
        if(fadeDirection == true)
        {
            fadeTimer = fadeTimer - Time.deltaTime;
        }
        if(fadeDirection == false)
        {
            fadeTimer = fadeTimer + Time.deltaTime;
        }

        float newAlpha = Mathf.Lerp(0, 1, fadeTimer / fadeTime);
        image.color = new Color(image.color.r, image.color.g, image.color.b, newAlpha);
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
