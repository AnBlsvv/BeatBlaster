using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CharacterStats : MonoBehaviour
{
    public static CharacterStats _CSInstance;

    public int maxHealth;
    public int currentHealth;
    public int attackDamage;
    public HealthBar healthBar;
    [SerializeField] private AudioSource enemyGetHitSound;
    [HideInInspector] public Coins coins;
    [HideInInspector] public bool isDie = false;

    void Awake()
    {
        _CSInstance = this;
    }

    public void TakeDamage(int damage)
    {
        if(!isDie)
        {
            damage = Mathf.Clamp(damage, 0, int.MaxValue);
            currentHealth -= damage;
            // if health < 0, Mathf.Max will always set health to 0
            currentHealth = Mathf.Max(currentHealth, 0);
            healthBar.SetHealth(currentHealth);
            if(currentHealth <= 0)
            {
                isDie = true;
                Die();
            }
            if(this.CompareTag("Enemy"))
            {
                enemyGetHitSound?.Play();
                Transform thisTransform = this.GetComponent<Transform>();
                if(thisTransform != null)
                {
                    float duration = 0.5f;
                    float strength = 0.5f;
                    var tween = thisTransform.DOShakePosition(duration, strength);
                    if(tween.IsPlaying())return;
                    thisTransform.DOShakeRotation(duration, strength);
                    thisTransform.DOShakeScale(duration, strength);
                }
            }
        }
    }
    public virtual void Die()
    {
        // different dies for characters
    }
}
