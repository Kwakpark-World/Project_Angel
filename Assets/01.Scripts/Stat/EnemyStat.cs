using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public enum EnemyStatType
{
    maxHealth,
    armor,
    speed,
    detectRange,
    damage,
    attackRange,
    attackDelay,
    patternAmount,
    initialPatternCooldown,
}

public class EnemyStat : ScriptableObject
{

    [Header("Defensive stats")]
    public Stat maxHealth; //체력
    public Stat armor; //방어도
    public Stat speed;
    public Stat detectRange;


    [Header("Offensive stats")]
    public Stat damage;
    public Stat attackRange;
    public Stat attackDelay;

    [Header("Boss stats")]
    public Stat patternAmount;
    public Stat initialPatternCooldown;



    protected Entity _owner;

    protected Dictionary<EnemyStatType, FieldInfo> _fieldInfoDictionary = new Dictionary<EnemyStatType, FieldInfo>();

    public virtual void SetOwner(Entity owner)
    {
        _owner = owner;
    }

    public virtual void IncreaseStatBy(int modifyValue, float duration, Stat statToModify)
    {
        _owner.StartCoroutine(StatModifyCoroutine(modifyValue, duration, statToModify));
    }

    protected IEnumerator StatModifyCoroutine(int modifyValue, float duration, Stat statToModify)
    {
        statToModify.AddModifier(modifyValue);
        yield return new WaitForSeconds(duration);
        statToModify.RemoveModifier(modifyValue);
    }

    public float GetDamage()
    {
        return damage.GetValue();
    }

    public float ArmoredDamage(float incomingDamage, bool isChilled)
    {
        return 0;
    }

    public bool IsCritical(ref float incomingDamage)
    {
        return false;
    }

    protected float CalculateCriticalDamage(float incomingDamage)
    {
        return 0;
    }

    public float GetMaxHealthValue()
    {
        return maxHealth.GetValue();
    }

    public float GetDetectRange()
    {
        return detectRange.GetValue();
    }

    public float GetAttackRange()
    {
        return attackRange.GetValue();
    }
    
    public float GetMoveSpeed()
    {
        return speed.GetValue();
    }
}
