using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerStats : CharacterStats
{
    public static PlayerStats _PSInstance;
    UIController uiController;
    [SerializeField] private PlayerController player;

    [Header("Stamina Parameners")]
    public StaminaBar staminaBar;
    public int maxStamina;
    public int currentStamina;

    [Header("Potion Parameters")]
    public int potionRestoreValue;
    public TMP_Text potionsAmountTxt;
    public int potionsAmount;

    [Header("Audio Source")]
    [SerializeField] private AudioSource healingSound;
    [SerializeField] private AudioSource deathSound;

    private void Awake() {
        if(_PSInstance != null && _PSInstance != this)
        {
            Destroy(this);
        }
        else
        {
            _PSInstance = this;
        }
    }

    private void Start()
    {
        uiController = UIController._UICInstance;
        //player = GetComponent<PlayerController>();
        UpdatePotionAmount();

        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        healthBar.SetHealth(currentHealth);

        currentStamina = maxStamina;
        staminaBar.SetMaxStamina(maxStamina);
        staminaBar.SetStamina(currentStamina);
    }

    public void StaminaReduction(int count)
    {
        currentStamina -= count;
        currentStamina = Mathf.Max(currentStamina, 0);
        staminaBar.SetStamina(currentStamina);
    }

    public void StaminaRecovery()
    {
        currentStamina++;
        currentStamina = (int)Mathf.Clamp(currentStamina, 0f, maxStamina);
        staminaBar.SetStamina(currentStamina);
    }
   
    public void HealthPotion()
    {
        if(potionsAmount > 0)
        {
            healingSound.Play();
            int healthTaken = maxHealth - currentHealth;
            if(healthTaken < potionRestoreValue)
            {
                currentHealth += healthTaken;
            }
            else
            {
                currentHealth += potionRestoreValue;  
            }
            healthBar.SetHealth(currentHealth);
            potionsAmount--;
            UpdatePotionAmount();
        }
    }

    public void AddPotion()
    {
        if(potionsAmount < 3)
        {
            potionsAmount++;
            UpdatePotionAmount();
        }
        else
            Debug.Log("You cant take one more potion");
    }

    public void UpdatePotionAmount()
    {
        potionsAmountTxt.text = potionsAmount.ToString();
    }

    public override void Die()
    {
        base.Die();
        deathSound.Play();
        player._animator.SetBool("isDie", true);
    }

    public void Revival()
    {
        player._animator.SetBool("isDie", false);
        uiController.FadeOutObject(uiController.deathWindow);
        currentHealth += 300;
        healthBar.SetHealth(currentHealth);
        isDie = false;
    }
}
