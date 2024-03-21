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

                // �������� ���� ���� ����
                float randomValue = Random.value;  // 0: ���, 1: �ü�, 2: ������

                EnemyBrain enemy = null;


                if (randomValue < 0.35f) // 30% Ȯ���� ���
                {
                    enemy = PoolManager.Instance.Pop(PoolingType.KnightEnemy) as EnemyBrain;
                }
                else if (randomValue < 0.65f) // 30% Ȯ���� �ü�
                {
                    enemy = PoolManager.Instance.Pop(PoolingType.ArcherEnemy) as EnemyBrain;
                }
                else // ������ Ȯ���� ������
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
