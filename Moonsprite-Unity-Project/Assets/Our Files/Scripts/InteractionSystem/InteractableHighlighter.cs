using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class InteractableHighlighter : MonoBehaviour
{
    // Attribution: Maciej W

    [Header("Required References")]
    public ParticleSystem psHighlighter;

    // Interactable highlight stuff
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerInteractionHighlighter"))
        {
            EnableDisableParticleHighlight(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("PlayerInteractionHighlighter"))
        {
            EnableDisableParticleHighlight(false);
        }
    }

    void EnableDisableParticleHighlight(bool enableDisable)
    {
        if (psHighlighter == null)
        {
            Debug.LogWarning("No highlighter particleSystem on item");
            return;
        }

        if (enableDisable == true)
        {
            psHighlighter.Play();
        }
        else
        {
            psHighlighter.Stop();
        }
    }
}
