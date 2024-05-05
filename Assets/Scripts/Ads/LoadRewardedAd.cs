using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;

public class LoadRewardedAd : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{
    [SerializeField] string androidAdUnitId;
    [SerializeField] string iosAdUnitId;
    private string adUnitId = null;

    Coins coins;
    GamePause gamePause;
    MusicController musicController;
    PurchaseManager purchaseManager;

    private string reward;

    void Awake()
    {   
        // Get the Ad Unit ID for the current platform:
#if UNITY_IOS
        adUnitId = iosAdUnitId;
#elif UNITY_ANDROID
        adUnitId = androidAdUnitId;
#endif
    }

    private void Start() {
        gamePause = GamePause._GPInstance;
        musicController = MusicController._MusicInstance;
        purchaseManager = PurchaseManager._PurchaseInstance;
    }
 
    // Call this public method when you want to get an ad ready to show.
    public void LoadAd(string name)
    {
        if(!purchaseManager.isRemoveAdsPurchased)
        {
            reward = name;
            // IMPORTANT! Only load content AFTER initialization (in this example, initialization is handled in a different script).
            Debug.Log("Loading Ad: " + adUnitId);
            Advertisement.Load(adUnitId, this);
            
            gamePause.Pause();
            musicController.ToggleSound();
        }  
    }
 
    // If the ad successfully loads, add a listener to the button and enable it:
    public void OnUnityAdsAdLoaded(string _adUnitId)
    {
        Debug.Log("Ad Loaded: " + _adUnitId);
        ShowAd();
    }
 
    // Implement a method to execute when the user clicks the button:
    public void ShowAd()
    {
        // Then show the ad:
        Advertisement.Show(adUnitId, this);
    }
 
    // Implement the Show Listener's OnUnityAdsShowComplete callback method to determine if the user gets a reward:
    public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
    {
        if (adUnitId.Equals(adUnitId) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
        {
            Debug.Log("Unity Ads Rewarded Ad Completed");

            // Grant a reward.
            if(reward == "coins")
            {
                coins = Coins._CoinsInstance;
                coins.IncreaseNumberOfCoins(15);
            }
            if(reward == "death")
            {
                PlayerStats playerStats = PlayerStats._PSInstance;
                playerStats.Revival();
                gamePause.Resume();
            }
            
            musicController.ToggleSound();
        }
    }
 
    // Implement Load and Show Listener error callbacks:
    public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
    {
        Debug.Log($"Error loading Ad Unit {adUnitId}: {error.ToString()} - {message}");
        // Use the error details to determine whether to try to load another ad.
    }
 
    public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
    {
        Debug.Log($"Error showing Ad Unit {adUnitId}: {error.ToString()} - {message}");
        // Use the error details to determine whether to try to load another ad.
    }
 
    public void OnUnityAdsShowStart(string adUnitId) { }
    public void OnUnityAdsShowClick(string adUnitId) { }
 
    void OnDestroy() { }
}
