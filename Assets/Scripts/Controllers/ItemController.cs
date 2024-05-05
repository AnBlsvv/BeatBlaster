using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class ItemController : MonoBehaviour
{
    public static ItemController _ICInstance;

    GamePause gamePause;
    PlayerStats playerStats;
    PlayerController playerController;
    Coins coins;
    DistantBattleWeapon currentDistantBattleWeapon;
    BombController bombController;
    UIController uiController;
    ItemSlots[] itemSlots;
    PurchaseManager purchaseManager;

    public DronesController drones;

    [Header("Upgrade Window Parameters")]
    public List<Item> itemsList = new List<Item>();
    public GameObject upgradeItemWindow;
    public Transform itemsParent;

    [Header("Weapons Parameters")]
    public Transform weaponParent;
    public WeaponSlots[] weaponSlots;
    private List<Item> abilities = new List<Item>();

    [Header("Other Parameters")]
   // public Image attackButton;
    public TMP_Text ammoAmountText;
    public GameObject updatesItemsPrefab;
    private int counterUpgradeBombRadius = 0;
    private bool changeText = false;
    [HideInInspector] public GameObject currentWeapon;

    private void Awake()
    {
        if(_ICInstance != null && _ICInstance != this)
        {
            Destroy(this);
        }
        else
        {
            _ICInstance = this;
        }
    }

    private void Start()
    {
        itemsList = Resources.LoadAll<Item>("Items/").ToList();
        upgradeItemWindow.SetActive(false);

        gamePause = GamePause._GPInstance;
        coins = Coins._CoinsInstance;
        playerStats = PlayerStats._PSInstance;
        playerController = PlayerController._PCInstance;
        bombController = BombController._BombInstance;
        uiController = UIController._UICInstance;
        purchaseManager = PurchaseManager._PurchaseInstance;
        
        BuyItem(itemsList[0]);
        UpdateWeapon(abilities[0]);

        ammoAmountText.text = "0 / 0";
        
    }

    private void Update() 
    {
        if(changeText)
        {
            ammoAmountText.text = currentDistantBattleWeapon.currentAmmoCount + " / " + currentDistantBattleWeapon.maxAmmoCount;
        } 
    }

    public void ShowUpgrades()
    {
        purchaseManager.GetComponent<LoadBannerAd>().HideWhenUpgradeWindow(true);
        drones.ResetDronesPosition();
        playerController.DisableAllAnimations();

        for(int i = 0; i < itemsList.Count; i++)
        {
            if(itemsList[i].itemName != "Bomb Radius" && counterUpgradeBombRadius < 5)
            {
                GameObject updatePrefab = Instantiate(updatesItemsPrefab, itemsParent);
                updatePrefab.GetComponent<ItemSlots>().AddItem(itemsList[i]);
            }
        }
        uiController.FadeInObject(upgradeItemWindow);
    }

    public void AcceptInItemWindow()
    {
        coins.Save();
        purchaseManager.GetComponent<LoadBannerAd>().HideWhenUpgradeWindow(false);
        uiController.FadeOutObject(upgradeItemWindow);
        for(int i = 3; i < itemsParent.childCount; i++)
        {
            Destroy(itemsParent.GetChild(i).gameObject);
        }
        uiController.AnimationWaveCounterTextStart();
    }

    public void DecreaseCoins(int i)
    {
        coins.DecreaseNumberOfCoins(i);
    }

    public void BuyItem(Item upgradeItem)
    {
        if(upgradeItem.weaponType == Item.WeaponType.none)
        {
            switch (upgradeItem.upgradeType)
            {
                case Item.UpgradeType.none:
                    Debug.Log("Nothing to upgrade");
                    break;
                case Item.UpgradeType.DamageIncrease:
                    ApplyDamageIncreaseUpgrade(upgradeItem);
                    break;
                case Item.UpgradeType.PotionCountIncrease:
                    ApplyPotionCountIncrease(upgradeItem);
                    break;
                case Item.UpgradeType.PotionRestoreValueIncrease:
                    ApplyPotionRestoreValueIncrease(upgradeItem);
                    break;
                case Item.UpgradeType.HealthMaxIncrease:
                    ApplyHealthMaxIncrease(upgradeItem);
                    break;
                case Item.UpgradeType.BombCountIncrease:
                    ApplyBombCountIncrease(upgradeItem);
                    break;
                case Item.UpgradeType.BombDamageIncrease:
                    ApplyBombDamageIncrease(upgradeItem);
                    break;
                case Item.UpgradeType.BombExplosionRadius:
                    ApplyBombExplosionRadius(upgradeItem);
                    break;
                default:
                    Debug.Log("Unsupported upgrade type");
                    break;
            }
        }
        else if(upgradeItem.weaponType == Item.WeaponType.DistantBattle 
                || upgradeItem.weaponType == Item.WeaponType.Melee 
                || upgradeItem.weaponType == Item.WeaponType.Heavy)
        {
            AddWeapon(upgradeItem);
            itemsList.Remove(upgradeItem);
            foreach(var upgrade in upgradeItem.upgradeItems)
            {
                itemsList.Add(upgrade);
            }
        }
    }

    private void ApplyDamageIncreaseUpgrade(Item item)
    {
        AddWeapon(item);
    }

    private void ApplyPotionCountIncrease(Item item)
    {
        playerStats.potionsAmount += item.count;
        playerStats.UpdatePotionAmount();
    }

    private void ApplyPotionRestoreValueIncrease(Item item)
    {
        playerStats.potionRestoreValue = (int)Math.Ceiling((playerStats.potionRestoreValue * item.count) / 100.0);
    }

    private void ApplyHealthMaxIncrease(Item item)
    {
        int increasedMaxHealth = (int)Math.Ceiling((playerStats.maxHealth * item.count) / 100.0);
        playerStats.maxHealth += increasedMaxHealth;
        playerStats.healthBar.SetMaxHealth(playerStats.maxHealth);

        if (playerStats.currentHealth < playerStats.maxHealth)
        {
            int increasedCurrentHealth = (int)Math.Ceiling((playerStats.currentHealth * item.count) / 100.0);
            playerStats.currentHealth += increasedCurrentHealth;
            playerStats.healthBar.SetHealth(playerStats.currentHealth);
        }
    }

    private void ApplyBombCountIncrease(Item item)
    {
        bombController.bombAmount += item.count;
        bombController.bombAmountTxt.text = bombController.bombAmount.ToString();
    }

    private void ApplyBombDamageIncrease(Item item)
    {
        bombController.damage += (int)Math.Ceiling((bombController.damage * item.count) / 100.0);
    }

    private void ApplyBombExplosionRadius(Item item)
    {
        counterUpgradeBombRadius++;
        bombController.explosionRadius += item.count;
    }

    public void UpdateWeapon(Item item)
    {
        for (int i = 0; i < weaponParent.childCount; i++)
        {
            weaponParent.GetChild(i).gameObject.SetActive(false);
        }

        Item existingAbility = abilities.Find(ability => ability.itemName == item.itemName);
        if (existingAbility != null)
        {
            int index = abilities.IndexOf(existingAbility);
            GameObject weapon = weaponParent.GetChild(index).gameObject;
            weapon.SetActive(true);
            currentWeapon = weapon;
            //attackButton.sprite = item.icon;

            if(weapon.CompareTag("MeleeWeapon"))
            {
                changeText = false;
                ammoAmountText.text = "";
            }
            else if(weapon.CompareTag("DistantWeapon") || weapon.CompareTag("Heavy"))
            {
                currentDistantBattleWeapon = weapon.GetComponent<DistantBattleWeapon>();
                ammoAmountText.text = currentDistantBattleWeapon.currentAmmoCount + " / " + currentDistantBattleWeapon.maxAmmoCount;
                changeText = true;
            }

            if(weapon.CompareTag("Heavy"))
            {
                bombController.bombButton.interactable = false;
            }
            else
            {
                bombController.bombButton.interactable = true;
            }
        }
    }

    private void AddWeapon(Item newWeapon)
    {
        Item existingAbility = abilities.Find(ability => ability.itemName == newWeapon.itemName);

        if (existingAbility != null)
        {
            int index = abilities.IndexOf(existingAbility);
            GameObject weapon = weaponParent.GetChild(index).gameObject;
            if(weapon.CompareTag("Heavy") || weapon.CompareTag("DistantWeapon"))
            {
                weapon.GetComponent<DistantBattleWeapon>().weaponDamage += newWeapon.count;
            }
            else if(weapon.CompareTag("MeleeWeapon"))
            {
                weapon.GetComponent<MeleeWeapon>().weaponDamage += newWeapon.count;
            }
        }
        else
        {
            // add a new ability to the end of the list
            abilities.Add(newWeapon);
            Instantiate(newWeapon.prefab, weaponParent);
            newWeapon.prefab.SetActive(false);
        }
        UpdateWeaponsSlots();
    }

    private void UpdateWeaponsSlots()
    {
        for (int i = 0; i < weaponSlots.Length; i++)
        {
            if (i < abilities.Count)
            {
                // set ability to slot
                weaponSlots[i].AddItem(abilities[i]);
            }
            else
            {
                // clear the cell if there no similar ability
                weaponSlots[i].ClearSlot();
            }
        }
    }
}
