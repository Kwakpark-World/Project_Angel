using UnityEngine;

public class EnemyMannequin : MonoBehaviour
{
    public EnemySpawnDataSO enemySpawnData;

    public Brain _brain;
    private float _ratioSum;

    private void Awake()
    {
        InitializeSpawner();
    }

    private void Update()
    {
        if (!_brain)
        {
            return;
        }

        if (_brain.AnimatorCompo.GetCurrentAnimationState("Die"))
        {
            _brain = null;

            gameObject.SetActive(false);
        }
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

            _ratioSum += enemy.spawnRatio;
            enemy.spawnRatio = _ratioSum;
        }

        foreach (EnemyToSpawn enemy in enemySpawnData.enemiesToSpawn)
        {
            if (!enemy.canSpawn)
            {
                continue;
            }

            enemy.spawnRatio = enemy.spawnRatio / _ratioSum;
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
                _brain = PoolManager.Instance.Pop(enemy.enemyType, transform.position) as Brain;

                break;
            }
        }
    }
}
