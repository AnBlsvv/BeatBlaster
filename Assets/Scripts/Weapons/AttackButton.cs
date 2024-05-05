using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AttackButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public static AttackButton _ABInstance;

    PlayerController playerController; 
    ItemController itemController;
    DistantBattleWeapon gun;
    GameObject currentItem;
    [HideInInspector] public bool isPressed;
    [HideInInspector] public bool isHolding;
    private float currentHoldTime;
    private bool isGunShoot = false;
    private float fireInterval;
    private float delayFire;


    [SerializeField] private AudioSource gunshotSound;

    void Awake()
    {
        if(_ABInstance != null && _ABInstance != this)
        {
            Destroy(this);
        }
        else
        {
            _ABInstance = this;
        }
    }

    private void Start() {
        playerController = PlayerController._PCInstance;
        itemController = ItemController._ICInstance;
        fireInterval = 0f;
        delayFire = 0f;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(playerController.canAttack && delayFire <= 0)
        {
            currentItem = itemController.currentWeapon;
            isPressed = true;
            isHolding = false;
            currentHoldTime = 0f;

            if (currentItem != null)
            {
                if(currentItem.CompareTag("MeleeWeapon"))
                {
                    playerController.AttackMeleeTrue();
                    fireInterval = 0.2f;
                }
                else if(currentItem.CompareTag("DistantWeapon"))
                {
                    gun = currentItem.GetComponent<DistantBattleWeapon>();
                    fireInterval = gun.fireInterval;
                    if(gun.currentAmmoCount > 0)
                    {
                        playerController.AttackDistantTrue();
                        gunshotSound.Play();
                        gun.Shoot();
                        isGunShoot = true;
                    }
                    if(gun.currentAmmoCount <= 0)
                    {
                        playerController.AttackDistantFalse();
                        playerController.ReloadGun();
                        gun.StartAddingAmmo();
                    }
                }
                else if(currentItem.CompareTag("Heavy"))
                {
                    gun = currentItem.GetComponent<DistantBattleWeapon>();
                    fireInterval = gun.fireInterval;
                    if(gun.currentAmmoCount > 0)
                    {
                        gunshotSound.Play();
                        gun.Shoot();
                        isGunShoot = true;
                    }
                    if(gun.currentAmmoCount <= 0)
                    {
                        gun.StartAddingAmmo();
                    }
                }
                delayFire = fireInterval;
                StartCoroutine(CountdownFireInterval());
            }
        }
    }

    private IEnumerator CountdownFireInterval()
    {
        while (delayFire > 0)
        {
            delayFire -= Time.deltaTime;
            yield return null;
        }
        yield break;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPressed = false;

        playerController.AttackDistantFalse();
        playerController.AttackMeleeFalse();

        if (isHolding)
        {
            // logic after holding
        }

        isGunShoot = false;
        CancelInvoke("DelayedShoot");
    }

    private void Update()
    {
        if (isPressed)
        {
            currentHoldTime += Time.deltaTime;
            if (currentHoldTime >= fireInterval && !isHolding)
            {
                isHolding = true;
                // logic while holding
                playerController.AttackMeleeFalse();
                
                if(isGunShoot && playerController.canAttack && gun.currentAmmoCount > 0 && !currentItem.CompareTag("Heavy"))
                {
                    InvokeRepeating("DelayedShoot", 0f, fireInterval);
                }
            }
            if(isHolding && gun != null && gun.currentAmmoCount <= 0 && !currentItem.CompareTag("Heavy") && playerController.canAttack)
            {
                CancelInvoke("DelayedShoot");
                playerController.AttackDistantFalse();
                playerController.ReloadGun();
                gun.StartAddingAmmo();
            }
        }
    }

    private void DelayedShoot()
    {
        gunshotSound.Play();
        gun.Shoot();
    }
}
