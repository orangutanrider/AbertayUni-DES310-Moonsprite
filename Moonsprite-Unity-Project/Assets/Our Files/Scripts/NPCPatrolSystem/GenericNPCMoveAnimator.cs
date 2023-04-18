using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericNPCMoveAnimator : MonoBehaviour, INPCMoveAnimator
{
    public Animator npcAnimator;
    public SpriteRenderer spriteRenderer;

    const string xMove = "xMove";
    const string yMove = "yMove";

    void INPCMoveAnimator.UpdateWalkCycleAnimation(Vector2 moveDirection)
    {
        npcAnimator.SetFloat(xMove, moveDirection.x);
        npcAnimator.SetFloat(yMove, moveDirection.y);

        if(moveDirection.x > 0.05f)
        {
            spriteRenderer.flipX = true;
        }
        if(moveDirection.x < -0.05f)
        {
            spriteRenderer.flipX = false;
        }
    }
}
