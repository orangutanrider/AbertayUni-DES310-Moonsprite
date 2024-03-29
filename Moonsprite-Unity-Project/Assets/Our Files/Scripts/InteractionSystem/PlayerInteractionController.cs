using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractionController : MonoBehaviour
{
    public bool active = true;

    [Header("Required References")]
    public PlayerMovement playerMovement;

    [Header ("Parameters")]
    public float interactionRange;
    public Vector3 originOffset;
    [Space]
    public LayerMask interactableMask;
    [Space]
    public bool enableGizmos = false;
    public bool printData = false;

    public readonly KeyCode[] interactionKeys = new KeyCode[3] { KeyCode.G, KeyCode.KeypadEnter, KeyCode.Return };
    int interactingWithXScripts = 0;

    public AudioSource ErrorAudio;

    // Update is called once per frame
    void Update()
    {
        foreach(KeyCode keyCode in interactionKeys)
        {
            if (Input.GetKeyDown(keyCode) == true && active == true)
            {
                TryInteract();
            }
        }
    }

    void OnDrawGizmos()
    {
        if (enableGizmos == true)
        {
            Gizmos.DrawWireSphere(transform.position + originOffset, interactionRange);
        }
    }

    void TryInteract()
    {
        if(interactingWithXScripts > 0)
        {
            Debug.Log("Interaction Attempted, but player is still interacting with " + interactingWithXScripts + " script(s)");
            return;
        }

        TagList activeItemTags = Inventory.instance.TagListOfCurrentItem();

        IInteractable[] interactionInterfaces = RayCastForInterface();

        if(interactionInterfaces == null)
        {
            ErrorAudio.Play();
            return;
        }

        foreach (IInteractable interactionInterface in interactionInterfaces)
        {
            InteractWith(interactionInterface, activeItemTags);
        }
    }

    public void ExitInteraction()
    {
        interactingWithXScripts--;
        if(interactingWithXScripts <= 0)
        {
            PlayerStateMachine.instance.StateUpdate(PlayerStateMachine.PlayerState.NoState);
        }

        if(printData == true)
        {
            Debug.Log("Call recieved to exit interaction, current number of active interactions is " + interactingWithXScripts);
        }
    }

    void InteractWith(IInteractable interactable, TagList tags)
    {
        bool stateUpdate = PlayerStateMachine.instance.StateUpdate(PlayerStateMachine.PlayerState.Interacting);
        if (stateUpdate == true)
        {
            interactingWithXScripts++;
        }
        interactable.InteractionEvent(this, tags);

        if (printData == true)
        {
            Debug.Log("Interacting with a new script, current number of scripts being interacted with is " + interactingWithXScripts);
        }
    }

    IInteractable[] RayCastForInterface()
    {
        // first check if there are hits
        // then check if there's more than one
        // then check if any of them are higher priority than the others (pick the highest priority one if they aren't all the same priority)
        // then check if the player is facing an interactable (pick that one if they're all the same priority)
        // finally, if they're all the same priority and the player isn't alligned with any of them, then pick the one at the start of the raycast hits array

        RaycastHit2D[] circleCastHits = Physics2D.CircleCastAll(transform.position + originOffset, interactionRange, Vector2.zero, 0, interactableMask);
        if(circleCastHits.Length == 0) // first check if there are hits
        {
            return null;
        }

        if(circleCastHits.Length == 1) // then check if there's more than one
        {
            return circleCastHits[0].collider.gameObject.GetComponents<IInteractable>();
        }

        // then check if any of them are higher priority than the others
        IInteractable[] highestPriorityInteractables = null;
        bool allSamePriority = true;
        int highestPriority = circleCastHits[0].collider.gameObject.GetComponent<IInteractable>().InteractionPriority;
        foreach (RaycastHit2D circleCastHit in circleCastHits)
        {
            IInteractable[] currentHitInteractables = circleCastHit.collider.gameObject.GetComponents<IInteractable>();
            foreach (IInteractable interactable in currentHitInteractables)
            {
                if (interactable.InteractionPriority > highestPriority)
                {
                    highestPriorityInteractables = currentHitInteractables;
                    highestPriority = interactable.InteractionPriority;
                    allSamePriority = false;
                }
            }
        }
        if (allSamePriority == false) // (pick the highest priority one if they aren't all the same priority)
        {
            return highestPriorityInteractables;
        }

        // then check if the player is facing an interactable
        RaycastHit2D playerDirectionCastHit = Physics2D.Raycast(transform.position + originOffset, playerMovement.GetFacingDirection(), interactionRange, interactableMask);
        if (playerDirectionCastHit == true) // (pick that one if they're all the same priority)
        {
            return playerDirectionCastHit.collider.gameObject.GetComponents<IInteractable>();
        }

        // finally, if they're all the same priority and the player isn't alligned with any of them, then pick the one at the start of the raycast hits array
        return circleCastHits[0].collider.gameObject.GetComponents<IInteractable>();
    }
}
