using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    [SerializeField]
    public EnemySpawnValueSO enemySpawnValue;
    [HideInInspector]
    public WaveFont waveFont;
    [HideInInspector]
    public int spawnWave = 0;
    [HideInInspector]
    public int enemyDieCount = 0;
    private int _enemySpawnCount = 0;
    
    private float ratioSum;                                           

    private void Awake()
    {
        InitializeSpawner();
    }                                                                                                                                                 

    private void Start()
    {
        SpawnEnemy();
        waveFont?.WavePrint();
    }

    private void Update()
    {
        if (enemyDieCount == enemySpawnValue.maxEnemySpawnCount)
        {
            enemyDieCount = 0;
            _enemySpawnCount = 0;
            SpawnEnemy();
            spawnWave++;
            waveFont?.WavePrint();
        }
    }

    public void InitializeSpawner()
    {
        enemySpawnValue = Instantiate(enemySpawnValue, transform);

        foreach (EnemyToSpawn enemy in enemySpawnValue.enemiesToSpawn)
        {
            if (!enemy.canSpawn)
            {
                continue;
            }

            ratioSum += enemy.spawnRatio;
            enemy.spawnRatio = ratioSum;
        }

        foreach (EnemyToSpawn enemy in enemySpawnValue.enemiesToSpawn)
        {
            if (!enemy.canSpawn)
            {
                continue;
            }

            enemy.spawnRatio = enemy.spawnRatio / ratioSum;
        }
    }

    public void SpawnEnemy()
    {
        while(_enemySpawnCount < enemySpawnValue.maxEnemySpawnCount)
        {
            Vector3 spawnPosition = new Vector3(
                Random.Range(enemySpawnValue.minimumSpawnRange.x, enemySpawnValue.maximumSpawnRange.x),
                Random.Range(enemySpawnValue.minimumSpawnRange.y, enemySpawnValue.maximumSpawnRange.y),
                Random.Range(enemySpawnValue.minimumSpawnRange.z, enemySpawnValue.maximumSpawnRange.z)
            );
            
            float randomValue = Random.value;
            
            foreach (EnemyToSpawn enemy in enemySpawnValue.enemiesToSpawn)
            {
                if (!enemy.canSpawn)
                {
                    continue;
                }

                if (enemy.spawnRatio >= randomValue)
                {
                    Brain brain = PoolManager.Instance.Pop(enemy.enemyType, spawnPosition) as Brain;
                    brain.enemySpawn = this;
                    _enemySpawnCount++;

                    break;
                }
                Debug.Log(spawnWave);
            }
        }
    }


}
