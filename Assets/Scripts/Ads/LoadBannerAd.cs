using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class LoadBannerAd : MonoBehaviour
{
    [SerializeField] string androidAdUnitId;
    [SerializeField] string iosAdUnitId;
    private string adUnitId = null;
    BannerPosition bannerPosition = BannerPosition.BOTTOM_CENTER;

    PurchaseManager purchaseManager;

    private void Start() {
    #if UNITY_IOS
        adUnitId = iosAdUnitId;
    #elif UNITY_ANDROID
        adUnitId = androidAdUnitId;
    #endif

        // Set the banner position:
        Advertisement.Banner.SetPosition(bannerPosition);
        purchaseManager = PurchaseManager._PurchaseInstance;
        LoadBanner();
    }

    // Implement a method to call when the Load Banner button is clicked:
    public void LoadBanner()
    {
        // Set up options to notify the SDK of load events:
        BannerLoadOptions options = new BannerLoadOptions
        {
            loadCallback = OnBannerLoaded,
            errorCallback = OnBannerError
        };
 
        // Load the Ad Unit with banner content:
        Advertisement.Banner.Load(adUnitId, options);
    }
 
    // Implement code to execute when the loadCallback event triggers:
    void OnBannerLoaded()
    {
        if(!purchaseManager.isRemoveAdsPurchased)
        {
            Debug.Log("Banner loaded");
            ShowBannerAd();
        }
        else
        {
            HideBannerAd();
        }
    }
 
    // Implement code to execute when the load errorCallback event triggers:
    void OnBannerError(string message)
    {
        Debug.Log($"Banner Error: {message}");
        // Optionally execute additional code, such as attempting to load another ad.
    }
 
    // Implement a method to call when the Show Banner button is clicked:
    void ShowBannerAd()
    {
        // Set up options to notify the SDK of show events:
        BannerOptions options = new BannerOptions
        {
            clickCallback = OnBannerClicked,
            hideCallback = OnBannerHidden,
            showCallback = OnBannerShown
        };
 
        // Show the loaded Banner Ad Unit:
        Advertisement.Banner.Show(adUnitId, options);
    }
 
    // Implement a method to call when the Hide Banner button is clicked:
    public void HideBannerAd()
    {
        // Hide the banner:
        Advertisement.Banner.Hide();
    }

    public void HideWhenUpgradeWindow(bool isShowWindow)
    {
        if(!purchaseManager.isRemoveAdsPurchased)
        {
            if(isShowWindow)
            {
                HideBannerAd();
            }
            else
            {
                Debug.Log("Banner loaded");
                ShowBannerAd();
            }
        }
    }
 
    void OnBannerClicked() { }
    void OnBannerShown() { }
    void OnBannerHidden() { }
    void OnDestroy() { }
}