using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Dialogue State")]
public class DialogueState : ScriptableObject
{

    [TextArea(14, 10)][SerializeField] string storyText;
    [TextArea(14, 10)][SerializeField] string NameText;
    [SerializeField] DialogueState[] nextStates;
    [SerializeField] bool isPlayer;
    [SerializeField] bool isOver;
    [SerializeField] int curSprite;
    [SerializeField] GameObject[] NpcAff;



    public string GetStateStory()
    {
        return storyText;
    }

    public string GetStateName()
    {
        return NameText;
    }

    public int GetSprite()
    {
        return curSprite;
    }

    public bool IsPlayer()
    {
        return isPlayer;
    }
    public bool IsOver()
    {
        return isOver;
    }

    public DialogueState[] GetNextStates()
    {
        return nextStates;
    }

    public void NpcEffects()
    {

      // NpcAff = GameObject.FindGameObjectsWithTag("NPC");

        if (NpcAff == null)
            return;

        
        for (int i = 0; i < NpcAff.Length; i++)
        {
            NPC_BasicLogic curNpc = NpcAff[i].GetComponent<NPC_BasicLogic>();
            curNpc.NPCNextDialogue();
        }


    }

}