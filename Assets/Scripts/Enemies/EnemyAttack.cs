using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public GameObject parent;

    public void EnemyAttackEvent()
    {
        EnemyController enemy = parent.GetComponent<EnemyController>();
        enemy.AttackEvent();
    }
}
