using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudioOnClick : MonoBehaviour
{
    public AudioSource ButtonAudio;

    public void PlayButtonAudio()
    {
        ButtonAudio.Play();
    }
}
