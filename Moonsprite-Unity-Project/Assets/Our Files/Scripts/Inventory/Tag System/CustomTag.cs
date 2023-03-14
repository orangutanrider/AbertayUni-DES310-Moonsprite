using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName ="New Tag", menuName ="Tags/NewTag")]

public class CustomTag : ScriptableObject
{
    public string Name => name;
}
