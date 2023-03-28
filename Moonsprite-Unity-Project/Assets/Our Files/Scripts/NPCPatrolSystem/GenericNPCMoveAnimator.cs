using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericNPCMoveAnimator : MonoBehaviour, INPCMoveAnimator
{
    public Animator npcAnimator;

    const string xMove = "xMove";
    const string yMove = "yMove";

    void INPCMoveAnimator.UpdateWalkCycleAnimation(Vector2 moveDirection)
    {
        npcAnimator.SetFloat(xMove, moveDirection.x);
        npcAnimator.SetFloat(yMove, moveDirection.y);
    }
}
