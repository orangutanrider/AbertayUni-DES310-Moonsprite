using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineScreenShaker : MonoBehaviour
{
    public static CinemachineScreenShaker Instance
    {
        get
        {
            return instance;
        }
        private set
        {
            instance = value;
        }
    }
    static CinemachineScreenShaker instance;

    [Header("Required References")]
    public CinemachineImpulseListener impulseListener;
    public CinemachineImpulseSource baseImpulseSource;

    [Header("Parameters")]
    public float globalShakeForce = 1;

    CinemachineImpulseDefinition impulseDefinition;

    void Start()
    {
        instance = this;
    }

    public void ShakeCamera(CinemachineImpulseSource impulseSource)
    {
        impulseSource.GenerateImpulseWithForce(globalShakeForce);
    }

    public void ShakeCameraFromScriptableObject(ScreenshakeParameters shakeParameters, CinemachineImpulseSource impulseSource)
    {
        SetupShakeSettings(shakeParameters, impulseSource);
        impulseSource.GenerateImpulseWithForce(shakeParameters.impactForce);
    }

    public void ShakeCameraFromScriptableObject(ScreenshakeParameters shakeParameters)
    {
        SetupShakeSettings(shakeParameters, baseImpulseSource);
        baseImpulseSource.GenerateImpulseWithForce(shakeParameters.impactForce);
    }

    public void ShakeCameraFromScriptableObjectWithDistanceFalloff(ScreenshakeParameters shakeParameters, Vector3 origin)
    {
        SetupShakeSettings(shakeParameters, baseImpulseSource);

        float distance = Vector2.Distance(PlayerStateMachine.instance.transform.position, origin);
        float distanceFallOffValue = shakeParameters.distanceFallOffCurve.Evaluate(distance);
        baseImpulseSource.GenerateImpulseWithForce(shakeParameters.impactForce * distanceFallOffValue);
    }

    void SetupShakeSettings(ScreenshakeParameters shakeParameters, CinemachineImpulseSource impulseSource)
    {
        impulseDefinition = impulseSource.m_ImpulseDefinition;

        // source settings
        impulseDefinition.m_ImpulseDuration = shakeParameters.impactTime;
        impulseSource.m_DefaultVelocity = shakeParameters.defaultVelocity;
        impulseDefinition.m_CustomImpulseShape = shakeParameters.impulseCurve;

        // listener settings
        impulseListener.m_ReactionSettings.m_AmplitudeGain = shakeParameters.listenerAmplitude;
        impulseListener.m_ReactionSettings.m_FrequencyGain = shakeParameters.listenerFrequency;
        impulseListener.m_ReactionSettings.m_Duration = shakeParameters.listenerDuration;
    }
}
