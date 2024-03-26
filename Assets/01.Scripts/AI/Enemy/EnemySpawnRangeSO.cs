using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/EnemySpawnRange")]
public class EnemySpawnRangeSO : ScriptableObject
{
    public Vector3 minimumSpawnRange;
    public Vector3 maximumSpawnRange;
}
