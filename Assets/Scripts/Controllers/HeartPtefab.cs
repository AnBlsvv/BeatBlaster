using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartPtefab : MonoBehaviour
{
    PlayerStats playerStats;
    [SerializeField] private AudioSource pickUp;

    private void Start() {
        playerStats = PlayerStats._PSInstance;
        StartCoroutine(DestroyHeart(10f));
    }

    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player"))
        {
            pickUp.Play();
            /*if(playerStats.currentHealth < playerStats.maxHealth)
            {
                playerStats.currentHealth += 50;
                playerStats.healthBar.SetHealth(playerStats.currentHealth);
            }*/

            int healthTaken = playerStats.maxHealth - playerStats.currentHealth;
            if(healthTaken < 50)
            {
                playerStats.currentHealth += healthTaken;
            }
            else
            {
                playerStats.currentHealth += 50; 
            }
            playerStats.healthBar.SetHealth(playerStats.currentHealth);

            StartCoroutine(DestroyHeart(0.2f));
        }
    }

    private IEnumerator DestroyHeart(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}
