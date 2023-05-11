using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFootstepPlayer : MonoBehaviour
{
    // so it's not actually a footstep rate, it's more like a footstep range.
    // when it plays a footstep sound this script store's the player's current position at that moment
    // and then if they get too far away from that stored position, it goes to play another footstep sound (re-setting the process)

    public bool printData = false;

    [Header("Required References")]
    public AudioSource audioSource;

    [Header("Parameters")]
    public bool useAnimEventsForFootstepRate = false;
    public float footstepRate = 0;
    [Space]
    public LayerMask surfaceLayerMask;
    public FootstepSurface fallbackFootstep;
    public FootstepSurface[] footstepSurfaceSounds;

    [Header("For Viewing Purposes")]
    [SerializeField] List<string> activeSurfaces = new List<string>();

    Vector3 previousFootstepPosition = Vector3.zero;

    const string errorTag = "ERROR";
    const string emptyTag = "NONE";

    // Start is called before the first frame update
    void Start()
    {
        previousFootstepPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (useAnimEventsForFootstepRate == true) { return; }

        // Error handling
        if (footstepSurfaceSounds == null)
        {
            if(printData == false) { return; }
            Debug.LogError("footstepSurfaceSounds list was null");
            return;
        }
        if(footstepSurfaceSounds.Length == 0)
        {
            if (printData == false) { return; }
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

    public void PlayFootstepSoundAnimTrigger()
    {
        if(useAnimEventsForFootstepRate == false) { return; }

        string surfaceTag = GetActiveSurfaceTagIndexZero();
        FootstepSurface footstepSurface = GetFootstepSurfaceBySurfaceTag(surfaceTag);
        PlayFootstepSound(footstepSurface);
    }

    string GetActiveSurfaceTagIndexZero()
    {
        if(activeSurfaces == null)
        {
            if (printData == false) { return errorTag; }
            Debug.LogError("The activeSurfaces list was null");
            return errorTag;
        }

        if (activeSurfaces.Count == 0)
        {
            if (printData == false) { return emptyTag; }
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

            if(printData == false) { return; }
            Debug.Log("Added: " + surfaceTrigger.tag);
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

                if (printData == false) { return; }
                Debug.Log("Removed: " + surfaceTrigger.tag);
            }
        }
    }
}
