using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    public float damageRadius;
    public int damageAmount;
    public float speed;

    public ParticleSystem missileParticlesExploding;
    public GameObject missileParticlesTrail;
    [SerializeField] private AudioSource explodeSound;

    EnemySpawner enemySpawner;
    MissileSpawner missileSpawner;
    PlayerManager playerManager;

    private bool isExploded = false;
    private Vector3 destinationPosition;

    public GameObject pointDecale;

    private void Start() {
        missileParticlesExploding.Stop();
        enemySpawner = EnemySpawner._ESInstance;
        missileSpawner = MissileSpawner._MSInstance;
        playerManager = PlayerManager._PMInstance;

        Transform destinationTransform = playerManager.player.transform;
        destinationPosition = destinationTransform.position;
        pointDecale = Instantiate(pointDecale, destinationPosition + new Vector3(0f, 0.05f, 0f), Quaternion.Euler(90f, 0f, 0f));
    }

    private void Update() {
        FaceTarget();
    }

    private void FaceTarget()
    {
        Vector3 direction = destinationPosition - transform.position;
        transform.position += direction * speed * Time.deltaTime;
        transform.up = Vector3.Slerp(transform.up, direction, Time.deltaTime * 5f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.name != "Missile Boss(Clone)")
        {
            if(!isExploded)
            {
                isExploded = true;
                explodeSound.Play();
                missileParticlesExploding.Play();
                missileParticlesTrail.SetActive(false);
                
                transform.GetChild(0).gameObject.SetActive(false);

                Collider[] colliders = Physics.OverlapSphere(transform.position, damageRadius);
                foreach (Collider collider in colliders)
                {
                    if (collider.gameObject.CompareTag("Player"))
                    {
                        PlayerStats playerStats = collider.gameObject.GetComponent<PlayerStats>();
                        if (playerStats != null)
                        {
                            playerStats.TakeDamage(damageAmount);
                        }
                    }
                }
                StartCoroutine(Explode(0.8f));
            }
        } 
    }

    private IEnumerator Explode(float delay)
    {
        yield return new WaitForSeconds(delay);
        missileParticlesExploding.Stop();
        Destroy(pointDecale);
        Destroy(gameObject);
    }
}
