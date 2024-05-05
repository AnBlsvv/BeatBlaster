using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : MonoBehaviour
{
    private AttackButton attackButton;
    PlayerStats playerStats;
    PlayerController playerController;

    public float radius = 7f;
    public Transform centerSphere;

    public int weaponDamage;

    private void Start() {
        attackButton = AttackButton._ABInstance;
        playerStats = PlayerStats._PSInstance;
        playerController = PlayerController._PCInstance;
    }

    public void AttackEvent()
    {
        foreach (Collider other in Physics.OverlapSphere(centerSphere.position, radius))
        {
            if (other.CompareTag("Enemy"))
            {
                EnemyStats targetStats = other.GetComponent<EnemyStats>();
                targetStats?.TakeDamage(weaponDamage);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(centerSphere.position, radius);
    }
}