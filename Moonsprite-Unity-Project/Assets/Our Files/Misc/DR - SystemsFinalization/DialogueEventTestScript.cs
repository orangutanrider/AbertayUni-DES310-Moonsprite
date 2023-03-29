using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueEventTestScript : MonoBehaviour, IDialogueEvent
{
    public CustomTag testTag = null;

    void IDialogueEvent.DialogueEvent(TagList tagList)
    {
        Debug.Log("testScript: Dialogue event succesfully triggered");

        if (tagList.HasTag(testTag) == true)
        {
            Debug.Log("testScript: Tag list successfully passed");
        }
    }
}
