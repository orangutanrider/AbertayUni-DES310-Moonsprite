using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class InventoryItem
{
    public ItemData itemData;
    [Space]
    public GameObject objectToGetItemActionsFrom;
    [HideInInspector] public bool itemActionsLoaded = false;
    public IItemAction[] itemActions = null; // interface fields can't be serialized, so we have to go through a gameobject to get these references

    public InventoryItem(ItemData item, GameObject getActionsFrom = null, bool actionsLoaded = false, IItemAction[] actions = null)
    {
        itemData = item;

        objectToGetItemActionsFrom = getActionsFrom;
        itemActionsLoaded = actionsLoaded;
        itemActions = actions;
    }
}
