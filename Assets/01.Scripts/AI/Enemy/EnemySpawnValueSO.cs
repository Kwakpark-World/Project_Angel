using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EnemyToSpawn
{
    public PoolingType enemyType;
    public bool canSpawn;
    public float spawnRatio;
}

[CreateAssetMenu(menuName = "SO/EnemySpawnValue")]
public class EnemySpawnValueSO : ScriptableObject
{
    public Vector3 minimumSpawnRange;
    public Vector3 maximumSpawnRange;
    public List<EnemyToSpawn> enemiesToSpawn;
    public int maxEnemySpawnCount = 7;
}
