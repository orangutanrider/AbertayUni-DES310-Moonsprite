using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueObject", menuName = "Dialogue/DialogueSpeakerObject")]
public class DialogueSpeakerObject : ScriptableObject
{
    public bool isPlayer;
    [Space]
    public string nameText;
    public Sprite avatarSprite;
}
