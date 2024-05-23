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
    [Tooltip("�ִ� ü��")]
    public Stat maxHealth;
    [Tooltip("����")]
    public Stat lifetime;
    [Tooltip("����")]
    public Stat defensivePower;

    [Header("Offensive stats")]
    [Tooltip("Ž�� ����")]
    public Stat detectRange;
    [Tooltip("���ݷ�")]
    public Stat attackPower;
    [Tooltip("���� ����")]
    public Stat attackRange;
    [Tooltip("���� ������")]
    public Stat attackDelay;
    [Tooltip("��ų ��ٿ�")]
    public Stat skillCooldown;

    [Header("Move stats")]
    [Tooltip("�̵� �ӵ�")]
    public Stat moveSpeed;
    [Tooltip("ȸ�� �ӵ�")]
    public Stat rotateSpeed;

    [Header("Boss stats")]
    [Tooltip("���� ����")]
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
