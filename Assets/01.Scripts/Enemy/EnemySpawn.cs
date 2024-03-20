using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public Vector3 MinSpawnValue;
    public Vector3 MaxSpawnValue;

    //private 

    private float SpawnWave = 3f;
    private int EnemySpawnCount = 0;

    public void Update()
    {
        knightEnemySpawn();
    }

    public void knightEnemySpawn()
    {
        if (EnemySpawnCount < 15) 
        {
            // 무작위 소환 위치 계산
            Vector3 spawnPosition = new Vector3(
                Random.Range(MinSpawnValue.x, MaxSpawnValue.x),
                Random.Range(MinSpawnValue.y, MaxSpawnValue.y),
                Random.Range(MinSpawnValue.z, MaxSpawnValue.z)
            );

            EnemyBrain enemy = PoolManager.Instance.Pop(PoolingType.KnightEnemy) as EnemyBrain; 

            enemy.transform.position = spawnPosition;
            EnemySpawnCount++;
        }

    }

    public void ArcherEnemySpawn()
    {
        //EnemyAI.
    }
}
