using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbienceManagerPartnerScript : MonoBehaviour
{
    [Header("Required References")]
    public StormAmbienceManager ambienceManager;

    public void StartProxyLerpCoroutine(IEnumerator proxyLerp)
    {
        StartCoroutine(proxyLerp);
    }
}
