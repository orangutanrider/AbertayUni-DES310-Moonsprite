using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractionController : MonoBehaviour
{
    [Header("Required References")]
    public PlayerMovement playerMovement;

    [Header ("Parameters")]
    public KeyCode interactionKey;
    public float interactionRange;
    public Vector3 originOffset;
    [Space]
    public LayerMask interactableMask;

    int interactingWithXScripts = 0;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(interactionKey) == true )
        {
            TryInteract();
        }
    }

    void TryInteract()
    {
        if(interactingWithXScripts > 0)
        {
            Debug.Log("Interaction Attempted, but player is still interacting with " + interactingWithXScripts + " script(s)");
            return;
        }

        TagList tagList = ToolBarUIScript.Instance.GetTagListOfActiveItem();

        IInteractionInterface[] interactionInterfaces = RayCastForInterface();
        foreach (IInteractionInterface interactionInterface in interactionInterfaces)
        {
            interactionInterface.InteractionEvent(this, tagList);
        }
    }

    public void ExitInteraction()
    {
        interactingWithXScripts = interactingWithXScripts - 1;
    }

    IInteractionInterface[] RayCastForInterface()
    {
        RaycastHit2D[] circleCastHits = Physics2D.CircleCastAll(transform.position + originOffset, interactionRange, Vector2.zero, 0, interactableMask);
        if(circleCastHits.Length == 0)
        {
            return null;
        }

        if(circleCastHits.Length == 1)
        {
            return circleCastHits[0].collider.gameObject.GetComponents<IInteractionInterface>();
        }
        else
        {
            RaycastHit2D playerDirectionCastHit = Physics2D.Raycast(transform.position + originOffset, playerMovement.GetFacingDirection(), interactionRange, interactableMask);
            if(playerDirectionCastHit == true)
            {
                return playerDirectionCastHit.collider.gameObject.GetComponents<IInteractionInterface>();
            }
            else
            {
                return circleCastHits[0].collider.gameObject.GetComponents<IInteractionInterface>();
            }
        }
    }
}
