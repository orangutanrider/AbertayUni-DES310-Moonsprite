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
    public GameObject otherDialogueUIObject;
    public TMP_Text otherNameTextBox;
    public TMP_Text otherDialogueTextBox;
    public Image otherAvatarImageDisplay;

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
        if (Input.GetMouseButtonUp(0) == true || Input.GetKeyDown(PlayerInteractionController.interactionKey) == true)
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

    void ActivateDialogue(DialogueObject dialogueObject)
    {
        if(dialogueObject == null)
        {
            CloseDialogue();
            return;
        }

        dialogueCooldownTimer = 0;

        curDialogueObject = dialogueObject;

        TriggerDialogueEvents(dialogueObject);
        UpdateDialogueDisplayElements(dialogueObject);
    }

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
        if(eventsHolder ?? null)
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

    void CloseDialogue()
    {
        if(isTalking == false) // once pause system is in game this will need to take that into account
        {
            return;
        }

        Time.timeScale = 1;
        isTalking = false;
        curDialogueObject = null;
        dialogueCanvas.SetActive(false);
        curDialogueGiver = null;

        if (wasTriggeredViaInteraction == false)
        {
            return;
        }

        wasTriggeredViaInteraction = false;

        if (playerInteractor == null)
        {
            Debug.LogWarning("dialogue triggered via interaction, but playerInteractionController is null");
        }
        else
        {
            playerInteractor.ExitInteraction();
        }
    }

    void UpdateDialogueDisplayElements(DialogueObject dialogueObject)
    {
        if(dialogueObject == null)
        {
            Debug.LogWarning("Dialogue object recieved was null");
            CloseDialogue();
            return;
        }

        if (dialogueObject.dialogueSpeaker.isPlayer == true)
        {
            otherDialogueUIObject.SetActive(false);
            playerDialogueUIObject.SetActive(true);

            playerDialogueTextBox.text = dialogueObject.dialogueText;
            playerAvatarImagerDisplay.sprite = dialogueObject.dialogueSpeaker.avatarSprite;
        }
        else
        {
            otherDialogueUIObject.SetActive(true);
            playerDialogueUIObject.SetActive(false);

            otherDialogueTextBox.text = dialogueObject.dialogueText;
            otherNameTextBox.text = dialogueObject.dialogueSpeaker.nameText;
            otherAvatarImageDisplay.sprite = dialogueObject.dialogueSpeaker.avatarSprite;
        }
    }

    void NextDialogueObject(int inputIndex = 0)
    {
        if(curDialogueObject == null)
        {
            CloseDialogue();
            return;
        }

        if(curDialogueObject.nextDialogueObjects == null)
        {
            CloseDialogue();
            return;
        }

        if (curDialogueObject.nextDialogueObjects.Length == 0)
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
