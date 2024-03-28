using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    [SerializeField]
    private EnemySpawnValueSO enemySpawnValue;
    [SerializeField]
    private int maxEnemyCount = 7;
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
        if (GameManager.Instance.EnemyDieCount == maxEnemyCount)
        {
            GameManager.Instance.SpawnWave++;
            SpawnEnemy();
            GameManager.Instance.EnemyDieCount = 0;
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
        while(GameManager.Instance.EnemySpawnCount < maxEnemyCount)
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
                    PoolManager.Instance.Pop(enemy.enemyType, spawnPosition);

                    GameManager.Instance.EnemySpawnCount++;

                    break;
                }
            }
        }

        if (GameManager.Instance.EnemySpawnCount == maxEnemyCount)
        {
            GameManager.Instance.EnemySpawnCount = 0;
        }
    }
}
