using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    [Header("Required References")]
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

    bool isTalking = false;
    DialogueObject curDialogueObject = null;
    GameObject dialogueGiver = null;

    void Update()
    {
        DialogueSystemInputs();
    }

    void DialogueSystemInputs()
    {
        if(curDialogueObject == null)
        {
            return;
        }

        // generic next dialogue input
        if (Input.GetMouseButtonUp(0) == true || Input.GetKeyUp(PlayerInteractionController.interactionKey) == true)
        {
            NextDialogueObject();
        }

        // reply input
        for (int index = 0; index < curDialogueObject.nextDialogueObjects.Length; index++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + index))
            {
                NextDialogueObject(index);
            }
        }
    }

    public void StartDialogue(DialogueObject dialogueObject)
    {
        if(dialogueObject == null)
        {
            CloseDialogue();
            return;
        }

        Time.timeScale = 0;
        isTalking = true;
        curDialogueObject = dialogueObject;
        dialogueCanvas.SetActive(true);

        UpdateDialogueDisplayElements(dialogueObject);
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

        if(curDialogueObject.nextDialogueObjects.Length - 1 < inputIndex)
        {
            StartDialogue(curDialogueObject.nextDialogueObjects[0]);
        }

        StartDialogue(curDialogueObject.nextDialogueObjects[inputIndex]);
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
