using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ToolbarSlotScript : MonoBehaviour
{
    // Attribution: Maceij Wolski + Dominic Rooney

    public ToolBarSlotSharedParameters toolBarSlotSharedParameters;
    [Space]
    public Image slotBackgroundImageDisplay;
    public Image itemIconImageDisplay;

    void Start()
    {
        ShowEmptySlot();
    }

    public void ShowEmptySlot()
    {
        // background
        slotBackgroundImageDisplay.sprite = toolBarSlotSharedParameters.emptySlotImage;

        // icon
        Color invisibleColor = new Color(itemIconImageDisplay.color.r, itemIconImageDisplay.color.b, itemIconImageDisplay.color.g, 0);
        itemIconImageDisplay.color = invisibleColor;
        itemIconImageDisplay.raycastTarget = false;
    }

    public void ShowItemInSlot(InventoryItem item)
    {
        // background
        slotBackgroundImageDisplay.sprite = toolBarSlotSharedParameters.activeSlotImage;

        // icon
        Color visibleColor = new Color(itemIconImageDisplay.color.r, itemIconImageDisplay.color.b, itemIconImageDisplay.color.g, 1);
        itemIconImageDisplay.sprite = item.itemData.icon;
        itemIconImageDisplay.color = visibleColor;
        itemIconImageDisplay.raycastTarget = true;
    }
}
