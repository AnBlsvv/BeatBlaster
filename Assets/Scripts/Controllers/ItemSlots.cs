using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using UnityEngine.Localization.Components;

public class ItemSlots : MonoBehaviour
{
    public Image icon;
    public TMP_Text itemName;
    public TMP_Text description;
    [SerializeField] private LocalizeStringEvent nameLocalizeString;
    [SerializeField] private LocalizeStringEvent descriptionLocalizeString;
    public TMP_Text cost;
    Item item;
    Coins coins;

    ItemController itemController;

    void Start()
    {
        itemController = ItemController._ICInstance;
        coins = Coins._CoinsInstance;
    }

    public void AddItem(Item newItem)
    {
        item = newItem;
        icon.sprite = item.icon;
        icon.enabled = true;
       // itemName.text = newItem.itemName;
        nameLocalizeString.StringReference = newItem.nameKey;
        descriptionLocalizeString.StringReference = newItem.descriptionKey;
       // description.text = newItem.description;
        cost.text = newItem.costAmount.ToString();
    }

    public void ClickOnItem()
    {
        if(item != null && coins.numberOfCoins >= item.costAmount)
        {
            itemController.BuyItem(item);
            coins.DecreaseNumberOfCoins(item.costAmount);
            Destroy(gameObject);
        } 
    }
}
