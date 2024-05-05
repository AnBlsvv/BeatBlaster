using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Purchasing;

using TMPro;

[System.Serializable]
public class NonConsumablePurchase
{
    public string Id = "remove_ads";
    public TMP_Text removeAdsCostTxt;
}

[System.Serializable]
public class ConsumablePurchase
{
    public string ProductOneId = "purchase_coins_150";
    public TMP_Text purchaseCoinsCostTxt150;
}

public class PurchaseManager : MonoBehaviour, IStoreListener
{
    public static PurchaseManager _PurchaseInstance;

    public NonConsumablePurchase ncPurchase;
    public ConsumablePurchase cPurchase;

    private IStoreController storeController;
    private IExtensionProvider storeExtensionProvider;

    public bool isRemoveAdsPurchased = false;

    Coins coins;
    public GameObject noAdsItem;

    private void Awake() 
    {
        _PurchaseInstance = this;
    }

    private void Start()
    {
        InitializePurchasing();
        coins = Coins._CoinsInstance;
        noAdsItem.SetActive(true);
    }

    void InitializePurchasing()
    {
        if (IsInitialized())
        {
            return;
        }

        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
        builder.AddProduct(ncPurchase.Id, ProductType.NonConsumable);
        builder.AddProduct(cPurchase.ProductOneId, ProductType.Consumable);
        UnityPurchasing.Initialize(this, builder);
    }

    private bool IsInitialized()
    {
        return storeController != null && storeExtensionProvider != null;
    }

    public void BuyNoAds()
    {
        if (IsInitialized())
        {
            var product = storeController.products.WithID(ncPurchase.Id);

            if (product != null && product.availableToPurchase)
            {
                storeController.InitiatePurchase(product);
            }
            else
            {
                Debug.LogError("No Ads product is not available for purchase.");
            }
        }
    }

    public void BuyCoins()
    {
        if (IsInitialized())
        {
            var product = storeController.products.WithID(cPurchase.ProductOneId);

            if (product != null && product.availableToPurchase)
            {
                storeController.InitiatePurchase(product);
            }
            else
            {
                Debug.LogError("No product is not available for purchase.");
            }
        }
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.LogError($"Unity Purchasing initialization failed: {error}");
    }
 
    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        Debug.Log("Initialize Failed " + error + message);
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
    {
        var product = purchaseEvent.purchasedProduct;
        Debug.Log("Id of purchase" + product.definition.id);

        if (String.Equals(product.definition.id, ncPurchase.Id, StringComparison.Ordinal))
        {
            // remove ads
            GetComponent<LoadBannerAd>().HideBannerAd();
            noAdsItem.SetActive(false);
            isRemoveAdsPurchased = true;
        }
        if (String.Equals(product.definition.id, cPurchase.ProductOneId, StringComparison.Ordinal))
        {
            coins.IncreaseNumberOfCoins(150);
        }

        return PurchaseProcessingResult.Complete;
    }
 
    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.LogError($"Purchase failed for product {product.definition.id}. Reason: {failureReason}");
    }
 
    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        Debug.Log("Success");
        storeController = controller;
        storeExtensionProvider = extensions;
        ncPurchase.removeAdsCostTxt.text = storeController.products.WithID(ncPurchase.Id).metadata.localizedPriceString;
        cPurchase.purchaseCoinsCostTxt150.text = storeController.products.WithID(cPurchase.ProductOneId).metadata.localizedPriceString;
        CheckNonConsumable(ncPurchase.Id);
    }

    void CheckNonConsumable(string id)
    {
        if(storeController != null)
        {
            var product = storeController.products.WithID(id);
            if(product != null)
            {
                if(product.hasReceipt)
                {
                    //remove ads
                    isRemoveAdsPurchased = true;
                    noAdsItem.SetActive(false);
                }
                else
                {
                    // show ads
                    isRemoveAdsPurchased = false;
                    noAdsItem.SetActive(true);
                }
            }
        }
    }
}