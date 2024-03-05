using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Stat/Player")]
public class PlayerStat : CharacterStat
{
    protected void OnEnable()
    {
        Type playerStatType = typeof(PlayerStat);

        foreach (PlayerStatType statType in Enum.GetValues(typeof(PlayerStatType)))
        {
            string statName = statType.ToString();
            FieldInfo statField = playerStatType.GetField(statName);

            if (statField == null)
            {
                Debug.LogError($"There are no stat! error : {statName}");
            }
            else
            {
                _fieldInfoDictionary.Add(statType, statField);
            }
        }
    }
    //
    public Stat GetStatByType(PlayerStatType statType)
    {
        return _fieldInfoDictionary[statType].GetValue(this) as Stat;
    }
}
