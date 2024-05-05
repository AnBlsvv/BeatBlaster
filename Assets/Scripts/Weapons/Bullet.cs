using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;
    private Vector3 direction;
    public int damage;

    private void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    private void Start() {
        StartCoroutine(DestroyBullet(3f));
    }

    public void SetDirection(Vector3 dir)
    {
        direction = dir;
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject other = collision.gameObject;
        EnemyStats enemyStats = other.GetComponent<EnemyStats>();
        if (enemyStats != null)
        {
            enemyStats.TakeDamage(damage);
        }
        StartCoroutine(DestroyBullet(0f));
    }

    private IEnumerator DestroyBullet(float delay)
    {
        yield return new WaitForSeconds(delay);
        TrailRenderer trailRenderer = GetComponent<TrailRenderer>();
        if (trailRenderer != null)
        {
            trailRenderer.enabled = false;
        }
        Destroy(gameObject);
    }
}
