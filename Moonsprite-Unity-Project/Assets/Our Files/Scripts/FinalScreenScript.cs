using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FinalScreenScript : MonoBehaviour
{
    [Header("Required References")]
    public TMP_Text titleText;
    public TMP_Text descriptionText;
    [Space]
    public TMP_Text scoreText;

    [Header("Perfect Ending")]
    public int perfectEndingScoreThreshold = 0;
    public string perfectEndingTitleText = "null";
    [TextArea(10, 100)]
    public string perfectEndingDescriptionText = "null";

    [Header("Near Perfect Ending")]
    public string nearPerfectEndingTitleText = "null";
    [TextArea(10, 100)]
    public string nearPerfectEndingDescriptionText = "null";

    [Header("Squatter Injured")]
    public string squatterInjuredTitleText = "null";
    [TextArea(10, 100)]
    public string squatterInjuredDescriptionText = "null";

    [Header("Citizens Sleeping")]
    public string citizensSleepingTitleText = "null";
    [TextArea(10, 100)]
    public string citizensSleepingDescriptionText = "null";

    [Header("Full Destruction")]
    public string fullDestructionTitleText = "null";
    [TextArea(10, 100)]
    public string fullDestructionDescriptionText = "null";

    // hard coded and not great in the inspector
    // could use scriptable objects to make it nicer, but doesn't really matter at this point in development

    void Start()
    {
        DevestationTracker.DevestationEventEntry squatterInjuredEntry = DevestationTracker.instance.GetDevestationEventByName(DevestationTracker.squatterInjuredLogName);
        DevestationTracker.DevestationEventEntry allCitizensWokenEntry = DevestationTracker.instance.GetDevestationEventByName(DevestationTracker.allCitizensWokenLogName);
        int score = DevestationTracker.instance.score;

        scoreText.text = score.ToString();

        // perfect ending
        if (score >= perfectEndingScoreThreshold && squatterInjuredEntry.wasPrevented == true && allCitizensWokenEntry.wasPrevented == true)
        {
            titleText.text = perfectEndingTitleText;
            descriptionText.text = perfectEndingDescriptionText;
            return;
        }

        // near perfect ending
        if(squatterInjuredEntry.wasPrevented == true && allCitizensWokenEntry.wasPrevented == true)
        {
            titleText.text = nearPerfectEndingTitleText;
            descriptionText.text = nearPerfectEndingDescriptionText;
            return;
        }

        // squatter injured ending
        if(squatterInjuredEntry.wasPrevented == false)
        {
            titleText.text = squatterInjuredTitleText;
            descriptionText.text = squatterInjuredDescriptionText;
            return;
        }

        // citizens sleeping ending
        if(allCitizensWokenEntry.wasPrevented == false)
        {
            titleText.text = squatterInjuredTitleText;
            descriptionText.text = squatterInjuredDescriptionText;
            return;
        }

        // full destruction ending
        titleText.text = fullDestructionTitleText;
        descriptionText.text = fullDestructionDescriptionText;
    }
}
