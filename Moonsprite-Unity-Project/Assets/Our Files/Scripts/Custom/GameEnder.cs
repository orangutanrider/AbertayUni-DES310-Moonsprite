using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEnder : MonoBehaviour, IInteractable, ITimelineEvent, IConfirmationStep
{
    [Header("Required References")]
    public GameObject gameEndScreenPrefab;
    public ParticleSystem[] particleSystems;
    public UnityEngine.Rendering.Universal.Light2D[] lights;

    [Header("Parameters")]
    [SerializeField] int interactionPriority = 0;
    public float lerpDuration = 2;
    public float particleTriggerTime = 1;
    [Space]
    public DialogueObject dialogueOnInteract;
    public GameObject[] setInactiveOnGameEnd;

    List<float> lightListMaxIntensitiesParralelList = new List<float>();

    float lerpTimer = -1;
    bool particlesActivated = false;
    bool dialogueTriggered = false;

    PlayerInteractionController cachedPlayerInteractionController = null;

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
        cachedPlayerInteractionController = playerInteractionController;
        if (particlesActivated == false) 
        {
            playerInteractionController.ExitInteraction();
            return; 
        }

        if (dialogueTriggered == true)
        {
            ConfirmationUIManger.Instance.AskForConfirmation(this);
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

    void IConfirmationStep.Confirm()
    {
        // end the game
        Time.timeScale = 1;
        StartCoroutine(ScreenBlurManager.Instance.LerpBlur(2f, 0.0066f));
        foreach(GameObject gameObjectInArray in setInactiveOnGameEnd)
        {
            gameObjectInArray.SetActive(false);
        }
        Instantiate(gameEndScreenPrefab, transform.position, transform.rotation);
    }

    void IConfirmationStep.Return()
    {
        cachedPlayerInteractionController.ExitInteraction();
    }
}
