using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetAnimatorParameterOnTimelineEvent : MonoBehaviour, ITimelineEvent
{
    public Animator animator;

    [Header("Bool")]
    public bool setBoolParameter = true;
    public string boolParameterName = "bool";
    public bool boolParameterValue = false;

    void ITimelineEvent.TimelineEvent()
    {
        if(setBoolParameter == true)
        {
            animator.SetBool(boolParameterName, boolParameterValue);
        }
    }
}
