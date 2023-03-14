using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static System.Net.Mime.MediaTypeNames;
using System.Diagnostics;
using Debug = UnityEngine.Debug;
using System.Threading;

public class DialogueText : MonoBehaviour
{

    [SerializeField] TMP_Text textComponent;
    [SerializeField] DialogueState startingState;
    [SerializeField] bool isPlayer;
    [SerializeField] GameObject sprite;
    [SerializeField] GameObject dialogueUI;
    [SerializeField] GameObject Pos1;
    [SerializeField] GameObject Pos2;
    [SerializeField] Sprite[] AllSprites;
    [SerializeField] int curSrpite;
    DialogueState dialogueState;
    public bool isTalking;


    public void ChangeIsTalking()
    {
        isTalking = !isTalking;
    }

    // Use this for initialization
    void Start()
    {
        dialogueState = startingState;
        textComponent.text = dialogueState.GetStateStory();
        isPlayer = dialogueState.IsPlayer();
        isTalking = true;

    }

    void Update()
    {
       
        ManageState();
       
    }


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
    public void StartDialogue(DialogueState index)
    {

        dialogueState = index;
        var nextStates = dialogueState.GetNextStates();
        isTalking = true;

        curSrpite = dialogueState.GetSprite();
        sprite.gameObject.GetComponent<SpriteRenderer>().sprite = AllSprites[curSrpite];

        if (isPlayer)
        {
            sprite.transform.position = Pos1.transform.position;

        }
        else
        {
            sprite.transform.position = Pos2.transform.position;

        }

        dialogueUI.SetActive(true);
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
            sprite.transform.position = Pos1.transform.position;
           
        }
        else
        {
            sprite.transform.position = Pos2.transform.position;
           
        }
    }
}