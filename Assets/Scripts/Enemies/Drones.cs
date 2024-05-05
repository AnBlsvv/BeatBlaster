using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drones : MonoBehaviour
{
    [Header("Movements Animation")]
    public Transform[] waypoints;
    public float moveSpeed;

    private int currentWaypointIndex = 0;
    private bool isMovingForward = true;
    [HideInInspector] public bool isDroneStarts = false;

    [Header("Lazer Components")]
    [SerializeField] private LineRenderer lazer;
    [SerializeField] private Transform muzzlePoint;
    [SerializeField] private float maxLength;
    public int damage;
    private bool isLaserActive;
    private bool isLazerHitPlayer = false;

    PlayerController playerController;

    private void Start()
    {
        DeactivateLazer();
        playerController = PlayerController._PCInstance;
    }

    private void FixedUpdate() {
        if(isDroneStarts)
        {
            MoveDrone();
            LazerHit();
        } 
    }

    private void LazerHit()
    {
        if(!isLaserActive) return;

        lazer.SetPosition(0, muzzlePoint.position);
        Ray ray = new Ray(muzzlePoint.position, muzzlePoint.forward);
        RaycastHit hit;
        bool cast = Physics.Raycast(ray, out hit, maxLength);
        if (cast)
        {
            if (hit.collider.CompareTag("Player") && !isLazerHitPlayer && !playerController.isRolling)
            {
                hit.collider.GetComponent<PlayerStats>().TakeDamage(damage);
                isLazerHitPlayer = true;
            }
            lazer.SetPosition(1, hit.point);
        }
        else
        {
            isLazerHitPlayer = false;
            lazer.SetPosition(1, muzzlePoint.position + muzzlePoint.forward * maxLength);
        }
    }

    private void MoveDrone()
    {
        if (waypoints.Length == 0)
            return;

        Vector3 direction = waypoints[currentWaypointIndex].position - transform.position;
        direction.Normalize();

        transform.position += direction * moveSpeed * Time.deltaTime;

        if (Vector3.Distance(transform.position, waypoints[currentWaypointIndex].position) < 0.1f)
        {
            if (isMovingForward)
            {
                currentWaypointIndex++;
                if(currentWaypointIndex == 2) ActivateLazer();
                if (currentWaypointIndex >= waypoints.Length)
                {
                    isMovingForward = false;
                    currentWaypointIndex = waypoints.Length - 2;
                }
            }
            else
            {
                currentWaypointIndex--;
                if(currentWaypointIndex == 0) DeactivateLazer();
                if (currentWaypointIndex < 0)
                {
                    isMovingForward = true;
                    currentWaypointIndex = 1;
                }
            }
        }
    }

    public void ResetPosition()
    {
        DeactivateLazer();
        transform.position = waypoints[0].position;
        currentWaypointIndex = 0;
    }

    private void ActivateLazer()
    {
        isLaserActive = true;
        lazer.enabled = true;
    }

    private void DeactivateLazer()
    {
        isLaserActive = false;
        lazer.enabled = false;
    }
}
