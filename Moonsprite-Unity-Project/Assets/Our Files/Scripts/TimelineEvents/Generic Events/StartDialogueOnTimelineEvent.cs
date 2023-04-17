using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartDialogueOnTimelineEvent : MonoBehaviour, ITimelineEvent
{
    public DialogueObject dialogueObject;

    void ITimelineEvent.TimelineEvent()
    {
        DialogueManager.instance.StartNewDialogue(dialogueObject, gameObject);
    }
}
