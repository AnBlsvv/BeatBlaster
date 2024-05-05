using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinsPrefabs : MonoBehaviour
{
    Coins coins;
    public int coinsAmount;
    [SerializeField] private AudioSource pickUp;

    private void Start() {
        coins = Coins._CoinsInstance;
        StartCoroutine(DestroyCoin(10f));
    }

    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player"))
        {
            pickUp.Play();
            coins.IncreaseNumberOfCoins(coinsAmount);
            StartCoroutine(DestroyCoin(0.2f));
        }
    }

    private IEnumerator DestroyCoin(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}
