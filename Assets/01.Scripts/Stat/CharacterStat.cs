using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public enum PlayerStatType
{
    maxHealth,
    currentHealth,
    defensivePower,
    moveSpeed,
    attackPower,
    criticalChance,
    criticalMultiplier,
}

public class CharacterStat : ScriptableObject
{
    [Header("Defensive stats")]
    public Stat maxHealth; // 최대 체력
    public Stat currentHealth; // 현재 체력
    public Stat defensivePower; // 방어력
    public Stat moveSpeed; // 이동 속도

    [Header("Offensive stats")]
    public Stat attackPower; // 공격력
    public Stat criticalChance; // 치명타 확률
    public Stat criticalMultiplier; // 치명타 배율 

    protected PlayerController owner;

    protected Dictionary<PlayerStatType, FieldInfo> fieldInfoDictionary = new Dictionary<PlayerStatType, FieldInfo>();

    public virtual void SetOwner(PlayerController owner)
    {
        this.owner = owner;
    }

    public virtual void IncreaseStatBy(float modifyValue, float duration, Stat statToModify)
    {
        owner.StartCoroutine(StatModifyCoroutine(modifyValue, duration, statToModify));
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

    public float GetDefensivePower()
    {
        return defensivePower.GetValue();
    }

    public float GetMoveSpeed()
    {
        return moveSpeed.GetValue();
    }

    public float GetAttackPower()
    {
        return attackPower.GetValue();
    }

    public void Hit(float incomingDamage)
    {
        if (!(owner as Player).IsDefense && !(owner as Player).IsDie)
        {
            currentHealth.AddModifier(-Mathf.Max(incomingDamage - GetDefensivePower(), 0f));
        }
    }
}