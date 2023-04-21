using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherEffectsManager : MonoBehaviour
{
    public ParticleSystem[] weatherParticleSystems;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag != "Player")
        {
            return;
        }

        foreach (ParticleSystem particleSystem in weatherParticleSystems)
        {
            particleSystem.Stop();
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Player")
        {
            return;
        }

        foreach(ParticleSystem particleSystem in weatherParticleSystems)
        {
            particleSystem.Play();
        }
    }
}
