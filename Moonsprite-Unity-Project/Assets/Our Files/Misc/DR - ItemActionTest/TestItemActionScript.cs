using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestItemActionScript : MonoBehaviour, IItemAction
{
    void IItemAction.TriggerItemAction()
    {
        Debug.Log("Item action test succesful");
    }
}
