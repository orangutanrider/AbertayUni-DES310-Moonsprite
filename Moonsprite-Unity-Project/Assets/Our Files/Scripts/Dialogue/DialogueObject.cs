using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueObject", menuName = "Dialogue/DialogueObject")]
public class DialogueObject : ScriptableObject
{
    // Attribution: Vasco F + Dominic R

    [Header("(add multiple for when the player has reply options)")]
    [Header("(leave empty to exit the dialogue)")]
    public DialogueObject[] nextDialogueObjects = null;

    [Header("Dialogue Event Settings")]
    [Tooltip("The event will pass the taglist to the triggered script")] public bool triggerDialogueEventsOnDialogueGiver = false;
    [Tooltip("The event will pass the taglist to the triggered script")] public bool triggerDialogueEventsOnPlayer = false;
    public TagList customTags = null;

    [Header("Dialogue display settings")]
    public DialogueSpeakerObject dialogueSpeaker = null;
    [TextArea(6,5)] public string dialogueText = "(Placeholder text)";
}
