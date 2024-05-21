using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public enum EnemyStatType
{
    maxHealth,
    lifetime,
    defensivePower,
    detectRange,
    attackPower,
    attackRange,
    attackDelay,
    skillCooldown,
    moveSpeed,
    rotateSpeed,
    patternAmount,
}

[CreateAssetMenu(menuName = "SO/Stat/Enemy")]
public class EnemyStat : ScriptableObject
{
    [Header("Defensive stats")]
    [Tooltip("최대 체력")]
    public Stat maxHealth;
    [Tooltip("수명")]
    public Stat lifetime;
    [Tooltip("방어력")]
    public Stat defensivePower;

    [Header("Offensive stats")]
    [Tooltip("탐지 범위")]
    public Stat detectRange;
    [Tooltip("공격력")]
    public Stat attackPower;
    [Tooltip("공격 범위")]
    public Stat attackRange;
    [Tooltip("공격 딜레이")]
    public Stat attackDelay;
    [Tooltip("스킬 쿨다운")]
    public Stat skillCooldown;

    [Header("Move stats")]
    [Tooltip("이동 속도")]
    public Stat moveSpeed;
    [Tooltip("회전 속도")]
    public Stat rotateSpeed;

    [Header("Boss stats")]
    [Tooltip("패턴 개수")]
    public Stat patternAmount;

    private Brain _owner;

    private Dictionary<EnemyStatType, FieldInfo> _fieldInfoDictionary = new Dictionary<EnemyStatType, FieldInfo>();

    private void OnEnable()
    {
        Type enemyStatType = GetType();

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
                _fieldInfoDictionary.Add(statType, statField);
            }
        }
    }

    public void SetOwner(Brain owner)
    {
        _owner = owner;
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
        return _fieldInfoDictionary[statType].GetValue(this) as Stat;
    }

    public float GetMaxHealth()
    {
        return maxHealth.GetValue();
    }

    public float GetLifetime()
    {
        return lifetime.GetValue();
    }

    public float GetDefensivePower()
    {
        return defensivePower.GetValue();
    }

    public float GetDetectRange()
    {
        return detectRange.GetValue();
    }

    public float GetAttackPower()
    {
        return attackPower.GetValue();
    }

    public float GetAttackRange()
    {
        return attackRange.GetValue();
    }

    public float GetAttackDelay()
    {
        return attackDelay.GetValue();
    }

    public float GetSkillCooldown()
    {
        return skillCooldown.GetValue();
    }

    public float GetMoveSpeed()
    {
        return moveSpeed.GetValue();
    }

    public float GetRotateSpeed()
    {
        return rotateSpeed.GetValue();
    }

    public int GetPatternAmount()
    {
        return (int)patternAmount.GetValue();
    }
}
