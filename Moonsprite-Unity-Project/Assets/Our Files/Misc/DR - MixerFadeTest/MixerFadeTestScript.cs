using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MixerFadeTestScript : MonoBehaviour
{
    public AudioMixer targetAudioMixer;
    public float fadeDuration = 2;
    [Space]
    [TextArea(10, 100)]
    public string ReadMeText = "for 01 fade: space to fade it out, enter to fade it in" + System.Environment.NewLine 
        +"for raw threshold fade: F to fade it out, G to fade it in";

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) == true)
        {
            StartCoroutine(FadeMixerGroup.FadeMixerGroup01Volume(targetAudioMixer, AudMixerParamReferences.testFadeVolume, 0, fadeDuration));
            Debug.Log("FadingOut");
        }
        if (Input.GetKeyDown(KeyCode.Return) == true)
        {
            StartCoroutine(FadeMixerGroup.FadeMixerGroup01Volume(targetAudioMixer, AudMixerParamReferences.testFadeVolume, 1, fadeDuration));
            Debug.Log("FadingIn");
        }

        if(Input.GetKeyDown(KeyCode.F) == true)
        {
            StartCoroutine(FadeMixerGroup.FadeMixerGroupRawThresholdVolume(targetAudioMixer, AudMixerParamReferences.testFadeVolume, -80f, fadeDuration));
            Debug.Log("FadingIn");
        }
        if(Input.GetKeyDown(KeyCode.G) == true)
        {
            StartCoroutine(FadeMixerGroup.FadeMixerGroupRawThresholdVolume(targetAudioMixer, AudMixerParamReferences.testFadeVolume, 0, fadeDuration));
        }
    }
}
