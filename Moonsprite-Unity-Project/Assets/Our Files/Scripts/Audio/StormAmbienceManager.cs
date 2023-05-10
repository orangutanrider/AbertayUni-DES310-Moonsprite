using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class StormAmbienceManager : MonoBehaviour
{
    public static StormAmbienceManager Instance
    {
        get
        {
            return instance;
        }
        private set
        {
            instance = value;
        }
    }
    private static StormAmbienceManager instance = null;

    public enum AmbienceState
    {
        Indoors,
        Outdoors,
        TransitioningToOutdoors,
        TransitioningToIndoors 
    }

    [System.Serializable]
    public class StormAmbienceChannel
    {
        public AudioMixer mixer = null;
        public string volumeParameterName = "null";
        [Space]
        public float startPlayingAt = 5;
        public float stopPlayingAt = 10;
        public float fadeInTime = 1;
        public float fadeOutTime = 1;
        [Space]
        [HideInInspector] public bool active = false;
        [HideInInspector] public bool stopped = false;

        public StormAmbienceChannel(AudioMixer _mixer, string _volumeParameter, bool _active, bool _stopped = false)
        {
            mixer = _mixer;
            volumeParameterName = _volumeParameter;
            active = _active;
            stopped = _stopped;
        }
    }

    [Header("Parameters")]
    public AmbienceState ambienceState = AmbienceState.Indoors;
    public float stateTransitionFadeDuration = 2f;
    [Space]
    public List<StormAmbienceChannel> indoorsAmbientChannels = new List<StormAmbienceChannel>();
    public List<StormAmbienceChannel> outdoorsAmbientChannels = new List<StormAmbienceChannel>();

    float timer = 0;

    void Start()
    {
        if(instance != null)
        {
            Debug.LogWarning("This will break if there's more than one in a scene at a time");
        }
        instance = this;

        foreach (StormAmbienceChannel stormChannel in outdoorsAmbientChannels)
        {
            stormChannel.mixer.SetFloat(stormChannel.volumeParameterName, 0);
        }
        foreach (StormAmbienceChannel stormChannel in indoorsAmbientChannels)
        {
            stormChannel.mixer.SetFloat(stormChannel.volumeParameterName, 0);
        }
    }

    void Update()
    {
        timer = timer + Time.deltaTime;

        foreach (StormAmbienceChannel stormChannel in outdoorsAmbientChannels)
        {
            UpdateOutdoorsChannel(stormChannel, timer);
        }
        foreach (StormAmbienceChannel stormChannel in indoorsAmbientChannels)
        {
            UpdateIndoorsChannel(stormChannel, timer);
        }
    }

    public void TransitionToOutdoorsTrigger()
    {
        StopAllCoroutines();
        ambienceState = AmbienceState.TransitioningToOutdoors;
        StartCoroutine(StateTransition(AmbienceState.Outdoors, stateTransitionFadeDuration));

        // fade in active outdoor ambient channels
        FadeActiveChannels(outdoorsAmbientChannels, 1);

        // fade out active indoor ambient channels
        FadeActiveChannels(indoorsAmbientChannels, 0);
    }

    public void TransitionToIndoorsTrigger()
    {
        StopAllCoroutines();
        ambienceState = AmbienceState.TransitioningToIndoors;
        StartCoroutine(StateTransition(AmbienceState.Indoors, stateTransitionFadeDuration));

        // fade out active outdoor ambient channels
        FadeActiveChannels(outdoorsAmbientChannels, 0);

        // fade in active indoor ambient channels
        FadeActiveChannels(indoorsAmbientChannels, 1);
    }

    IEnumerator StateTransition(AmbienceState state, float duration)
    {
        yield return new WaitForSeconds(duration);
        ambienceState = state;
        yield break;
    }

    void UpdateOutdoorsChannel(StormAmbienceChannel channel, float time)
    {
        if(channel.stopped == true) { return; }
        if(time >= channel.stopPlayingAt)
        {
            // fade out and stop channel
            channel.stopped = true;
            channel.active = false;
            StartCoroutine(FadeMixerGroup.FadeMixerGroup01Volume(channel.mixer, channel.volumeParameterName, 0, channel.fadeOutTime));
            return;
        }

        if(time <= channel.startPlayingAt || channel.active == true) { return; }

        channel.active = true;

        if (ambienceState != AmbienceState.Outdoors && ambienceState != AmbienceState.TransitioningToOutdoors)
        {
            return;
        }

        // fade in channel
        StartCoroutine(FadeMixerGroup.FadeMixerGroup01Volume(channel.mixer, channel.volumeParameterName, 1, channel.fadeInTime));
    }

    void UpdateIndoorsChannel(StormAmbienceChannel channel, float time)
    {
        if (channel.stopped == true) { return; }
        if (time >= channel.stopPlayingAt)
        {
            // fade out and stop channel
            channel.stopped = true;
            channel.active = false;
            StartCoroutine(FadeMixerGroup.FadeMixerGroup01Volume(channel.mixer, channel.volumeParameterName, 0, channel.fadeOutTime));
            return;
        }

        if (time <= channel.startPlayingAt || channel.active == true) { return; }

        channel.active = true;

        if (ambienceState != AmbienceState.Indoors && ambienceState != AmbienceState.TransitioningToIndoors)
        {
            return;
        }

        // fade in channel
        StartCoroutine(FadeMixerGroup.FadeMixerGroup01Volume(channel.mixer, channel.volumeParameterName, 1, channel.fadeInTime));
    }

    void FadeActiveChannels(List<StormAmbienceChannel> channelList, float targetVolume)
    {
        foreach (StormAmbienceChannel stormChannel in channelList)
        {
            if (stormChannel.active == false || stormChannel.stopped == true) { continue; }

            StartCoroutine(FadeMixerGroup.FadeMixerGroup01Volume(stormChannel.mixer, stormChannel.volumeParameterName, targetVolume, stateTransitionFadeDuration));
        }
    }
}
