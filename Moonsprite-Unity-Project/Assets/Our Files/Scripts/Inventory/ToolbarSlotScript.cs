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
    [Space]
    [SerializeField] bool active = false;
    public bool Active 
    { 
        get
        {
            return active;
        }
        set
        {
            active = value;

            if(value == false)
            {
                HideSlot();
            }
            else
            {
                ShowSlot();
            }
        }
    }

    float backgroundDefaultAlpha = 1;
    float imageDisplayDefaultAlpha = 1;

    public void Start()
    {
        backgroundDefaultAlpha = slotBackgroundImageDisplay.color.a;
        imageDisplayDefaultAlpha = itemIconImageDisplay.color.a;

        HideSlot();
    }

    void HideSlot()
    {
        slotBackgroundImageDisplay.sprite = toolBarSlotSharedParameters.emptySlotImage;
    }
    void ShowSlot()
    {
        slotBackgroundImageDisplay.sprite = toolBarSlotSharedParameters.activeSlotImage;
    }

    public void ShowEmptySlot()
    {
        Color invisibleColor = new Color(itemIconImageDisplay.color.r, itemIconImageDisplay.color.b, itemIconImageDisplay.color.g, 0);
        itemIconImageDisplay.color = invisibleColor;
        HideSlot();
        itemIconImageDisplay.raycastTarget = false;
    }
    public void ShowItemInSlot(InventoryItem item)
    {
        if(Active == false)
        {
            Active = true;
        }
        ShowSlot();
        itemIconImageDisplay.sprite = item.itemData.icon;
        Color visibleColor = new Color(itemIconImageDisplay.color.r, itemIconImageDisplay.color.b, itemIconImageDisplay.color.g, imageDisplayDefaultAlpha);
        itemIconImageDisplay.color = visibleColor;
        itemIconImageDisplay.raycastTarget = true;
    }
}
