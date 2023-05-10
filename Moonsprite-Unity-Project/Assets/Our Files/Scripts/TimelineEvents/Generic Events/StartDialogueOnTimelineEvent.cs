using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartDialogueOnTimelineEvent : MonoBehaviour, ITimelineEvent
{
    public DialogueObject dialogueObject;

    void ITimelineEvent.TimelineEvent(int eventIndex)
    {
        DialogueManager.instance.StartNewDialogue(dialogueObject, gameObject);
    }
}
