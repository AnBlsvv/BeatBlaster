using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;
using UnityEngine.UI;

using Unity.Services.Analytics;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner _ESInstance;

    public enum SpawnState { SPAWNING, WAITING, COUNTING};
    
    [System.Serializable]
    public struct EnemyTransformPair
    {
        public GameObject enemy;
        public Transform point;
    }

    public GameObject bossEnemy;

    [HideInInspector] public int waveCount = 1;
    private int countEnemies = 3;
    public List<EnemyTransformPair> enemyTransformPairs = new List<EnemyTransformPair>();

    public float timeBetweenWaves = 3f;
    private float waveCountdown;
    private float searchCountdown = 1f;
    [HideInInspector] public SpawnState state = SpawnState.COUNTING;

    [HideInInspector] public int healthEnemiesIncrease = 0;
    [HideInInspector] public int attackEnemiesDamage = 0;

    UIController uiController;
    ItemController itemController;

    //public bool isSpawningPaused = false;

    private void Awake() {
        if(_ESInstance != null && _ESInstance != this)
        {
            Destroy(this);
        }
        else
        {
            _ESInstance = this;
        }
    }

    private void Start()
    {
        waveCountdown = timeBetweenWaves;
        uiController = UIController._UICInstance;
        itemController = ItemController._ICInstance;
    }

    private void Update()
    {
        //if(!isSpawningPaused)
       // {
            if(state == SpawnState.WAITING)
            {
                if(!EnemyIsAlive())
                {
                    WaveCompleted();
                }
                else
                {
                    return;
                }
            }
            if(waveCountdown <= 0)
            {
                if(state != SpawnState.SPAWNING)
                {
                    StartCoroutine(SpawnWave());
                }
            }
            else
            {
                waveCountdown -= Time.deltaTime;
            }
       /* }
        else
        {
            // check whether the current wave was launched and not completed
            if (state == SpawnState.SPAWNING || state == SpawnState.WAITING)
            {
                // clearing the scene of current enemies
                ClearEnemies();
            }
        }*/
    }

    /*private void ClearEnemies()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            Destroy(enemy);
        }
    }*/

    private void WaveCompleted()
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>()
        {
            { "waveCount", waveCount}
        };
        
        AnalyticsService.Instance.CustomData("waveCompleted", parameters);
        itemController.ShowUpgrades();
        state = SpawnState.COUNTING;
        waveCountdown = timeBetweenWaves;
        waveCount++;
    }

    private bool EnemyIsAlive()
    {
        searchCountdown -= Time.deltaTime;
        if(searchCountdown <= 0)
        {
            searchCountdown = 1f;
            if(GameObject.FindGameObjectWithTag("Enemy") == null)
            {
                return false;
            }
        }
        return true;
    }

    private IEnumerator SpawnWave ()
    {
        Debug.Log("spawning wave: " + waveCount);
        state = SpawnState.SPAWNING;
        
        int objectCount = enemyTransformPairs.Count; // number of elements in the list objectTransformPairs

        for (int i = 0; i < countEnemies; i++)
        {
            // get the index of an element in a list or array enemies using the moduls operation
            int index = i % objectCount;

            GameObject enemy = enemyTransformPairs[index].enemy;
            Transform point = enemyTransformPairs[index].point;
            InstantiateEnemy(enemy, point);

            yield return new WaitForSeconds(0.5f);
        }

        if(waveCount >= 3)
        {
            InstantiateEnemy(bossEnemy, enemyTransformPairs[3].point);
        }
        state = SpawnState.WAITING;
        countEnemies += 2;
        healthEnemiesIncrease += (int)Math.Ceiling((healthEnemiesIncrease * 15) / 100.0);
        attackEnemiesDamage += (int)Math.Ceiling((healthEnemiesIncrease * 15) / 100.0);
        yield break;
    }

    private void InstantiateEnemy(GameObject enemy, Transform point)
    {
        GameObject newEnemy = Instantiate(enemy, point.position, Quaternion.identity);
        var stats = newEnemy.GetComponents<EnemyStats>();
        foreach(var s in stats)
        {
            s.maxHealth += healthEnemiesIncrease;
            s.currentHealth += healthEnemiesIncrease;
            s.attackDamage += attackEnemiesDamage;
        }
    }
}
