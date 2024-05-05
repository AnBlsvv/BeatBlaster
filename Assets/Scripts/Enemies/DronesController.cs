using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DronesController : MonoBehaviour
{
    EnemySpawner enemySpawner;
    private bool isStart;
    public GameObject[] drones;

    Drones drone01;
    Drones drone02;

    private void Start() {
        enemySpawner = EnemySpawner._ESInstance;
        isStart = false;
        drone01 = drones[0].GetComponent<Drones>();
        drone02 = drones[1].GetComponent<Drones>();
    }

    private void Update() {
        if(enemySpawner.waveCount >= 7 && enemySpawner.state == EnemySpawner.SpawnState.WAITING && !isStart)
        {
            isStart = true;
            drone01.isDroneStarts = true;
            drone02.isDroneStarts = true;
        }
    }

    public void ResetDronesPosition()
    {
        isStart = false;
        drone01.isDroneStarts = false;
        drone01.ResetPosition();
        drone02.isDroneStarts = false;
        drone02.ResetPosition();
    }
}
