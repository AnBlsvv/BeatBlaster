using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventController : MonoBehaviour
{
    UIController uiController;
    ItemController itemController;
    BombController bombController;
    [SerializeField] private PlayerStats playerStats;

    [SerializeField] private AudioSource weaponSound;

    private void Start() 
    {
        uiController = UIController._UICInstance;
        itemController = ItemController._ICInstance;
        bombController = BombController._BombInstance;
    }

    public void PlayerDeathEvent()
    {
        uiController.DeathInformation();
    }

    public void AttackMelleEvent()
    {
        MeleeWeapon weapon = itemController.currentWeapon.GetComponent<MeleeWeapon>();
        weapon?.AttackEvent();
    }

    public void HitSoundPlay()
    {
        weaponSound.Play();
    }

    public void ThrowBombEvent()
    {
        bombController.ThrowBomb();
    }
}
