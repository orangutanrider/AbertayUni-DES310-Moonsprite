using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFootstepPlayer : MonoBehaviour
{
    // so it's not actually a footstep rate, it's more like a footstep range.
    // when it plays a footstep sound this script store's the player's current position at that moment
    // and then if they get too far away from that stored position, it goes to play another footstep sound (re-setting the process)

    [Header("Required References")]
    public AudioSource audioSource;

    [Header("Parameters")]
    public LayerMask surfaceLayerMask;
    public float footstepRate = 0;
    public FootstepSurface fallbackFootstep;
    public FootstepSurface[] footstepSurfaceSounds;

    List<string> activeSurfaces = new List<string>();
    Vector3 previousFootstepPosition = Vector3.zero;

    string errorTag = "ERROR";
    string emptyTag = "NONE";

    // Start is called before the first frame update
    void Start()
    {
        previousFootstepPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Error handling
        if(footstepSurfaceSounds == null)
        {
            Debug.LogError("footstepSurfaceSounds list was null");
            return;
        }
        if(footstepSurfaceSounds.Length == 0)
        {
            Debug.LogWarning("footstepSurfaceSounds list had no entries");
            return;
        }

        // Execution
        float distanceFromPreviousFootstep = Vector3.Distance(transform.position, previousFootstepPosition);
        if (distanceFromPreviousFootstep < footstepRate)
        {
            return;
        }

        previousFootstepPosition = transform.position;
        string surfaceTag = GetActiveSurfaceTagIndexZero();
        FootstepSurface footstepSurface = GetFootstepSurfaceBySurfaceTag(surfaceTag);
        PlayFootstepSound(footstepSurface);
    }

    string GetActiveSurfaceTagIndexZero()
    {
        if(activeSurfaces == null)
        {
            Debug.LogError("The activeSurfaces list was null");
            return errorTag;
        }

        if (activeSurfaces.Count == 0)
        {
            Debug.Log("No active surface");
            return emptyTag;
        }

        return activeSurfaces[0];
    }

    FootstepSurface GetFootstepSurfaceBySurfaceTag(string surfaceTag)
    {
        if(surfaceTag == errorTag || surfaceTag == emptyTag)
        {
            return fallbackFootstep;
        }

        foreach (FootstepSurface footstepSurface in footstepSurfaceSounds)
        {
            if(surfaceTag == footstepSurface.surfaceTag)
            {
                return footstepSurface;
            }
        }
        return fallbackFootstep;
    }

    void PlayFootstepSound(FootstepSurface footstepSurface)
    {
        int randomIndex = Random.Range(0, footstepSurface.footstepSounds.Length - 1);
        audioSource.clip = footstepSurface.footstepSounds[randomIndex];
        audioSource.Play();
    }

    private void OnTriggerEnter2D(Collider2D surfaceTrigger)
    {
        // https://answers.unity.com/questions/50279/check-if-layer-is-in-layermask.html
        if (surfaceLayerMask == (surfaceLayerMask | (1 << surfaceTrigger.gameObject.layer))) 
        {
            activeSurfaces.Add(surfaceTrigger.tag);
        }
    }

    private void OnTriggerExit2D(Collider2D surfaceTrigger)
    {
        // https://answers.unity.com/questions/50279/check-if-layer-is-in-layermask.html
        if (surfaceLayerMask == (surfaceLayerMask | (1 << surfaceTrigger.gameObject.layer)))
        {
            if (activeSurfaces.Contains(surfaceTrigger.tag) == true)
            {
                activeSurfaces.Remove(surfaceTrigger.tag);
            }
        }
    }
}
