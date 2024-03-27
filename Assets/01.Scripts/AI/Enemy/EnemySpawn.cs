using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    [SerializeField]
    private EnemySpawnRangeSO enemySpawnRange;
    [SerializeField]
    private int maxSpawnWave = 3;
    [SerializeField]
    private int maxEnemyCount = 7;

    private void Update()
    {
        EnemySpawner();
    }

    public void EnemySpawner()
    {
        if(GameManager.Instance.SpawnWave < maxSpawnWave)
        {
            if (GameManager.Instance.EnemySpawnCount < maxEnemyCount)
            {
                Vector3 spawnPosition = new Vector3(
                    Random.Range(enemySpawnRange.minimumSpawnRange.x, enemySpawnRange.maximumSpawnRange.x),
                    Random.Range(enemySpawnRange.minimumSpawnRange.y, enemySpawnRange.maximumSpawnRange.y),
                    Random.Range(enemySpawnRange.minimumSpawnRange.z, enemySpawnRange.maximumSpawnRange.z)
                );

                // 랜덤으로 적의 종류 선택
                float randomValue = Random.value;  // 0: 기사, 1: 궁수, 2: 마법사

                EnemyBrain enemy;

                if (randomValue < 0.35f) // 30% 확률로 기사
                {
                    enemy = PoolManager.Instance.Pop(PoolingType.KnightEnemy, spawnPosition) as EnemyBrain;
                }
                else if (randomValue < 0.65f) // 30% 확률로 궁수
                {
                    enemy = PoolManager.Instance.Pop(PoolingType.ArcherEnemy, spawnPosition) as EnemyBrain;
                }
                else // 나머지 확률로 마법사
                {
                    enemy = PoolManager.Instance.Pop(PoolingType.WitchEnemy, spawnPosition) as EnemyBrain;
                }

                if (enemy != null)
                {
                    GameManager.Instance.EnemySpawnCount++;
                }
            }
        }
    }
}
