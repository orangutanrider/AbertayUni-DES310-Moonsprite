using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManagerManager : MonoBehaviour
{
    public AudioManager[] audioManagers;

    public static AudioManagerManager AMinstance;

    void Awake()
    {
        if (AMinstance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            AMinstance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

        public void PlaySound(string name)
    {
        foreach (AudioManager x in audioManagers)
        {
            Debug.LogWarning("current Sound: " + name);
            x.Play(name);
           
        }
    }
}
