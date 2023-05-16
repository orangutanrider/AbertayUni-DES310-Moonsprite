using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    // Attribution: Vasco F + Dominic R

    public float dialogueInputCooldown = 0.5f;

    [Header("Required References")]
    public GameObject playerDialogueEventsContainer;
    [Space]
    public GameObject dialogueCanvas;
    [Space]
    public GameObject playerDialogueUIObject;
    public TMP_Text playerDialogueTextBox;
    public Image playerAvatarImagerDisplay;
    [Space]
    public GameObject npcDialogueUIObject;
    public TMP_Text npcNameTextBox;
    public TMP_Text npcDialogueTextBox;
    public Image npcAvatarImageDisplay;
    [Space]
    public GameObject narrationDialogueUIObject;
    public TMP_Text narrationDialogueTextBox;

    PlayerInteractionController playerInteractor = null;
    bool wasTriggeredViaInteraction = false;
    bool isTalking = false;
    DialogueObject curDialogueObject = null;
    GameObject curDialogueGiver = null;

    float dialogueCooldownTimer = 0;

    [HideInInspector] public static DialogueManager instance = null;

    void Awake()
    {
        if(instance != null)
        {
            Debug.LogWarning("DialogueManager instance wasn't null, is there more than one manager in the scene?");
        }

        instance = this;
    }

    void Update()
    {
        DialogueSystemInputs();
    }

    void Start()
    {
        isTalking = true;
        CloseDialogue();
    }

    #region System
    void DialogueSystemInputs()
    {
        if(isTalking == false)
        {
            return;
        }

        dialogueCooldownTimer = dialogueCooldownTimer + Time.unscaledDeltaTime;
        if(dialogueCooldownTimer < dialogueInputCooldown)
        {
            return;
        }

        // generic next dialogue input
        if (Input.GetKeyDown(KeyCode.G))
        {
            NextDialogueObject();
        }

        // reply input
        // this just checks each alphanumeric key for if they were pressed
        for (int index = 0; index + ((int)KeyCode.Alpha1) < ((int)KeyCode.Alpha9); index++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + index))
            {
                NextDialogueObject(index);
            }
        }
    }

    public void StartNewDialogue(DialogueObject dialogueObject, GameObject dialogueGiver = null, PlayerInteractionController playerInteractionController = null)
    {
        if(dialogueObject == null)
        {
            Debug.Log("dialogueObject given was null value");
            return;
        }

        if(playerInteractionController != null)
        {
            playerInteractor = playerInteractionController;
            wasTriggeredViaInteraction = true;
        }

        curDialogueGiver = dialogueGiver;
        curDialogueObject = dialogueObject;

        Time.timeScale = 0;
        isTalking = true;
        dialogueCanvas.SetActive(true);

        dialogueCooldownTimer = 0;

        ActivateDialogue(dialogueObject);
    }

    void NextDialogueObject(int inputIndex = 0)
    {
        if (curDialogueObject == null || curDialogueObject.nextDialogueObjects == null || curDialogueObject.nextDialogueObjects.Length == 0)
        {
            CloseDialogue();
            return;
        }

        int clampedInputIndex = Mathf.Clamp(inputIndex, 0, curDialogueObject.nextDialogueObjects.Length - 1);

        if (curDialogueObject.nextDialogueObjects.Length - 1 < clampedInputIndex)
        {
            ActivateDialogue(curDialogueObject.nextDialogueObjects[0]);
        }

        ActivateDialogue(curDialogueObject.nextDialogueObjects[clampedInputIndex]);
    }
    #endregion

    #region Buttons
    // I call these buttons cause they're pretty simple
    // they're for the system to press, essentialy just switching things on and off

    // this loads the object given into the system and asks to display it on the ui elements
    void ActivateDialogue(DialogueObject dialogueObject)
    {
        if (dialogueObject == null)
        {
            CloseDialogue();
            return;
        }

        dialogueCooldownTimer = 0;

        curDialogueObject = dialogueObject;

        TriggerDialogueEvents(dialogueObject);
        UpdateDialogueDisplayElements(dialogueObject);
    }

    // this asks to exit the dialogue, it doesn't goto the next one
    void CloseDialogue()
    {
        if (isTalking == false) // if the dialogue is already closed then don't try and close it again
        {
            return;
        }

        // closes dialogue
        Time.timeScale = 1;
        isTalking = false;
        curDialogueObject = null;
        dialogueCanvas.SetActive(false);
        curDialogueGiver = null;

        if (wasTriggeredViaInteraction == false)
        {
            return;
        }
        // if was triggered by the interaction system, then exit the interaction and reset the variable responsible for telling the system if it was triggered this way

        wasTriggeredViaInteraction = false;
        if (playerInteractor == null) // error handling
        {
            Debug.LogWarning("dialogue triggered via interaction, but playerInteractionController is null");
        }
        else
        {
            playerInteractor.ExitInteraction();
        }
    }
    #endregion

    #region Dialogue Event Triggering 
    void TriggerDialogueEvents(DialogueObject dialogueObject)
    {
        if (dialogueObject == null)
        {
            Debug.LogWarning("Dialogue object recieved was null");
            CloseDialogue();
            return;
        }

        if (dialogueObject.triggerDialogueEventsOnPlayer == true)
        {
            LoadAndExecuteDialogueEventsOnObject(playerDialogueEventsContainer, dialogueObject);
        }

        if (dialogueObject.triggerDialogueEventsOnDialogueGiver == true)
        {
            LoadAndExecuteDialogueEventsOnObject(curDialogueGiver, dialogueObject);
        }
    }

    bool LoadAndExecuteDialogueEventsOnObject(GameObject eventsHolder, DialogueObject dialogueObject)
    {
        if(eventsHolder ?? null) // you can't use '==' for detecting if a gameobject is null, unity overrides it, so you have to use this '??' thingy to do it 
        {
        }
        else
        {
            Debug.Log("eventsHolderObject was null, " + eventsHolder);
            return false;
        }

        if(dialogueObject == null)
        {
            Debug.Log("dialogueObject was null?, no tags passed through to events");
        }

        IDialogueEvent[] dialogueEvents = eventsHolder.GetComponents<IDialogueEvent>();

        bool anyTriggered = false;
        foreach(IDialogueEvent dialogueEvent in dialogueEvents)
        {
            anyTriggered = true;
            dialogueEvent.DialogueEvent(dialogueObject.customTags);
        }

        return anyTriggered;
    }
    #endregion

    #region Display Dialogue Functions
    // these just handle getting the data from the system to the ui objects

    void UpdateDialogueDisplayElements(DialogueObject dialogueObject)
    {
        if (dialogueObject == null)
        {
            Debug.LogWarning("Dialogue object recieved was null");
            CloseDialogue();
            return;
        }

        switch (dialogueObject.dialogueSpeaker.speakerType)
        {
            case DialogueSpeakerObject.SpeakerType.Player:
                DisplayPlayerDialogue(dialogueObject.dialogueText, dialogueObject.dialogueSpeaker.avatarSprite);
                break;
            case DialogueSpeakerObject.SpeakerType.NPC:
                DisplayNPCDialogue(dialogueObject.dialogueText, dialogueObject.dialogueSpeaker.nameText, dialogueObject.dialogueSpeaker.avatarSprite);
                break;
            case DialogueSpeakerObject.SpeakerType.Narration:
                DisplayNarrationDialogue(dialogueObject.dialogueText);
                break;
        }
    }

    void DisplayPlayerDialogue(string dialogueText, Sprite avatarSprite)
    {
        SetDialogueUIActive(playerDialogueUIObject);

        playerDialogueTextBox.text = dialogueText;
        playerAvatarImagerDisplay.sprite = avatarSprite;
    }

    void DisplayNPCDialogue(string dialogueText, string nameText, Sprite avatarSprite)
    {
        SetDialogueUIActive(npcDialogueUIObject);

        npcDialogueTextBox.text = dialogueText;
        npcNameTextBox.text = nameText;
        npcAvatarImageDisplay.sprite = avatarSprite;
    }

    void DisplayNarrationDialogue(string dialogueText)
    {
        SetDialogueUIActive(narrationDialogueUIObject);

        narrationDialogueTextBox.text = dialogueText;
    }

    void SetDialogueUIActive(GameObject dialogueUIObject)
    {
        playerDialogueUIObject.SetActive(false);
        npcDialogueUIObject.SetActive(false);
        narrationDialogueUIObject.SetActive(false);

        dialogueUIObject.SetActive(true);
    }
    #endregion

    /*
    private void ManageState()
    {
        if (!isTalking)
            return;

        var nextStates = dialogueState.GetNextStates();
        for (int index = 0; index < nextStates.Length; index++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + index))
            {
                NextSlide(index);
            }

        }

        if (Input.GetMouseButtonDown(0) || (Input.GetKeyDown(KeyCode.E)))
        {
            NextSlide(0);
        }
        textComponent.text = dialogueState.GetStateStory();

    }
    private void NextSlide(int j)
    {
        var nextStates = dialogueState.GetNextStates();
        dialogueState = nextStates[j];
        if (dialogueState.IsOver())
        {
            dialogueUI.SetActive(false);
            isTalking = false;

        }
        //dialogueState.NpcEffects();
        curSrpite = dialogueState.GetSprite();
        sprite.gameObject.GetComponent<SpriteRenderer>().sprite = AllSprites[curSrpite];
        isPlayer = dialogueState.IsPlayer();

        if (isPlayer)
        {
            Pos2.SetActive(false);
            sprite.transform.position = Pos1.transform.position;
            Pos1.SetActive(true);
        }
        else
        {
            Pos1.SetActive(false);
            sprite.transform.position = Pos2.transform.position;
            Pos2.SetActive(true);
        }
    }
    */
}
