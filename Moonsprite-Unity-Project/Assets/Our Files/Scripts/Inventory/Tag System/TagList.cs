using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TagList
{
    public List<CustomTag> tags;

    public bool HasTag(CustomTag passedTag)
    {

        return tags.Contains(passedTag);

    }

    public bool HasTag(string passedTag)
    {

        return tags.Exists(t => t.Name == passedTag);

    }

}
