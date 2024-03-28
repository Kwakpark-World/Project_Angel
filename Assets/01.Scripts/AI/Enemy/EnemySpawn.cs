using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    [SerializeField]
    public EnemySpawnValueSO enemySpawnValue;
    [SerializeField]
    public int SpawnWave = 0;
    public int EnemySpawnCount = 0;
    public int EnemyDieCount = 0;
    
    private float ratioSum;                                           

    private void Awake()
    {
        InitializeSpawner();
    }

    public WaveFont waveFont;                                                                                                                                                     

    private void Start()
    {
        SpawnEnemy();
        waveFont.WavePrint();
    }

    private void Update()
    {
        if (EnemyDieCount == enemySpawnValue.maxEnemyCount)
        {
            EnemyDieCount = 0;
            SpawnWave++;
            SpawnEnemy();
            EnemySpawnCount = 0;
            waveFont.WavePrint();
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
        while(EnemySpawnCount < enemySpawnValue.maxEnemyCount)
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
                    EnemySpawnCount++;

                    break;
                }
            }
        }
    }


}
