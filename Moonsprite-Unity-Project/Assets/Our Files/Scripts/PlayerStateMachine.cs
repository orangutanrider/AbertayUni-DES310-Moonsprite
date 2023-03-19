using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    [Header("Required References")]
    public PlayerMovement movement;
    public PlayerInteractionController interactionController;
    public PlayerInventoryController inventoryController;

    [Header("PlayerState")]
    public PlayerState playerState = PlayerState.NoState;
    public enum PlayerState
    {
        NoState,
        Interacting
    }

    [HideInInspector] public static PlayerStateMachine instance = null;

    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        ComputeStateLogicToPlayerScripts();
    }

    void ComputeStateLogicToPlayerScripts()
    {
        switch (playerState)
        {
            case PlayerState.NoState:
                movement.active = true;
                interactionController.active = true;
                inventoryController.active = true;
                break;
            case PlayerState.Interacting:
                movement.active = false;
                interactionController.active = false;
                inventoryController.active = false;
                break;
        }
    }

    public bool StateUpdate(PlayerState newState)
    {
        if (playerState == PlayerState.NoState)
        {
            playerState = newState;
            return true;
        }

        if (playerState == PlayerState.Interacting)
        {
            playerState = newState;
            return true;
        }

        playerState = newState;
        return true;
    }
}
