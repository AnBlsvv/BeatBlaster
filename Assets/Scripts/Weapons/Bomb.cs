using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public ParticleSystem bombParticles;
    [SerializeField] private AudioSource explodeSound;
    BombController bombController;

    private void Start() {
        bombParticles.Stop();
        bombController = BombController._BombInstance;
    }

    public void ExplodeBomb()
    {     
        StartCoroutine(Explode(2f));
    }

    private IEnumerator Explode(float delay)
    {
        yield return new WaitForSeconds(delay);
        bombParticles.Play();
        explodeSound.Play();
        this.gameObject.transform.GetChild(0).gameObject.SetActive(false);

        Collider[] colliders = Physics.OverlapSphere(transform.position, bombController.explosionRadius);

        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.CompareTag("Enemy"))
            {
                EnemyStats enemyStats = collider.gameObject.GetComponent<EnemyStats>();
                if (enemyStats != null)
                {
                    enemyStats.TakeDamage(bombController.damage);
                }
            }
        }

        yield return new WaitForSeconds(2f);
        bombParticles.Stop();
        Destroy(gameObject);
    }
}
