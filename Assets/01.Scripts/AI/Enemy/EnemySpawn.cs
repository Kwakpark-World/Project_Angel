using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    [SerializeField]
    private EnemySpawnValueSO enemySpawnValue;
    [SerializeField]
    private int maxSpawnWave = 3;
    [SerializeField]
    private int maxEnemyCount = 7;
    private float ratioSum;

    private void Awake()
    {
        InitializeSpawner();
    }

    private void Update()
    {
        SpawnEnemy();
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
        if (GameManager.Instance.SpawnWave < maxSpawnWave)
        {
            if (GameManager.Instance.EnemySpawnCount < maxEnemyCount)
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
        }
    }
}
