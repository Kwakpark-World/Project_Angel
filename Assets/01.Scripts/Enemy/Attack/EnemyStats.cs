using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Stats/Enemy")]
public class EnemyStats : ScriptableObject
{
    [Header("Attack")]
    public float _damage;
    public float _coolTime;
    [Header("Hp")]
    public float _maxHp;
    public float _currentHp;
    [Header("Move")]
    public float _moveSpeed;
}
