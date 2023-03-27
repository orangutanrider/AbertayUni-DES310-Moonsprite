using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerCameraManager : MonoBehaviour
{
    public CinemachineConfiner cinemachineConfiner = null;

    [HideInInspector] public static PlayerCameraManager instance = null;

    void Awake()
    {
        instance = this;
    }

    public void ConfineCameraWith(Collider2D collider2D, float damping = 0)
    {
        if(collider2D == null)
        {
            FreeCameraFromConfiner();
            return;
        }

        cinemachineConfiner.m_BoundingShape2D = collider2D;
        cinemachineConfiner.m_Damping = damping;
    }

    public void FreeCameraFromConfiner()
    {
        cinemachineConfiner.m_BoundingShape2D = null;
        cinemachineConfiner.m_Damping = 0;
    }
}
