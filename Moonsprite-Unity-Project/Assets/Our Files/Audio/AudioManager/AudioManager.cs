using UnityEngine.Audio;
using System;
using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;

public class AudioManager : MonoBehaviour
{

    public static AudioManager instance;

    public AudioMixerGroup mixerGroup;

    public Sound[] sounds;

    public string managerName;


    void Awake()
    {
        //if (instance != null)
        //{
        //    Destroy(gameObject);
        //}
        //else
        //{
            instance = this;
            DontDestroyOnLoad(gameObject);
        //}

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.loop = s.loop;

            s.source.outputAudioMixerGroup = mixerGroup;
        }
    }

    public void Play(string sound)
    {
        Debug.LogWarning("searching for Sound: " + sound + "...");
        Sound s = Array.Find(sounds, item => item.name == sound);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }

        s.source.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
        s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));

        s.source.Play();
    }

    public void StopPlaying()
    {

        foreach (Sound s in sounds)
        {
            s.source.Stop();
        }
    }

    public bool CheckForFile(string sound)
    {
        Debug.LogWarning("searching for Sound: " + sound + "...");
        Sound s = Array.Find(sounds, item => item.name == sound);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return false;
        }

        return true;
    }

    public void playDialogue()
    {
        string[] test = Regex.Split("3454435433", string.Empty);
        float y = 1;
        foreach (string x in test)
        {
            Debug.LogWarning("current Sound: " + x);
            StartCoroutine(PlayDialogue(x, y, 1, 1));
            // StartCoroutine(DelayLoop());
            y = y + 0.2f;
        }
    }


    IEnumerator PlayDialogue(string sound, float delay, float pitch, float volume)
    {

        yield return new WaitForSeconds(delay);

        //Debug.LogWarning("current Sound: " + sound);

        //string nameString = "Letter" + sound;

        //Sound s = Array.Find(sounds, item => item.name == sound);
        //Debug.LogWarning("Sound: " + sound + " not found!");
        //if (s == null)
        //{
        //    Debug.LogWarning("Sound: " + sound + " not found!");
        //    FindObjectOfType<AudioManager>().Play("123");
        //}
        //else
        //{
        //    Debug.LogAssertion("Sound: " + sound + " found!");
        //    s.source.volume = volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
        //    s.source.pitch = pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));

        //    s.source.Play();

        //    Debug.LogAssertion("Sound: " + sound + " is playing");



        Play(sound);


        //}


        //
        
    }

    IEnumerator DelayLoop()
    {
        yield return new WaitForSeconds(1);
    }

        //for (int i = 0; i < sound.Length; i++)
        //{
        //    //string letter = sound[i];
        //    Sound s = Array.Find(dialogueSounds, item => item.name == sound);
        //    if (s == null)
        //    {
        //        Debug.LogWarning("Sound: " + name + " not found!");

        //    }
        //    else
        //    {
        //        s.source.volume = volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
        //        s.source.pitch = pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));

        //        s.source.Play();


        //    }

        //    yield return new WaitForSeconds(delay);
        //}


        //yield on a new YieldInstruction that waits for 5 seconds.


}


