using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public static class FadeMixerGroup
{
    // https://johnleonardfrench.com/how-to-fade-audio-in-unity-i-tested-every-method-this-ones-the-best/

    // use these by using StartCoroutine(...)

    // lerps the raw threshold volume value in dB
    public static IEnumerator FadeMixerGroupRawThresholdVolume(AudioMixer audioMixer, string exposedParam, float targetThresholdVolume, float duration)
    {
        float currentTime = 0;
        audioMixer.GetFloat(exposedParam, out float previousThresholdVolume);
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            float newThresholdVol = Mathf.Lerp(previousThresholdVolume, targetThresholdVolume, currentTime / duration);
            audioMixer.SetFloat(exposedParam, newThresholdVol);
            yield return null;
        }
        audioMixer.SetFloat(exposedParam, targetThresholdVolume);
        yield break;
    }

    // this one is better
    // it uses maths to lerp the real volume of the mixer, accepting values between 0 and 1
    // (it still lerps the threshold volume)
    public static IEnumerator FadeMixerGroup01Volume(AudioMixer audioMixer, string exposedParam, float targetVolume, float duration)
    {
        float currentTime = 0;
        audioMixer.GetFloat(exposedParam, out float currentVol);
        currentVol = Mathf.Pow(10, currentVol / 20);
        float targetValue = Mathf.Clamp(targetVolume, 0.0001f, 1);
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            float newVol = Mathf.Lerp(currentVol, targetValue, currentTime / duration);
            audioMixer.SetFloat(exposedParam, Mathf.Log10(newVol) * 20f);
            yield return null;
        }

        float endVolume = Mathf.Lerp(currentVol, targetValue, 1);
        audioMixer.SetFloat(exposedParam, Mathf.Log10(endVolume) * 20f);
        yield break;
    }
}
