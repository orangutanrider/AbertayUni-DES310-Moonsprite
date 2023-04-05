using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueObject", menuName = "Dialogue/DialogueObject")]
public class DialogueObject : ScriptableObject
{
    // Attribution: Vasco F + Dominic R

    [Header("Dialogue")]
    public DialogueSpeakerObject dialogueSpeaker = null;
    [TextArea(7, 7)] public string dialogueText = "(Placeholder text)";
    [Space]
    public DialogueObject[] nextDialogueObjects = null;

    [Header("Event Settings")]
    [Tooltip("The event will pass the taglist to the triggered script")] public bool triggerDialogueEventsOnDialogueGiver = false;
    [Tooltip("The event will pass the taglist to the triggered script")] public bool triggerDialogueEventsOnPlayer = false;
    public TagList customTags = null;
}
