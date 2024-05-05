using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class LoadInterstitialAd : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{
    [SerializeField] string androidAdUnitId;
    [SerializeField] string iosAdUnitId;
    private string adUnitId;

    GamePause gamePause;
    MusicController musicController;
    PurchaseManager purchaseManager;
 
    void Awake()
    {
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
 
    // Load content to the Ad Unit:
    public void LoadAd()
    {
        if(!purchaseManager.isRemoveAdsPurchased)
        {
            // IMPORTANT! Only load content AFTER initialization (in this example, initialization is handled in a different script).
            Debug.Log("Loading Ad: " + adUnitId);
            Advertisement.Load(adUnitId, this);
            gamePause.Pause();
            musicController.ToggleSound();
        }
    }
 
    // Show the loaded content in the Ad Unit:
    public void ShowAd()
    {
        // Note that if the ad content wasn't previously loaded, this method will fail
        Debug.Log("Showing Ad: " + adUnitId);
        Advertisement.Show(adUnitId, this);
    }
 
    // Implement Load Listener and Show Listener interface methods: 
    public void OnUnityAdsAdLoaded(string _adUnitId)
    {
        // Optionally execute code if the Ad Unit successfully loads content.
        ShowAd();
    }
 
    public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
    {
        Debug.Log($"Error loading Ad Unit: {adUnitId} - {error.ToString()} - {message}");
        // Optionally execute code if the Ad Unit fails to load, such as attempting to try again.
    }
 
    public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
    {
        Debug.Log($"Error showing Ad Unit {adUnitId}: {error.ToString()} - {message}");
        // Optionally execute code if the Ad Unit fails to show, such as loading another ad.
    }
 
    public void OnUnityAdsShowStart(string adUnitId) { }
    public void OnUnityAdsShowClick(string adUnitId) { }
    public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
    {
        gamePause.Resume();
        musicController.ToggleSound();
    }
}
