using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class InventoryItem
{
    public ItemData itemData;
    [Space]
    [HideInInspector] public bool itemActionsFailLoaded = false;
    [HideInInspector] public bool itemActionsLoaded = false;
    public IItemAction[] itemActions = null; // interface fields can't be serialized, so we have to go through a gameobject to get these references

    public InventoryItem(ItemData item, bool actionsFailLoaded = false , bool actionsLoaded = false, IItemAction[] actions = null)
    {
        itemData = item;

        itemActionsLoaded = actionsLoaded;
        itemActions = actions;
    }
}
