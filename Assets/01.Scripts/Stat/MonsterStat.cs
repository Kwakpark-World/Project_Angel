using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Stat/Enemy")]
public class MonsterStat : EnemyStat
{
    protected void OnEnable()
    {
        Type enemyStatType = typeof(EnemyStat);

        foreach (EnemyStatType statType in Enum.GetValues(typeof(EnemyStatType)))
        {
            string statName = statType.ToString();
            FieldInfo statField = enemyStatType.GetField(statName);

            if (statField == null)
            {
                Debug.LogError($"There are no stat! error : {statName}");
            }
            else
            {
                _fieldInfoDictionary.Add(statType, statField);
            }
        }

        currentHealth.SetDefalutValue(GetMaxHealthValue());
    }
    //
    public Stat GetStatByType(EnemyStatType statType)
    {
        return _fieldInfoDictionary[statType].GetValue(this) as Stat;
    }
}
