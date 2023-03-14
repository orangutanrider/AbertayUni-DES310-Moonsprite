using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TagList : MonoBehaviour
{

    private List<CustomTag> tags;

    public List<CustomTag> All => tags;

    public bool HasTag(CustomTag passedTag)
    {

        return tags.Contains(passedTag);

    }

    public bool HasTag(string passedTag)
    {

        return tags.Exists(t => t.Name == passedTag);

    }

}
