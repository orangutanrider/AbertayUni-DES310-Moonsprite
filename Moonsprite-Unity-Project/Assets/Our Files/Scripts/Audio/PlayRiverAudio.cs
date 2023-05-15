using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayRiverAudio : MonoBehaviour
{
    public AudioSource RiverBackgroundaudio;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            RiverBackgroundaudio.Play();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            RiverBackgroundaudio.Stop();
        }
    }
}