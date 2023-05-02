using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevestationTracker : MonoBehaviour
{
    [System.Serializable]
    public class DevestationEventEntry 
    {
        public string name;
        [TextArea] public string description;
        [Space]
        public bool hasHappened = false;
        public bool wasPrevented = false;

        public DevestationEventEntry(string _name, string _description, bool _hasHappened, bool _wasPrevented)
        {
            name = _name;
            description = _description;
            hasHappened = _hasHappened;
            wasPrevented = _wasPrevented;
        }
    }

    public List<DevestationEventEntry> devestationEventEntries = new List<DevestationEventEntry>();
    public int score = 0;

    public const string allCitizensWokenLogName = "All Citizens Woken";
    public const string squatterInjuredLogName = "Squatter Injured";

    [HideInInspector] public static DevestationTracker instance;
    void Awake()
    {
        instance = this;
    }

    #region public functions
    public DevestationEventEntry GetDevestationEventByName(string name)
    {
        int numFound = 0;
        DevestationEventEntry devestationEventToReturn = null;
        foreach (DevestationEventEntry devestationEvent in devestationEventEntries)
        {
            if(devestationEvent.name == name)
            {
                devestationEventToReturn = devestationEvent;
                numFound++;
            }
        }

        if(numFound > 1)
        {
            Debug.LogWarning("Error, more than one devestation event with the same name, returning the one at the end of the list");
        }

        if(numFound == 0)
        {
            Debug.Log("Couldn't find event with name " + name);
        }

        return devestationEventToReturn;
    }

    public void ConfirmDevestationEventHappened(DevestationEventEntry devestationEvent)
    {
        devestationEvent.hasHappened = true;
    }

    public void ConfirmDevestationEventPrevented(DevestationEventEntry devestationEvent)
    {
        devestationEvent.wasPrevented = true;
    }
    #endregion
}
