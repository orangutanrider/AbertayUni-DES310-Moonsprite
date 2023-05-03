using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventoryItem
{
    public ItemData itemData = null;
    [Space]
    public int slotIndex = -1; // -1 represents outside the bounds of the ui
    [Space]
    public bool itemActionsFailLoaded = false;
    public bool itemActionsLoaded = false;

    public IItemAction[] itemActions = null; // interface fields can't be serialized, so we have to go through a gameobject to get these references

    public InventoryItem(ItemData item, bool actionsFailLoaded = false, bool actionsLoaded = false, IItemAction[] actions = null)
    {
        itemData = new ItemData(item);
        itemActionsFailLoaded = actionsFailLoaded;
        itemActionsLoaded = actionsLoaded;
        itemActions = actions;
    }
}
