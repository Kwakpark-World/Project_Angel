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

                // �������� ���� ���� ����
                float randomValue = Random.value;  // 0: ���, 1: �ü�, 2: ������

                EnemyBrain enemy;

                if (randomValue < 0.35f) // 30% Ȯ���� ���
                {
                    enemy = PoolManager.Instance.Pop(PoolingType.KnightEnemy, spawnPosition) as EnemyBrain;
                }
                else if (randomValue < 0.65f) // 30% Ȯ���� �ü�
                {
                    enemy = PoolManager.Instance.Pop(PoolingType.ArcherEnemy, spawnPosition) as EnemyBrain;
                }
                else // ������ Ȯ���� ������
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
