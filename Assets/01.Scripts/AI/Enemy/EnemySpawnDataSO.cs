using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EnemyToSpawn
{
    public PoolType enemyType;
    public bool canSpawn;
    public float spawnRatio;
}

[CreateAssetMenu(menuName = "SO/Data/EnemySpawn")]
public class EnemySpawnDataSO : ScriptableObject
{
    public List<EnemyToSpawn> enemiesToSpawn;
}
