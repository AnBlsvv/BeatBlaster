using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistantBattleWeapon : MonoBehaviour
{
    public static DistantBattleWeapon _DBWInstance;
    PlayerController playerController;

    public GameObject bulletPrefab;
    public int weaponDamage;
    public int maxAmmoCount;
    public int currentAmmoCount;
    public float fireInterval;
    public GameObject usedAmmoPrefab;
    private float maxForceMagnitude = 10;

    private float addTime = 3f;

    private void Awake() {
        _DBWInstance = this;
    }

    private void Start() {
        playerController = PlayerController._PCInstance;
        currentAmmoCount = maxAmmoCount;
    }

    public void Shoot()
    {
        currentAmmoCount--;
        // Get the direction vector of the character's face
        Vector3 targetDirection = playerController.GetLookDirection();
        targetDirection.y = 0f;
        targetDirection.Normalize();

        // Find the poin at which we shoot (in front of character)
        Vector3 shootPoint = transform.position + targetDirection;

        // draw a line for debugging
        Debug.DrawLine(shootPoint, shootPoint + targetDirection * 10f, Color.red, 1f);

        // create ammo
        GameObject bulletObject = Instantiate(bulletPrefab, shootPoint, Quaternion.identity);
        // get component Bullet
        Bullet bullet = bulletObject.GetComponent<Bullet>();
        if (bullet != null)
        {
            bullet.SetDirection(targetDirection);
            bullet.damage = weaponDamage;
        }
        StartCoroutine(SpawnUsedAmmo(0.2f));
    }

    private IEnumerator SpawnUsedAmmo(float delay)
    {
        yield return new WaitForSeconds(delay);
        GameObject usedAmmo = Instantiate(usedAmmoPrefab, transform.position, Quaternion.identity);
        float forceMagnitude = Random.Range(3f, maxForceMagnitude);
        // add force
        Rigidbody usedAmmoRigidbody = usedAmmo.GetComponent<Rigidbody>();
        usedAmmoRigidbody.AddForce(transform.forward * forceMagnitude, ForceMode.Impulse);
    }

    public void StartAddingAmmo()
    {
        if (AddAmmoCoroutine() != null)
        {
            StopCoroutine(AddAmmoCoroutine());
        }
        StartCoroutine(AddAmmoCoroutine());
    }

    private IEnumerator AddAmmoCoroutine()
    {
        float elapsedTime = 0f;
        int startAmmo = currentAmmoCount;
        int targetAmmo = maxAmmoCount;

        while (elapsedTime < addTime)
        {
            float normalizedTime = elapsedTime / addTime;
            currentAmmoCount = Mathf.RoundToInt(Mathf.Lerp(startAmmo, targetAmmo, normalizedTime));
            yield return null;
            elapsedTime += Time.deltaTime;
        }
        currentAmmoCount = targetAmmo;
    }
}
