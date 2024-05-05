using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats
{
    EnemyController enemyController;
    public GameObject[] dropdownObjects;

    private void Start() {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        healthBar.SetHealth(currentHealth);
        enemyController = EnemyController._ECInstance;
    }
    
    public override void Die()
    {
        base.Die();
        enemyController.DieAnimation();
    }

    public void DestroyEnemy()
    {
        if(dropdownObjects.Length > 1)
        {
            float randomValue = Random.value; // returns random value from 0 to 1
            if (randomValue <= 0.8f) // 80% chance for coin
            {
                Instantiate(dropdownObjects[0], transform.position + new Vector3(0f, 1f, 0f), Quaternion.identity);
            }
            else // 20% chance for heart
            {
                Instantiate(dropdownObjects[1], transform.position + new Vector3(0f, 2.5f, 0f), Quaternion.identity);
            }
        }
        else if(dropdownObjects.Length == 1)
        {
            Instantiate(dropdownObjects[0], transform.position + new Vector3(0f, 2.5f, 0f), Quaternion.identity);
        }
        Destroy(gameObject);
    }
}
