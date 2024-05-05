using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MissileSpawner : MonoBehaviour
{
    public static MissileSpawner _MSInstance;

    public GameObject missilePrefab;
    public Transform spawnPoint;
    public float spawnInterval;

    EnemySpawner enemySpawner;
    GamePause pause;
    private bool isStart;

    public Transform[] destinationPoints;

    private void Awake() {
        _MSInstance = this;
    }

    private void Start()
    {
        enemySpawner = EnemySpawner._ESInstance;
        pause = GamePause._GPInstance;
        isStart = false;
    }

    private void Update() {
        if(pause.gameIsPaused && isStart)
        {
            isStart = false;
        }
        else if (enemySpawner.waveCount >= 5 && enemySpawner.state == EnemySpawner.SpawnState.WAITING && !isStart)
        {
            spawnInterval = 5f;
            isStart = true;
            GameObject[] missiles = GameObject.FindGameObjectsWithTag("Missile");
            foreach (GameObject missile in missiles)
            {
                DOTween.Kill(missile.transform);
                Destroy(missile);
            }
            StartCoroutine(SpawnMissilesCoroutine());
        }
        else if ((enemySpawner.state == EnemySpawner.SpawnState.SPAWNING || enemySpawner.state == EnemySpawner.SpawnState.COUNTING) && isStart)
        {
            isStart = false;
            StopCoroutine(SpawnMissilesCoroutine());
        }
    }

    private IEnumerator SpawnMissilesCoroutine()
    {
        while (!pause.gameIsPaused)
        {
            Instantiate(missilePrefab, spawnPoint.position, Quaternion.identity);
            if(spawnInterval > 2f)
            {
                spawnInterval -= 0.1f;
            }
            yield return new WaitForSecondsRealtime(spawnInterval);
        }
    }
}
