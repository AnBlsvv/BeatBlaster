using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeController : MonoBehaviour
{
    ItemController itemController;

    private void Start() {
        itemController = ItemController._ICInstance;
    }

    public void AttackMelleEvent()
    {
        MeleeWeapon weapon = itemController.currentWeapon.GetComponent<MeleeWeapon>();
        if(weapon != null)
        {
            weapon.AttackEvent();
        }
    }
}
