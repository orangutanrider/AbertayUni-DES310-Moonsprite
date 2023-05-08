using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventoryItem
{
    public bool empty = true;
    public ItemData itemData = null;

    [HideInInspector] public bool itemActionsFailLoaded = false;
    [HideInInspector] public bool itemActionsLoaded = false;

    [HideInInspector] public int previousIndex = 0;

    public IItemAction[] itemActions = null; // interface fields can't be serialized, so we have to go through a gameobject to get these references

    public InventoryItem(ItemData item, bool _empty, bool actionsFailLoaded = false, bool actionsLoaded = false, IItemAction[] actions = null)
    {
        empty = _empty;
        itemData = item;
        itemActionsFailLoaded = actionsFailLoaded;
        itemActionsLoaded = actionsLoaded;
        itemActions = actions;
    }
}
