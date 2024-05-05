using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSlots : MonoBehaviour
{
    public Image icon;
    public Item item;

    ItemController itemController;

    void Start()
    {
        itemController = ItemController._ICInstance;
    }

    public void AddItem(Item newItem)
    {
        item = newItem;
        icon.sprite = item.icon;
        icon.enabled = true;
    }

    public void ClickOnItem()
    {
        if(item != null)
        {
            itemController.UpdateWeapon(item);
        }
    }

    public void ClearSlot()
    {
        icon.sprite = null;
        // clear setting for empty cell
    }
}
