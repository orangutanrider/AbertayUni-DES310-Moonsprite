using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEnder : MonoBehaviour, IInteractable, ITimelineEvent
{
    [Header("Required References")]
    public ParticleSystem[] particleSystems;
    public UnityEngine.Rendering.Universal.Light2D[] lights;

    [Header("Parameters")]
    [SerializeField] int interactionPriority = 0;
    public float lerpDuration = 2;
    public float particleTriggerTime = 1;
    [Space]
    public DialogueObject dialogueOnInteract;

    List<float> lightListMaxIntensitiesParralelList = new List<float>();

    float lerpTimer = -1;
    bool particlesActivated = false;
    bool dialogueTriggered = false;

    int IInteractable.InteractionPriority
    {
        get
        {
            return interactionPriority;
        }
        set
        {
            interactionPriority = value;
        }
    }

    void Start()
    {
        foreach(UnityEngine.Rendering.Universal.Light2D light2D in lights)
        {
            lightListMaxIntensitiesParralelList.Add(light2D.intensity);
            light2D.intensity = 0;
        }
    }

    void Update()
    {
        if (lerpTimer < 0) { return; }

        lerpTimer = lerpTimer + Time.deltaTime;
        for (int loop = 0; loop <= lights.Length - 1; loop++)
        {
            float lerpValue = Mathf.Lerp(0, lightListMaxIntensitiesParralelList[loop], lerpTimer / lerpDuration);
            lights[loop].intensity = lerpValue;
        }
        
        if(particlesActivated == true) { return; }
        if(lerpTimer >= particleTriggerTime)
        {
            particlesActivated = true;
            foreach(ParticleSystem ps in particleSystems)
            {
                ps.Play();
            }
        }
    }

    void IInteractable.InteractionEvent(PlayerInteractionController playerInteractionController, TagList activeItemTagList)
    {
        if(particlesActivated == false || dialogueTriggered == true) 
        {
            playerInteractionController.ExitInteraction();
            return; 
        }

        dialogueTriggered = true;
        DialogueManager.instance.StartNewDialogue(dialogueOnInteract, gameObject);
        playerInteractionController.ExitInteraction();
    }

    void ITimelineEvent.TimelineEvent(int eventIndex)
    {
        lerpTimer = 0;
    }
}
