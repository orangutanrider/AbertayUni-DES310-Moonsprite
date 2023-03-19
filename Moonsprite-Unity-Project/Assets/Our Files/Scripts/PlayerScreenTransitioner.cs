using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScreenTransitioner : MonoBehaviour
{
    [Header("Required Components")]
    public ImageFader doorTransitionImageFader;

    [Header("Parameters")]
    public float doorTransitionTime;

    [HideInInspector] public static PlayerScreenTransitioner instance = null;

    void Awake()
    {
        instance = this;
    }

    public IEnumerator DoorTransition()
    {
        doorTransitionImageFader.FadeIn(doorTransitionTime * (1 / 3));
        yield return new WaitForSeconds(doorTransitionTime * (2 / 3));
        doorTransitionImageFader.FadeOut(doorTransitionTime * (1 / 3));
        yield break;
    }
}
