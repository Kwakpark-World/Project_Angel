using UnityEngine;
using UnityEngine.AI;

public class EnemyMannequin : MonoBehaviour
{
    public EnemySpawnDataSO enemySpawnData;

    private float ratioSum;

    private void Awake()
    {
        InitializeSpawner();
    }

    private void Start()
    {
        SpawnEnemy();
        gameObject.SetActive(false);
    }

    public void InitializeSpawner()
    {
        enemySpawnData = Instantiate(enemySpawnData, transform);

        foreach (EnemyToSpawn enemy in enemySpawnData.enemiesToSpawn)
        {
            if (!enemy.canSpawn)
            {
                continue;
            }

            ratioSum += enemy.spawnRatio;
            enemy.spawnRatio = ratioSum;
        }

        foreach (EnemyToSpawn enemy in enemySpawnData.enemiesToSpawn)
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
        float randomValue = Random.value;

        foreach (EnemyToSpawn enemy in enemySpawnData.enemiesToSpawn)
        {
            if (!enemy.canSpawn)
            {
                continue;
            }

            if (enemy.spawnRatio >= randomValue)
            {
                Brain brain = PoolManager.Instance.Pop(enemy.enemyType, transform.position) as Brain;

                break;
            }
        }
    }
}
