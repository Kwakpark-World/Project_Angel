using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public Vector3 MinSpawnValue;
    public Vector3 MaxSpawnValue;

    private float SpawnWave = 3f;
    private float EnemySpawnCount = 15;

    public void Update()
    {
        // 최소값과 최대값 사이의 랜덤한 값을 생성합니다.
        Vector3 spawnValue = new Vector3(
            Random.Range(MinSpawnValue.x, MaxSpawnValue.x),
            Random.Range(MinSpawnValue.y, MaxSpawnValue.y),
            Random.Range(MinSpawnValue.z, MaxSpawnValue.z)
        );

        Debug.Log(spawnValue);
    }

    private void knightEnemySpawn()
    {
        //EnemyAI enemyAI = PoolManager.instance.Pop()
    }

    public void ArcherEnemySpawn()
    {
        //EnemyAI.
    }
}
