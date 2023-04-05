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

    [Header("README")]
    [TextArea(10, 10)] public string readMeText = "Dialogue speaker objects hold data pertaining to the person giving the dialogue, stuff like the sprite file for their avatar and their name." + System.Environment.NewLine +
        "Dialogue text is where you enter the dialogue itself." + System.Environment.NewLine +
        System.Environment.NewLine +
        "The next dialogue objects list is where you link this object to the next one, it's a list so that it can support multiple dialogue replies (branching dialogue)." + System.Environment.NewLine +
        "So, if the dialogue doesn't branch at this point then just put in one list entry and reference the next dialogue object there. If it ends at this point then leave the list empty, and if it branches here then add multiple entries." + System.Environment.NewLine +
        System.Environment.NewLine +
        "The events are for triggering custom scripts via dialogue, if you want this dialogue object to trigger something then check one of those boxes." + System.Environment.NewLine +
        "The way the system works is by getting all scripts of the interface IDialogueEvent on either the player or the game object that triggered this dialogue." + System.Environment.NewLine +
        "So you check the boxes for if you're wanting to trigger them on the player, the giver, or both." + System.Environment.NewLine +
        "When the events are called, it passes this object's tag list to the event, so that we can specify some information with the events. Stuff like 'this event only does something if x tag is recieved'";
}
