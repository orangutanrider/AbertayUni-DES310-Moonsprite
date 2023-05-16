using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeInOnStart : MonoBehaviour
{
    public AudioSource source;
    public float fadeEndVolume = 1;
    public float fadeDuration = 1;

    // Start is called before the first frame update
    void Start()
    {
        source.volume = 0;
        StartCoroutine(StartFade(source, fadeDuration, fadeEndVolume));
    }

    IEnumerator StartFade(AudioSource audioSource, float duration, float targetVolume)
    {
        float currentTime = 0;
        float start = audioSource.volume;
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
            yield return null;
        }

        audioSource.volume = targetVolume;
        yield break;
    }
}
