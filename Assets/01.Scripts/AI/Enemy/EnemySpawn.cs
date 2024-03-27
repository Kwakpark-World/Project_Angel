using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    [SerializeField]
    private EnemySpawnRangeSO enemySpawnRange;
    [SerializeField]
    private int maxEnemyCount = 7;

    public WaveFont waveFont;

    private void Start()
    {
        EnemySpawner();
        waveFont.WavePrint();
    }

    private void Update()
    {
        if(GameManager.Instance.EnemyDieCount == maxEnemyCount)
        {
            GameManager.Instance.SpawnWave++;
            EnemySpawner();
            GameManager.Instance.EnemyDieCount = 0;
            waveFont.WavePrint();
        }
    }

    public void EnemySpawner()
    {
        
        while(GameManager.Instance.EnemySpawnCount < maxEnemyCount || GameManager.Instance.SpawnWave == 3)
        {
            Vector3 spawnPosition = new Vector3(
            Random.Range(enemySpawnRange.minimumSpawnRange.x, enemySpawnRange.maximumSpawnRange.x),
            Random.Range(enemySpawnRange.minimumSpawnRange.y, enemySpawnRange.maximumSpawnRange.y),
            Random.Range(enemySpawnRange.minimumSpawnRange.z, enemySpawnRange.maximumSpawnRange.z));

            // 랜덤으로 적의 종류 선택
            float randomValue = Random.value;

            EnemyBrain enemy;

            if (randomValue < 0.35f)
            {
                enemy = PoolManager.Instance.Pop(PoolingType.KnightEnemy, spawnPosition) as EnemyBrain;
            }
            else if (randomValue < 0.65f)
            {
                enemy = PoolManager.Instance.Pop(PoolingType.ArcherEnemy, spawnPosition) as EnemyBrain;
            }
            else
            {
                enemy = PoolManager.Instance.Pop(PoolingType.WitchEnemy, spawnPosition) as EnemyBrain;
            }

            GameManager.Instance.EnemySpawnCount++;
        }

        if (GameManager.Instance.EnemySpawnCount == maxEnemyCount)
        {
            GameManager.Instance.EnemySpawnCount = 0;
        }
    }
}
