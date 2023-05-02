using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SleepingCitizenScript : MonoBehaviour
{
    [Header("Required References")]
    public ParticleSystem sleepingParticleSystem;
    public DevestationTracker devestationTracker;

    const int scoreAdd = 10;
    static int totalTriggered = 0;

    bool woken = false;

    public void WakeUp()
    {
        if(woken == true) { return; }
        woken = true;

        sleepingParticleSystem.Stop();
        devestationTracker.score = devestationTracker.score + scoreAdd;

        totalTriggered++;
        if(totalTriggered >= 4)
        {
            devestationTracker.score = devestationTracker.score + 15;
            DevestationTracker.DevestationEventEntry allCitizensWokenEntry = DevestationTracker.instance.GetDevestationEventByName(DevestationTracker.allCitizensWokenLogName);
            DevestationTracker.instance.ConfirmDevestationEventPrevented(allCitizensWokenEntry);
        }
    }
}
