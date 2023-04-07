using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneReferenceHolder : MonoBehaviour
{
    // this is just so that, if we change the names of the scenes (then we'd have to edit the string references to them)
    // so this is just having them all here as constants so that, if they were to changed, we would only have to edit the references to them (via string) here

    public const string mainOutdoorsScene = "MainOutdoors";

    public const string menuBaseScene = "MenuBaseScene";
    public const string mainMenuScene = "Main Menu";
    public const string fadeScene = "FadeScene";
}
