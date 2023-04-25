using System;
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

    public AudioManager GetManagerName(string name)
    {
        AudioManager s = Array.Find(audioManagers, item => item.managerName == name);

        if (s == null)
        {
            Debug.LogWarning("manager " + name + " not found!");
            return null;
        }

        return s;
    }

    public string GetManagerNameByFile(string fileName)
    {

        foreach (AudioManager x in audioManagers)
        {
            Debug.LogWarning("current Sound: " + name);
            if (x.CheckForFile(fileName))
            {
                return x.managerName;
            }

        }

        return null;

    }

    public void PlaySpecificSound(string managerName, string fileName)
    {
        //foreach (AudioManager x in audioManagers)
        //{
        //    Debug.LogWarning("current Sound: " + name);
        //    if (x.managerName == managerName && x.CheckForFile(fileName))
        //        x.Play(fileName);

        //}

        AudioManager s = Array.Find(audioManagers, item => item.managerName == managerName);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }

        if (s.managerName == managerName && s.CheckForFile(fileName))
            s.Play(fileName);
    }

    public void StopSpecificManager(string managerName)
    {

        AudioManager s = Array.Find(audioManagers, item => item.managerName == managerName);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }

        
            s.StopPlaying();

    }

    public void StopAllManagers()
    {
        foreach (AudioManager x in audioManagers)
        {
            x.StopPlaying();
        }
       
    }
}
