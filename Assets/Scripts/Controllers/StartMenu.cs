using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using TMPro;

public class StartMenu : MonoBehaviour
{
    private const string saveKey = "mainSave";
    public GameObject privacyPolicyWindow;
    public float fadeTime;
    public TMP_Text coinsAmountText;
    public RectTransform description01, description02;
    

    private void Start() 
    {
        privacyPolicyWindow.SetActive(false);
        if(!PlayerPrefs.HasKey("AcceptPrivacyPolicy"))
        {
            FadeInObject(privacyPolicyWindow);
        }

        Load();
    }

    private void Update() 
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            PlayerPrefs.DeleteKey("AcceptPrivacyPolicy");
            FadeInObject(privacyPolicyWindow);
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void Load()
    {
        var data = SaveManager.Load<SaveData.SavePlayerData>(saveKey);
        coinsAmountText.text = data.coinsAmount.ToString();
    }

    public void FadeInObject(GameObject window)
    {
        window.transform.localScale = new Vector3(0f, 0f, 0f);
        window.SetActive(true);
        window.transform.DOScale(new Vector3(1f, 1f, 1f), fadeTime).SetEase(Ease.OutBounce).SetUpdate(true);
    }

    public void AcceptPrivacyPolicy()
    {  
        PlayerPrefs.SetInt("AcceptPrivacyPolicy", 1);
        PlayerPrefs.Save();
        FadeOutObject(privacyPolicyWindow);
    }

    public void FadeOutObject(GameObject window)
    {
        window.transform.DOScale(new Vector3(0f, 0f, 0f), fadeTime / 2).SetEase(Ease.Linear).SetUpdate(true).OnComplete(() => {
            window.SetActive(false);
        });
    }

    public void RedirectToTerms()
    {
        Application.OpenURL("https://sites.google.com/view/beatblaster/terms?authuser=0");
    }

    public void RedirectToPrivacyPolicy()
    {
        Application.OpenURL("https://sites.google.com/view/beatblaster/privacy");
    }

    public void SlideTutorialRight()
    {
        description01.DOAnchorPos(new Vector2(-4000f, 0), 0.25f);
        description02.DOAnchorPos(Vector2.zero, 0.25f);
    }

    public void SlideTutorialLeft()
    {
        description02.DOAnchorPos(new Vector2(4000f, 0), 0.25f);
        description01.DOAnchorPos(Vector2.zero, 0.25f);
    }
}