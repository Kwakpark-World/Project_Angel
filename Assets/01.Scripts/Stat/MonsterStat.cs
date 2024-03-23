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
                Debug.LogError($"{statName} stat doesn't exist.");
            }
            else
            {
                fieldInfoDictionary.Add(statType, statField);
            }
        }
    }

    public void InitializeAllModifiers()
    {
        foreach (EnemyStatType statType in Enum.GetValues(typeof(EnemyStatType)))
        {
            GetStatByType(statType).InitializeModifier();
        }
    }

    public Stat GetStatByType(EnemyStatType statType)
    {
        return fieldInfoDictionary[statType].GetValue(this) as Stat;
    }
}
