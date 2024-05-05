using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Coins : MonoBehaviour
{
    public static Coins _CoinsInstance;
    private const string saveKey = "mainSave";
    public int numberOfCoins;
    public TMP_Text coinsAmountText;

    [SerializeField] private AudioSource purchaseSound;

    void Awake()
    {
        if(_CoinsInstance != null && _CoinsInstance != this)
        {
            Destroy(this);
        }
        else
        {
            _CoinsInstance = this;
        }
    }

    void Start()
    {
        Load();
    }

    public void IncreaseNumberOfCoins(int count)
    {
        numberOfCoins += count;
        coinsAmountText.text = numberOfCoins.ToString();
    }

    public void DecreaseNumberOfCoins(int count)
    {
        purchaseSound.Play();
        numberOfCoins -= count;
        coinsAmountText.text = numberOfCoins.ToString();
    }

    private void Load()
    {
        var data = SaveManager.Load<SaveData.SavePlayerData>(saveKey);
        numberOfCoins = data.coinsAmount;
        coinsAmountText.text = numberOfCoins.ToString();
    }

    public void Save()
    {
        SaveManager.Save(saveKey, GetSaveSnapshot());
    }

    private SaveData.SavePlayerData GetSaveSnapshot()
    {
        var data = new SaveData.SavePlayerData()
        {
            coinsAmount = numberOfCoins
        };

        return data;
    }
}
