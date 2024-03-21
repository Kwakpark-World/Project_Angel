using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public Vector3 MinSpawnValue;
    public Vector3 MaxSpawnValue;

    //private 

    public void Update()
    {
        EnemySpawner();
    }

    public void Start()
    {
        
    }

    public void EnemySpawner()
    {
        if(GameManager.Instance.SpawnWave < 3)
        {
            if (GameManager.Instance.EnemySpawnCount < 7)
            {
                Vector3 spawnPosition = new Vector3(
                    Random.Range(MinSpawnValue.x, MaxSpawnValue.x),
                    Random.Range(MinSpawnValue.y, MaxSpawnValue.y),
                    Random.Range(MinSpawnValue.z, MaxSpawnValue.z)
                );

                // 랜덤으로 적의 종류 선택
                float randomValue = Random.value;  // 0: 기사, 1: 궁수, 2: 마법사

                EnemyBrain enemy = null;


                if (randomValue < 0.35f) // 30% 확률로 기사
                {
                    enemy = PoolManager.Instance.Pop(PoolingType.KnightEnemy) as EnemyBrain;
                }
                else if (randomValue < 0.65f) // 30% 확률로 궁수
                {
                    enemy = PoolManager.Instance.Pop(PoolingType.ArcherEnemy) as EnemyBrain;
                }
                else // 나머지 확률로 마법사
                {
                    enemy = PoolManager.Instance.Pop(PoolingType.WitchEnemy) as EnemyBrain;
                }


                if (enemy != null)
                {
                    enemy.transform.position = spawnPosition;
                    GameManager.Instance.EnemySpawnCount++;
                }
            }
        }
    }

    public void ArcherEnemySpawn()
    {

    }
}
