using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

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

    [Header("Tools")]
    public bool BUTTONGetAudioSources = false;
    public bool BUTTONMultiEdit = false;
    [Space]
    public bool multiEditBakedFades = false;
    public float multiEditFadeInDuration = 1;
    public float multiEditFadeOutDuration = 1;
    [Space]
    public AudioClip multiEditAudioClip = null;
    public AudioMixerGroup multiEditAudioMixerGroup = null;
    public bool multiEditBypassEffects = false;
    public bool multiEditBypassListenerEffects = false;
    public bool multiEditBypassReverbZones = false;
    [Range(0, 256)] public int multiEditPriority = 128;
    [Range(-3f, 3f)] public float multiEditPitch = 1;
    [Range(-1f, 1f)] public float multiEditStereoPan = 0;
    [Range(0f, 1f)] public float multiEditSpatialBlend = 0;
    [Range(0f, 1.1f)] public float multiEditReverbZoneMix = 1;

    [Header("Parameters")]
    // to change the system's volume during run-time it's best to use an audio mixer group
    // this system only updates the volume on start and when a new loop is played
    [Range(0f, 1f)] [SerializeField] float volume = 1; 
    [SerializeField] bool mute = false;
    public List<AudioSourceWithLoopPoints> audioSourcesInLoop = new List<AudioSourceWithLoopPoints>();

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

    #region Tools
    void OnValidate()
    {
        GetAudioButton();
        MultiEditAudioButton();
    }

    void MultiEditAudioButton()
    {
        if (BUTTONMultiEdit == false) { return; }
        BUTTONMultiEdit = false;

        foreach (AudioSourceWithLoopPoints audioSourceWithLoop in audioSourcesInLoop)
        {
            if (audioSourceWithLoop.audioSource == null) { continue; }

            audioSourceWithLoop.bakedFades = multiEditBakedFades;
            if(audioSourceWithLoop.bakedFades == true)
            {
                audioSourceWithLoop.audioSource.volume = volume;
            }

            audioSourceWithLoop.fadeInDuration = multiEditFadeInDuration;
            audioSourceWithLoop.fadeOutDuration = multiEditFadeOutDuration;

            audioSourceWithLoop.audioSource.clip = multiEditAudioClip;
            audioSourceWithLoop.audioSource.outputAudioMixerGroup = multiEditAudioMixerGroup;
            audioSourceWithLoop.audioSource.bypassEffects = multiEditBypassEffects;
            audioSourceWithLoop.audioSource.bypassListenerEffects = multiEditBypassListenerEffects;
            audioSourceWithLoop.audioSource.bypassReverbZones = multiEditBypassReverbZones;
            audioSourceWithLoop.audioSource.priority = multiEditPriority;
            audioSourceWithLoop.audioSource.pitch = multiEditPitch;
            audioSourceWithLoop.audioSource.panStereo = multiEditStereoPan;
            audioSourceWithLoop.audioSource.spatialBlend = multiEditSpatialBlend;
            audioSourceWithLoop.audioSource.reverbZoneMix = multiEditReverbZoneMix;
        }
    }

    void GetAudioButton()
    {
        if(BUTTONGetAudioSources == false) { return; }
        BUTTONGetAudioSources = false;

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

        BUTTONMultiEdit = true;
        MultiEditAudioButton();
    }
    #endregion

    void Start()
    {
        Mute = mute;

        AudioSourceWithLoopPoints current = audioSourcesInLoop[activeSourceIndex];

        if (current.bakedFades == false)
        {
            StartCoroutine(StartFade(current.audioSource, current.fadeInDuration, volume));
        }
        else
        {
            current.audioSource.volume = volume;
        }
        StartCoroutine(QuePlayAudio(current.audioSource, 0f));

        AudioUpdate();
    }

    void AudioUpdate()
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
        StartCoroutine(QueNextAudioUpdate(currentFadeOutStart));
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
        if (targetVolume <= 0)
        {
            audioSource.Stop();
        }
        yield break;
    }

    IEnumerator QueAudioFadeIn(AudioSourceWithLoopPoints currentlyPlayingAudioSource, AudioSourceWithLoopPoints newAudioSource)
    {
        float currentFadeOutStart = currentlyPlayingAudioSource.audioSource.clip.length - currentlyPlayingAudioSource.fadeOutDuration - currentlyPlayingAudioSource.audioSource.time;
        yield return new WaitForSeconds(currentFadeOutStart);
        StartCoroutine(QuePlayAudio(newAudioSource.audioSource, 0f));
        if (currentlyPlayingAudioSource.bakedFades == false)
        {
            StartCoroutine(StartFade(newAudioSource.audioSource, newAudioSource.fadeInDuration, volume));
        }
        else
        {
            newAudioSource.audioSource.volume = volume;
        }
        yield break;
    }

    IEnumerator QueAudioFadeOut(AudioSourceWithLoopPoints currentlyPlayingAudioSource)
    {
        float currentFadeOutStart = currentlyPlayingAudioSource.audioSource.clip.length - currentlyPlayingAudioSource.fadeOutDuration - currentlyPlayingAudioSource.audioSource.time;
        yield return new WaitForSeconds(currentFadeOutStart);
        if (currentlyPlayingAudioSource.bakedFades == false)
        {
            StartCoroutine(StartFade(currentlyPlayingAudioSource.audioSource, currentlyPlayingAudioSource.fadeOutDuration, 0));
        }
        yield break;
    }

    IEnumerator QueNextAudioUpdate(float delay)
    {
        yield return new WaitForSeconds(delay);

        activeSourceIndex++;
        if (activeSourceIndex >= audioSourcesInLoop.Count)
        {
            activeSourceIndex = 0;
        }

        AudioUpdate();

        yield break;
    }

    IEnumerator QuePlayAudio(AudioSource audioSource, float delay)
    {
        yield return new WaitForSeconds(delay);
        audioSource.Play();
        yield break;
    }
}
