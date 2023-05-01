using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossFadeLooper : MonoBehaviour
{
    [System.Serializable]
    public class AudioSourceWithLoopPoints
    {
        public AudioSource audioSource;
        [Space]
        public bool bakedFades = false;
        public float fadeInDuration = 0;
        public float fadeOutDuration = 0;

        public AudioSourceWithLoopPoints(AudioSource _audioSource, bool _bakedFades = false, float _fadeInDuration = 0, float _fadeOutDuration = 0)
        {
            audioSource = _audioSource;
            bakedFades = _bakedFades;
            fadeInDuration = _fadeInDuration;
            fadeOutDuration = _fadeOutDuration;
        }
    }

    public bool BUTTONGetAudioSources = false;
    public float playOffset = 0;
    public List<AudioSourceWithLoopPoints> audioSourcesInLoop = new List<AudioSourceWithLoopPoints>();
    [Space]
    [SerializeField] float volume = 1; // to change the system's volume during run-time use an audio mixer group
    [SerializeField] bool mute = false;

    int activeSourceIndex = 0;

    public bool Mute
    {
        get
        {
            return mute;
        }
        set
        {
            foreach(AudioSourceWithLoopPoints audioSourceWithLoop in audioSourcesInLoop)
            {
                audioSourceWithLoop.audioSource.mute = value;
            }
            mute = value;
        }
    }

    #region Tool(s)
    void OnValidate()
    {
        GetAudioButton();
    }

    void GetAudioButton()
    {
        if(BUTTONGetAudioSources == false) { return; }
        BUTTONGetAudioSources = true;

        List<AudioSourceWithLoopPoints> newAudioSourceList = new List<AudioSourceWithLoopPoints>();

        AudioSource[] audioSources = gameObject.GetComponents<AudioSource>();
        foreach(AudioSource audioSource in audioSources)
        {
            audioSource.playOnAwake = false;
            audioSource.volume = 0;
            audioSource.mute = false;
            AudioSourceWithLoopPoints newListEntry = new AudioSourceWithLoopPoints(audioSource);
            newAudioSourceList.Add(newListEntry);
        }
        audioSourcesInLoop = newAudioSourceList;
    }
    #endregion

    void Start()
    {
        Mute = mute;
        SoundUpdate();
    }

    void SoundUpdate()
    {
        AudioSourceWithLoopPoints current = audioSourcesInLoop[activeSourceIndex];

        int nextIndex = activeSourceIndex + 1;
        if(nextIndex >= audioSourcesInLoop.Count) 
        { 
            nextIndex = 0; 
        }
        AudioSourceWithLoopPoints next = audioSourcesInLoop[nextIndex];

        float currentFadeOutStart = current.audioSource.clip.length - current.fadeOutDuration - current.audioSource.time;
        StartCoroutine(QueAudioFadeOut(current));
        StartCoroutine(QueAudioFadeIn(current, next));
        StartCoroutine(QueSoundUpdate(currentFadeOutStart));
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
        yield break;
    }

    IEnumerator QueAudioFadeIn(AudioSourceWithLoopPoints currentlyPlayingAudioSource, AudioSourceWithLoopPoints newAudioSource)
    {
        float currentFadeOutStart = currentlyPlayingAudioSource.audioSource.clip.length - currentlyPlayingAudioSource.fadeOutDuration - currentlyPlayingAudioSource.audioSource.time;
        yield return new WaitForSeconds(currentFadeOutStart);
        StartCoroutine(QueAudioPlay(newAudioSource.audioSource, playOffset));
        StartCoroutine(StartFade(newAudioSource.audioSource, newAudioSource.fadeInDuration, volume));
        yield break;
    }

    IEnumerator QueAudioFadeOut(AudioSourceWithLoopPoints currentlyPlayingAudioSource)
    {
        float currentFadeOutStart = currentlyPlayingAudioSource.audioSource.clip.length - currentlyPlayingAudioSource.fadeOutDuration - currentlyPlayingAudioSource.audioSource.time;
        yield return new WaitForSeconds(currentFadeOutStart);
        StartCoroutine(StartFade(currentlyPlayingAudioSource.audioSource, currentlyPlayingAudioSource.fadeOutDuration, 0));
        StartCoroutine(QueStopAudio(currentlyPlayingAudioSource.audioSource, currentlyPlayingAudioSource.fadeOutDuration));
        yield break;
    }

    IEnumerator QueSoundUpdate(float delay)
    {
        yield return new WaitForSeconds(delay);

        activeSourceIndex++;
        if (activeSourceIndex >= audioSourcesInLoop.Count)
        {
            activeSourceIndex = 0;
        }
        SoundUpdate();
        yield break;
    }

    IEnumerator QueStopAudio(AudioSource audioSource, float delay)
    {
        yield return new WaitForSeconds(delay);
        audioSource.Stop();
        yield break;
    }

    IEnumerator QueAudioPlay(AudioSource audioSource, float delay)
    {
        yield return new WaitForSeconds(delay);
        audioSource.Play();
        yield break;
    }
}
