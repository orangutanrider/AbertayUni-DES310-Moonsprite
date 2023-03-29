using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueObject", menuName = "Dialogue/DialogueSpeakerObject")]
public class DialogueSpeakerObject : ScriptableObject
{
    // Attribution: Vasco F + Dominic R

    public bool isPlayer;
    [Space]
    public string nameText;
    public Sprite avatarSprite;
}
