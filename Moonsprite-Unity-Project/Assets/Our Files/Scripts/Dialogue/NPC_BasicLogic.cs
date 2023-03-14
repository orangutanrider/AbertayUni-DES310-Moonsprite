using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class NPC_BasicLogic : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public  DialogueState curDialogue;
    [SerializeField] DialogueState[] NPCDialogue;
    [SerializeField] int index;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        index = 0;
    }

    public void ChangeColor()
    {
        spriteRenderer.color = Color.blue;
    }

    public void ChangeBack()
    {
        spriteRenderer.color = Color.white;
    }
    public DialogueState GetDialogueState()
    {

        return curDialogue;
    }

    public void NPCNextDialogue()
    {
        
        if (index < NPCDialogue.Length -1)
        {
            index++;
        }
        curDialogue = NPCDialogue[index];
      
    }
   

}
