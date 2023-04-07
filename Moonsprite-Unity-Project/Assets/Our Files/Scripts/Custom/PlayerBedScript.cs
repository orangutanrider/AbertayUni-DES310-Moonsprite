using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerBedScript : MonoBehaviour, IInteractable
{
    [SerializeField] int interactionPriority = 0;
    int IInteractable.InteractionPriority
    {
        get
        {
            return interactionPriority;
        }
        set
        {
            interactionPriority = value;
        }
    }

    void IInteractable.InteractionEvent(PlayerInteractionController playerInteractionController, TagList activeItemTagList)
    {
        // there should be somekind of confirmation step in this
        // (i.e. asking the player "are you sure")

        // fade out

        // play cut-scene part 1 (camera pan over town, player picks up newspaper)
        // play cut-scene part 2 (news paper unrolled and viewed)

        // transition back to main menu


        SceneManager.LoadScene(SceneReferenceHolder.mainMenuScene, LoadSceneMode.Single);
    }
}
