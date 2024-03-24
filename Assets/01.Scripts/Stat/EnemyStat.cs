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
    moveSpeed,
    rotateSpeed,
    patternAmount,
    initialPatternCooldown,
}

public class EnemyStat : ScriptableObject
{
    [Header("Defensive stats")]
    public Stat maxHealth; // �ִ� ü��
    public Stat lifetime; // ����
    public Stat defensivePower; // ����

    [Header("Offensive stats")]
    public Stat detectRange; // Ž�� ����
    public Stat attackPower; // ���ݷ�
    public Stat attackRange; // ���� ����
    public Stat attackDelay; // ���� ������

    [Header("Move stats")]
    public Stat moveSpeed; // �̵� �ӵ�
    public Stat rotateSpeed; // ȸ�� �ӵ�

    [Header("Boss stats")]
    public Stat patternAmount; // ���� ����
    public Stat initialPatternCooldown; // �ʱ� ���� ��ٿ�

    protected Brain owner;

    protected Dictionary<EnemyStatType, FieldInfo> fieldInfoDictionary = new Dictionary<EnemyStatType, FieldInfo>();

    public virtual void SetOwner(Brain owner)
    {
        this.owner = owner;
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

    public float GetInitialPatternCooldown()
    {
        return initialPatternCooldown.GetValue();
    }
}
