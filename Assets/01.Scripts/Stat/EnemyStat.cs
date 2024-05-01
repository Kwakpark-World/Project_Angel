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

public class EnemyStat : ScriptableObject
{
    [Header("Defensive stats")]
    public Stat maxHealth; // 최대 체력
    public Stat lifetime; // 수명
    public Stat defensivePower; // 방어력

    [Header("Offensive stats")]
    public Stat detectRange; // 탐지 범위
    public Stat attackPower; // 공격력
    public Stat attackRange; // 공격 범위
    public Stat attackDelay; // 공격 딜레이
    public Stat skillCooldown; // 스킬 쿨다운

    [Header("Move stats")]
    public Stat moveSpeed; // 이동 속도
    public Stat rotateSpeed; // 회전 속도

    [Header("Boss stats")]
    public Stat patternAmount; // 패턴 개수

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
