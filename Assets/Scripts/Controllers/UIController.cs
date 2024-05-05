using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using DG.Tweening;

using Unity.Services.Analytics;

public class UIController : MonoBehaviour
{
    public static UIController _UICInstance;

    PlayerController playerController;
    PlayerManager playerManager;
    CharacterStats characterStats;
    EnemySpawner enemySpawn;
    GamePause gamePause;
    public LoadInterstitialAd interstitialAd;

    [Header("Death Widow Component")]
    public GameObject deathWindow;
    public TMP_Text healthBarAmountText;
    public TMP_Text wavesPassedText;
    public TMP_Text timeSpentInDeathWindow;

    [Header("InGame Timer Component")]
    public TMP_Text timeSpentMainText;
    private float timeSpentMain = 0f;

    [Header("Wave Counter Text Component")]
    public float startYPosition;
    public float middleYPosition;
    public float endYPosition;
    public float delay;
    public float duration;
    public Transform waveInformation;
    public TMP_Text waveInformationText;

    [Header("Settings")]
    public GameObject settingsWidow;

    [Header("Windows Animation")]
    public float fadeTime;

    public GameObject[] uiGameObjects;
    
    void Awake()
    {
        if(_UICInstance != null && _UICInstance != this)
        {
            Destroy(this);
        }
        else
        {
            _UICInstance = this;
        }
    }

    void Start()
    {
        playerManager = PlayerManager._PMInstance;
        enemySpawn = EnemySpawner._ESInstance;
        gamePause = GamePause._GPInstance;
        interstitialAd = interstitialAd.GetComponent<LoadInterstitialAd>();
        playerController = playerManager.player.GetComponent<PlayerController>();
        deathWindow.SetActive(false);
        settingsWidow.SetActive(false);
        characterStats = playerManager.player.GetComponent<CharacterStats>();
        AnimationWaveCounterTextStart();
    }

    void Update()
    {
        healthBarAmountText.text = characterStats.currentHealth + " / " + characterStats.maxHealth;
        
        timeSpentMain += Time.deltaTime;
        int hours = (int)(timeSpentMain / 3600f);
        int minutes = (int)((timeSpentMain % 3600f) / 60f);
        int seconds = (int)(timeSpentMain % 60f);
        timeSpentMainText.text = string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);
    }

    public void DeathInformation()
    {
        gamePause.Pause();
        deathWindow.SetActive(true);
        FadeInObject(deathWindow);
        timeSpentInDeathWindow.text = /*"Time Spent: " +*/ timeSpentMainText.text;
        wavesPassedText.text = /*"Waves Passed: " +*/ (enemySpawn.waveCount - 1).ToString();

        Dictionary<string, object> parameters = new Dictionary<string, object>()
        {
            { "waveCount", enemySpawn.waveCount - 1},
            { "timeSpent", timeSpentMain}
        };
        
        AnalyticsService.Instance.CustomData("DeathInfo", parameters);
    }

    public void RollingButtonPressed()
    {
        playerController.CanRoll();
    }

    public void GoToMenu()
    {
        DOTween.KillAll();
        SceneManager.LoadScene(0);
    }

    public void RetryGame()
    {
        DOTween.KillAll();
        SceneManager.LoadScene(1);
    }

    public void FadeInObject(GameObject obj)
    {
        obj.transform.localScale = new Vector3(0.5f,0.5f,0.5f);
        gamePause.Pause();
        obj.SetActive(true);
        obj.transform.DOScale(new Vector3(1f, 1f, 1f), fadeTime).SetEase(Ease.OutBounce).SetUpdate(true);
    }

    public void FadeOutObject(GameObject obj)
    {
        obj.transform.DOScale(new Vector3(0f, 0f, 0f), fadeTime / 2).SetEase(Ease.Linear).SetUpdate(true).OnComplete(() => {
            obj.SetActive(false); gamePause.Resume();
            if(obj.name == "UpgradeItemWindow" && (enemySpawn.waveCount - 1) % 3 == 0)
            {
                interstitialAd.LoadAd();
            }
        });
    }

    public void AnimationWaveCounterTextStart()
    {
        waveInformationText.text = "Wave " + enemySpawn.waveCount;
        waveInformation.localPosition = new Vector3(waveInformation.transform.localPosition.x, startYPosition, waveInformation.transform.localPosition.z);
        waveInformation.DOLocalMoveY(middleYPosition, duration).SetEase(Ease.OutCubic).OnComplete(AnimationWaveCounterTextMiddle);
    }

    private void AnimationWaveCounterTextMiddle()
    {
        waveInformation.DOLocalMoveY(middleYPosition, 0f).SetDelay(delay).OnComplete(AnimationWaveCounterTextEnd);
    }

    private void AnimationWaveCounterTextEnd()
    {
        DOVirtual.DelayedCall(delay, () =>
        {
            waveInformation.DOLocalMoveY(endYPosition, duration).SetEase(Ease.InCubic);
        });
    }
}
