using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDie : MonoBehaviour
{
    public GameObject parent;

    public void EnemyDieEvent()
    {
        EnemyStats enemy = parent.GetComponent<EnemyStats>();
        enemy.DestroyEnemy();
    }
}
