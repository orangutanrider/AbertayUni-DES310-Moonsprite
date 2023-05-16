using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class StormAmbienceManager : MonoBehaviour
{ 
    // I think this is a very overly complicated way of doing this
    // but at the same time I've impressed myself with this, I would've never have been able to make a script like this before this project
    // even if it is a bad way of doing this

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
        public string volumeParameterName = "null";
        [Space]
        public float startPlayingAt = 5;
        public float stopPlayingAt = 10;
        public float fadeInTime = 1;
        public float fadeOutTime = 1;

        [HideInInspector] public float proxy01Volume = 0;
        [HideInInspector] public float proxyTime = 0;
        [HideInInspector] public float proxy01Target = 0;

        [HideInInspector] public bool active = false;
        [HideInInspector] public bool stopped = false;

        
        public StormAmbienceChannel(string _volumeParameterName, float _startPlayingAt, float _stopPlayingAt, float _fadeInTime, float _fadeOutTime, float _proxy01Volume = 0, float _proxyTime = 0, float _proxy01Target = 0, bool _active = false, bool _stopped = false)
        {
            volumeParameterName = _volumeParameterName;

            startPlayingAt = _startPlayingAt;
            stopPlayingAt = _stopPlayingAt;
            fadeInTime = _fadeInTime;
            fadeOutTime = _fadeOutTime;

            proxy01Volume = _proxy01Volume;
            proxyTime = _proxyTime;
            proxy01Target = _proxy01Target;

            active = _active;
            stopped = _stopped;
        }
    }

    [SerializeField] float timer = 0;

    [Header("Required References")]
    public AudioMixer mixer = null;
    public AmbienceManagerPartnerScript partnerScript = null;

    [Header("Parameters")]
    public AmbienceState ambienceState = AmbienceState.Indoors;
    public float stateTransitionFadeDuration = 2f;
    [Space]
    public List<StormAmbienceChannel> indoorsAmbientChannels = new List<StormAmbienceChannel>();
    public List<StormAmbienceChannel> outdoorsAmbientChannels = new List<StormAmbienceChannel>();

    AmbienceState[] indoorAmbientStates = { AmbienceState.Indoors, AmbienceState.TransitioningToIndoors };
    AmbienceState[] outdoorAmbientStates = { AmbienceState.Outdoors, AmbienceState.TransitioningToOutdoors };

    void Start()
    {
        if(instance != null)
        {
            Debug.LogWarning("This script will break if there's more than one in a scene at a time");
        }
        instance = this;
    }

    void Update()
    {
        timer = timer + Time.deltaTime;

        foreach (StormAmbienceChannel stormChannel in outdoorsAmbientChannels)
        {
            UpdateChannel(stormChannel, timer, outdoorAmbientStates);
        }
        foreach (StormAmbienceChannel stormChannel in indoorsAmbientChannels)
        {
            UpdateChannel(stormChannel, timer, indoorAmbientStates);
        }
    }

    public void TransitionToOutdoorsTrigger()
    {
        StopAllCoroutinesOtherThanProxyLerp();
        ambienceState = AmbienceState.TransitioningToOutdoors;
        StartCoroutine(StateTransition(AmbienceState.Outdoors, stateTransitionFadeDuration));

        // fade out active outdoor ambient channels
        foreach (StormAmbienceChannel stormChannel in outdoorsAmbientChannels)
        {
            FadeActiveChannel(stormChannel, 1, outdoorAmbientStates);
        }
        
        // fade in active indoor ambient channels
        foreach (StormAmbienceChannel stormChannel in indoorsAmbientChannels)
        {
            FadeActiveChannel(stormChannel, 0, indoorAmbientStates);
        }
    }

    public void TransitionToIndoorsTrigger()
    {
        StopAllCoroutinesOtherThanProxyLerp();
        ambienceState = AmbienceState.TransitioningToIndoors;
        StartCoroutine(StateTransition(AmbienceState.Indoors, stateTransitionFadeDuration));

        // fade out active outdoor ambient channels
        foreach(StormAmbienceChannel stormChannel in outdoorsAmbientChannels)
        {
            FadeActiveChannel(stormChannel, 0, outdoorAmbientStates);
        }

        // fade in active indoor ambient channels
        foreach (StormAmbienceChannel stormChannel in indoorsAmbientChannels)
        {
            FadeActiveChannel(stormChannel, 1, indoorAmbientStates);
        }
    }

    IEnumerator StateTransition(AmbienceState state, float duration)
    {
        yield return new WaitForSeconds(duration);
        ambienceState = state;
        yield break;
    }

    IEnumerator ProxyLerp(float duration, float target01Volume, StormAmbienceChannel ambienceChannel)
    {
        ambienceChannel.proxy01Target = target01Volume;

        float currentTime = 0;
        float current01Volume = ambienceChannel.proxy01Volume;

        float target01Value = Mathf.Clamp(target01Volume, 0.0001f, 1);

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;

            float new01Volume = Mathf.Lerp(current01Volume, target01Value, currentTime / duration);
            ambienceChannel.proxy01Volume = new01Volume;

            ambienceChannel.proxyTime = currentTime;

            yield return null;
        }

        ambienceChannel.proxyTime = duration;
        ambienceChannel.proxy01Volume = target01Value;

        yield break;
    }

    void StopAllCoroutinesOtherThanProxyLerp()
    {
        // the proxy lerp coroutines aren't done on this script, but this script contains their defintions
        // they're done in the partner script
        StopAllCoroutines();
    }

    void UpdateChannel(StormAmbienceChannel channel, float time, AmbienceState[] states)
    {
        if(channel.stopped == true) { return; }
        if(time >= channel.stopPlayingAt)
        {
            // fade out and stop channel
            channel.stopped = true;

            IEnumerator fadeOutVolumeLerp = FadeMixerGroup.FadeMixerGroup01Volume(mixer, channel.volumeParameterName, 0, channel.fadeOutTime);
            StartCoroutine(fadeOutVolumeLerp);

            IEnumerator fadeOutProxyLerp = ProxyLerp(channel.fadeOutTime, 0, channel);
            partnerScript.StartProxyLerpCoroutine(fadeOutProxyLerp);

            return;
        }

        if(time <= channel.startPlayingAt || channel.active == true) { return; }

        // activate channel and start fading in proxy volume
        channel.active = true;

        IEnumerator fadeInProxyLerp = ProxyLerp(channel.fadeInTime, 1, channel);
        partnerScript.StartProxyLerpCoroutine(fadeInProxyLerp);

        bool allStatesAreNot = true;
        foreach (AmbienceState state in states)
        {
            if (state != ambienceState)
            {
                continue;
            }
            else
            {
                allStatesAreNot = false;
            }
        }
        if(allStatesAreNot == true) { return; }

        // if state is outdoors then fade in the real volume
        IEnumerator fadeInVolumeLerp = FadeMixerGroup.FadeMixerGroup01Volume(mixer, channel.volumeParameterName, 1, channel.fadeInTime);
        StartCoroutine(fadeInVolumeLerp);
    }

    void FadeActiveChannel(StormAmbienceChannel stormChannel, float targetVolume, AmbienceState[] states)
    {
        if(stormChannel.active == false) { return; }
        if (stormChannel.proxy01Volume == stormChannel.proxy01Target || targetVolume <= stormChannel.proxy01Volume) 
        {
            IEnumerator volumeTargetFade = FadeMixerGroup.FadeMixerGroup01Volume(mixer, stormChannel.volumeParameterName, targetVolume, stateTransitionFadeDuration);
            StartCoroutine(volumeTargetFade);
            return; 
        }

        IEnumerator volumeProxyFade = FadeMixerGroup.FadeMixerGroup01Volume(mixer, stormChannel.volumeParameterName, stormChannel.proxy01Volume, stateTransitionFadeDuration);
        StartCoroutine(volumeProxyFade);

        // if the channel being faded didn't finish its previous volume lerp and is presumably being switched on (hence the state check) then that lerp needs to be resumed
        bool allStatesAreNot = true;
        foreach (AmbienceState state in states)
        {
            if (state != ambienceState)
            {
                continue;
            }
            else
            {
                allStatesAreNot = false;
            }
        }
        if (allStatesAreNot == true) { return; }

        // resume the lerp
        IEnumerator resumePreviousFade = null;
        if(stormChannel.proxy01Target == 0)
        {
            Debug.Log(stormChannel.fadeOutTime - stormChannel.proxyTime);
            resumePreviousFade = FadeMixerGroup.FadeMixerGroup01Volume(mixer, stormChannel.volumeParameterName, stormChannel.proxy01Target, stormChannel.fadeOutTime - stormChannel.proxyTime);
        }
        else
        {
            Debug.Log(stormChannel.fadeOutTime - stormChannel.proxyTime);
            resumePreviousFade = FadeMixerGroup.FadeMixerGroup01Volume(mixer, stormChannel.volumeParameterName, stormChannel.proxy01Target, stormChannel.fadeInTime - stormChannel.proxyTime);
        }
        IEnumerator delayedResume = DelayedCoroutine(resumePreviousFade, stateTransitionFadeDuration);

        StartCoroutine(delayedResume);
    }

    IEnumerator DelayedCoroutine(IEnumerator enumerator, float delay)
    {
        yield return new WaitForSeconds(delay);
        StartCoroutine(enumerator);
    }
}
