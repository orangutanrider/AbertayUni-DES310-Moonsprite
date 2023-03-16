using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class InventoryItem
{

    public ItemData itemData;
    public int stacksize;

    public InventoryItem(ItemData item)
    {
        itemData = item;
        AddToStack();
    }

    public void AddToStack()
    {
        stacksize++;
    }

    public void RemoveFromStack()
    {
        stacksize--;
    }

}
