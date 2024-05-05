using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(menuName = "ScriptableObjects/Item")]
public class Item : ScriptableObject
{
    public string itemName;
    public LocalizedString nameKey;
    public GameObject prefab;
    public Sprite icon;
    [TextArea(5,5)]
    public string description;
    public LocalizedString descriptionKey;
    public int count;
    public int costAmount;

    public UpgradeType upgradeType;
    public enum UpgradeType
    {
        none,
        DamageIncrease,
        ProtectionIncrease,
        PotionCountIncrease,
        PotionRestoreValueIncrease,
        HealthMaxIncrease,
        BombCountIncrease,
        BombDamageIncrease,
        BombExplosionRadius
    }

    public WeaponType weaponType;
    public enum WeaponType
    {
        none,
        Melee,
        DistantBattle,
        Heavy
    }

    public Item[] upgradeItems;
}
