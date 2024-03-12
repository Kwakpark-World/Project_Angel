using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public enum EnemyStatType
{
    maxHealth,
    currentHealth,
    lifetime,
    defensivePower,
    moveSpeed,
    detectRange,
    attackPower,
    attackRange,
    attackDelay,
    patternAmount,
    initialPatternCooldown,
}

public class EnemyStat : ScriptableObject
{
    [Header("Defensive stats")]
    public Stat maxHealth; // 최대 체력
    public Stat currentHealth; // 현재 체력
    public Stat lifetime; // 수명
    public Stat defensivePower; // 방어력
    public Stat moveSpeed; // 이동 속도

    [Header("Offensive stats")]
    public Stat detectRange; // 탐지 범위
    public Stat attackPower; // 공격력
    public Stat attackRange; // 공격 범위
    public Stat attackDelay; // 공격 딜레이

    [Header("Boss stats")]
    public Stat patternAmount; // 패턴 개수
    public Stat initialPatternCooldown; // 초기 패턴 쿨다운

    protected Brain _owner;

    protected Dictionary<EnemyStatType, FieldInfo> _fieldInfoDictionary = new Dictionary<EnemyStatType, FieldInfo>();

    public virtual void SetOwner(Brain owner)
    {
        _owner = owner;
    }

    public virtual void IncreaseStatBy(float modifyValue, float duration, Stat statToModify)
    {
        _owner.StartCoroutine(StatModifyCoroutine(modifyValue, duration, statToModify));
    }

    protected IEnumerator StatModifyCoroutine(float modifyValue, float duration, Stat statToModify)
    {
        statToModify.AddModifier(modifyValue);

        yield return new WaitForSeconds(duration);

        statToModify.RemoveModifier(modifyValue);
    }

    public float GetMaxHealthValue()
    {
        return maxHealth.GetValue();
    }

    public float GetCurrentHealth()
    {
        return currentHealth.GetValue();
    }

    public float GetLifetime()
    {
        return lifetime.GetValue();
    }

    public float GetDefensivePower()
    {
        return defensivePower.GetValue();
    }

    public float GetMoveSpeed()
    {
        return moveSpeed.GetValue();
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

    public int GetPatternAmount()
    {
        return (int)patternAmount.GetValue();
    }

    public float GetInitialPatternCooldown()
    {
        return initialPatternCooldown.GetValue();
    }

    public void Hit(float incomingDamage)
    {
        currentHealth.AddModifier(-Mathf.Max(incomingDamage - GetDefensivePower(), 0f));
        _owner.OnHit();
    }
}
