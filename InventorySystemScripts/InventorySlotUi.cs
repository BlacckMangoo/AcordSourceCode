using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class InventorySlotUi : MonoBehaviour
{
    public Image iconImage;
    public TMPro.TextMeshProUGUI quantityText;

    public void UpdateSlot(InventorySlot slot)
    {
        if (slot.item != null)
        {
            iconImage.sprite = slot.item.icon;
            iconImage.enabled = true;
            quantityText.text = slot.item.isStackable ? slot.quantity.ToString() : "";
            quantityText.enabled = slot.item.isStackable;
        }
        else
        {
            iconImage.sprite = null;
            iconImage.enabled = false;
            quantityText.enabled = false;
        }
    }
}
