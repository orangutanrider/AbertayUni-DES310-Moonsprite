using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractionInterface
{
    public PlayerInteractionController InteractionEvent(CustomTag tag = null);
    
    [Tooltip("You must tell the interactor if they're now free to interact again, do this by calling the ExitInteraction function on the PlayerInteractionController")]
    public void FinishInteraction();
}
