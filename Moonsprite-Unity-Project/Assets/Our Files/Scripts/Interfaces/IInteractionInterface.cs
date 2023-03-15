using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractionInterface
{
    public PlayerInteractionController InteractionEvent(CustomTag tag = null);
    public void ExitInteraction();
}
