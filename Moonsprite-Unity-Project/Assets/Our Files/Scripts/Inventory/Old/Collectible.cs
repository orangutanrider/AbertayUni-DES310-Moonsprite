using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Obsolete("This doesn't do anything other than declare the Collect function (so it's essentially the same thing as an interace) so it has been replaced by ICollectible")]
public abstract class Collectible : MonoBehaviour, ICollectible
{
    /*
    public abstract void Collect();
    */
}
