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

    bool interacting;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(interactionKey) == true)
        {

        }
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
