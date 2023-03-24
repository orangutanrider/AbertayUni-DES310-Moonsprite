using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueObject : ScriptableObject
{
    public DialogueObject[] nextDialogueObjects = null;

    [Header("Dialogue Settings")]
    [Tooltip("The event will pass the taglist to the triggered script")] public bool triggerDialogueEventsOnDialogueGiver = false;
    [Tooltip("The event will pass the taglist to the triggered script")] public bool triggerDialogueEventsOnPlayer = false;
    public TagList customTags = null;
    [Space]
    public DialogueSpeakerObject dialogueSpeaker = null;
    [TextArea] public string dialogueText = "(Placeholder text)";
}
