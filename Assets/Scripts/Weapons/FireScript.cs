using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireScript : MonoBehaviour
{
    DistantBattleWeapon gun;
    ItemController itemController;
    [SerializeField] private PlayerController playerController;
    GameObject currentItem;

    private float fireInterval;
    private float delayFire;

    [SerializeField] private AudioSource gunshotSound;

    private void Start() {
        //playerController = PlayerController._PCInstance;
        itemController = ItemController._ICInstance;
        fireInterval = 0f;
        delayFire = 0f;
    }

    public void Fire()
    {
        if(playerController.canAttack && delayFire <= 0)
        {
            currentItem = itemController.currentWeapon;

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
                    }
                    if(gun.currentAmmoCount <= 0)
                    {
                        playerController.AttackDistantFalse();
                        playerController.ReloadGun();
                        gun.StartAddingAmmo();
                        CancelInvoke("DelayedShoot");
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

    public void StopFire()
    {
        playerController.AttackDistantFalse();
        playerController.AttackMeleeFalse();

       // stop coroutine
    }

    private void DelayedShoot()
    {
        gunshotSound.Play();
        gun.Shoot();
    }
}
