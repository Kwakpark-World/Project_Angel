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

public enum DebuffType
{
    Poison,
    Freeze,
    Knockback
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

    protected PlayerController _owner;

    private DebuffType _debuffType;

    protected Dictionary<PlayerStatType, FieldInfo> _fieldInfoDictionary = new Dictionary<PlayerStatType, FieldInfo>();
    protected Dictionary<DebuffType, bool> debuffDictionary = new Dictionary<DebuffType, bool>();
    protected List<Coroutine> _coroutines = new List<Coroutine>(Enum.GetValues(typeof(DebuffType)).Length);

    public virtual void SetOwner(PlayerController owner)
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
        if (!(_owner as Player).IsDefense && !(_owner as Player).IsDie)
        {
            currentHealth.AddModifier(-Mathf.Max(incomingDamage - GetDefensivePower(), 0f));
        }
    }

    public void Debuff(DebuffType type, float duration)
    {
        _coroutines[(int)type] = _owner.StartCoroutine(DebuffCoroutine(type, duration));
    }

    public bool GetDebuff(DebuffType type)
    {
        return debuffDictionary[type];
    }

    private IEnumerator DebuffCoroutine(DebuffType type, float duration)
    {
        debuffDictionary[type] = true;

        yield return new WaitForSeconds(duration);

        debuffDictionary[type] = false;
        _coroutines[(int)type] = null;
    }

    private float poisonDamage = 2;
    private float poisonDuration = 3f;
    private float poisonDurationTimer = -1f;
    private float poisonDelay = 1f;
    private float poisonDelayTimer = -1f;

    public void Poison()
    {
        if (poisonDurationTimer <= 0f)
        {
            poisonDurationTimer = Time.time;
            poisonDelayTimer = Time.time - poisonDelay;
        }

        if (Time.time <= poisonDurationTimer + poisonDuration)
        {
            if (Time.time > poisonDelayTimer + poisonDelay)
            {
                GameManager.Instance.player.PlayerStat.Hit(poisonDamage);

                poisonDelayTimer  = Time.time;
            }
        }
        else
        {
            poisonDurationTimer = poisonDelayTimer = -1f;
        }
    }

    private float freezeMoveSpeedModifier = -2f;
    private float freezeDuration = 3f;
    private float freezeDurationTimer = -1f;

    public void Freeze()
    {
        if (freezeDurationTimer <= 0f)
        {
            freezeDurationTimer = Time.time;

            moveSpeed.AddModifier(freezeMoveSpeedModifier);
        }

        if (Time.time > freezeDurationTimer + freezeDuration)
        {
            moveSpeed.RemoveModifier(freezeMoveSpeedModifier);

            freezeDurationTimer = -1f;
        }
    }
}