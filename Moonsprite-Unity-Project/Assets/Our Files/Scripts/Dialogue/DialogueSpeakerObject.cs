using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueObject", menuName = "Dialogue/DialogueSpeakerObject")]
public class DialogueSpeakerObject : ScriptableObject
{
    // Attribution: Vasco F + Dominic R

    public enum SpeakerType
    {
        Player,
        NPC,
        Narration
    }

    public SpeakerType speakerType;
    [Space]
    public string nameText;
    public Sprite avatarSprite;
}
