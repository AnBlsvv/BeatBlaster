using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsedAmmo : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        StartCoroutine(DestroyBullet(3f));
    }

    private IEnumerator DestroyBullet(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}
