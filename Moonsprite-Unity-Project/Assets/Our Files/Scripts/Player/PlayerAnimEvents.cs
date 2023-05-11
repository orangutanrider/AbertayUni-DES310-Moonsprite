using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimEvents : MonoBehaviour
{
    [Header("Required References")]
    public PlayerFootstepPlayer playerFootstepPlayer = null;

    public void FootstepAnimEvent()
    {
        playerFootstepPlayer.PlayFootstepSoundAnimTrigger();
    }
}
