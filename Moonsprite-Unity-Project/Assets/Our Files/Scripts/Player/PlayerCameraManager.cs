using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerCameraManager : MonoBehaviour
{
    public CinemachineConfiner2D cinemachineConfiner2D = null;

    [HideInInspector] public static PlayerCameraManager instance = null;

    void Awake()
    {
        instance = this;
    }

    public void ConfineCameraWith(Collider2D collider2D)
    {
        if(collider2D == null)
        {
            FreeCameraFromConfiner();
            return;
        }

        cinemachineConfiner2D.m_BoundingShape2D = collider2D;
    }

    public void FreeCameraFromConfiner()
    {
        cinemachineConfiner2D.m_BoundingShape2D = null;
    }
}
