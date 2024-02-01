using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public GameObject enemyPrefab; // 적 프리팹을 Inspector에서 할당
    public int numberOfEnemies = 5; // 생성할 적의 개수

    void Start()
    {
        SpawnEnemies();
    }

    void SpawnEnemies()
    {
        for (int i = 0; i < numberOfEnemies; i++)
        {
            GameObject newEnemy = Instantiate(enemyPrefab, GetRandomSpawnPosition(), Quaternion.identity);

            EnemyAI enemyAI = newEnemy.GetComponent<EnemyAI>();

            EnemyStats enemyStats = ScriptableObject.CreateInstance<EnemyStats>();
            enemyStats._damage = 10f; 
            enemyStats._coolTime = 2f;
            enemyStats._maxHp = 100f;
            enemyStats._currentHp = enemyStats._maxHp;

            enemyAI.enemyStats = enemyStats;
        }
    }

    Vector3 GetRandomSpawnPosition()
    {
        float randomX = Random.Range(-10f, 10f);
        float randomZ = Random.Range(-10f, 10f);

        return new Vector3(randomX, 0f, randomZ);
    }
}
