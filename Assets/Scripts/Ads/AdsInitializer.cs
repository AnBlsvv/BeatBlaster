using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
public class AdsInitializer : MonoBehaviour, IUnityAdsInitializationListener
{
    [SerializeField] string androidGameId;
    [SerializeField] string iosGameId;
    [SerializeField] bool isTestingMode = true;
    private string gameId;

    void Awake()
    {
        InitializeAds();
    }

    private void InitializeAds()
    {    
    #if UNITY_IOS
        gameId = iosGameId;
    #elif UNITY_ANDROID
        gameId = androidGameId;
    #elif UNITY_EDITOR
        gameId = androidGameId;
    #endif
        if (!Advertisement.isInitialized && Advertisement.isSupported)
        {
            Advertisement.Initialize(gameId, isTestingMode, this);
        }
    }

    public void OnInitializationComplete()
    {
        Debug.Log("Unity Ads initialization complete.");
    }
 
    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
    }
}
