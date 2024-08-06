using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public enum PlayerStatType
{
    maxHealth = 0,
    defensivePower = 10,
    attackPower = 100,
    attackSpeed,
    criticalChance = 110,
    criticalDamageMultiplier,
    chargeAttackSpeed = 120,
    chargeAttackDistance,
    chargeAttackCooldown,
    knockbackPower = 130,
    knockbackDuration,
    moveSpeed = 200,
    dashSpeed = 210,
    dashMaxDistance,
    dashCooldown,
    rotateSpeed = 220,
    rotateYSpeed,
    slamMaxDistance = 300,
    slamCooldown,
    maxAwakenGauge = 310,
    maxAwakenDuration,
    minChargeTime = 320,
    maxChargeTime,
    whirlwindCooldown = 330,
}

[CreateAssetMenu(menuName = "SO/Stat/Player")]
public class PlayerStat : ScriptableObject
{
    [Header("Defensive stats")]
    public Stat maxHealth;
    public Stat defensivePower;

    [Header("Offensive stats")]
    public Stat attackPower;
    public Stat attackSpeed;
    public Stat criticalChance;
    public Stat criticalDamageMultiplier;
    public Stat chargeAttackSpeed;
    public Stat chargeAttackDistance;
    public Stat chargeAttackCooldown;
    public Stat knockbackPower;
    public Stat knockbackDuration;

    [Header("Move stats")]
    public Stat moveSpeed;
    public Stat dashSpeed;
    public Stat dashMaxDistance;
    public Stat dashCooldown;
    public Stat rotateSpeed;
    public Stat rotateYSpeed;

    [Header("Skill stats")]
    public Stat slamMaxDistance;
    public Stat slamCooldown;
    public Stat maxAwakenGauge;
    public Stat maxAwakenDuration;
    public Stat minChargeTime;
    public Stat maxChargeTime;
    public Stat whirlwindCooldown;

    private PlayerController _owner;

    private Dictionary<PlayerStatType, FieldInfo> _fieldInfoDictionary = new Dictionary<PlayerStatType, FieldInfo>();

    private void OnEnable()
    {
        Type playerStatType = GetType();

        foreach (PlayerStatType statType in Enum.GetValues(typeof(PlayerStatType)))
        {
            string statName = statType.ToString();
            FieldInfo statField = playerStatType.GetField(statName);

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

    public void SetOwner(PlayerController owner)
    {
        _owner = owner;
    }

    public void InitializeAllModifiers()
    {
        foreach (PlayerStatType statType in Enum.GetValues(typeof(PlayerStatType)))
        {
            GetStatByType(statType).InitializeModifier();
        }
    }

    public Stat GetStatByType(PlayerStatType statType)
    {
        return _fieldInfoDictionary[statType].GetValue(this) as Stat;
    }

    public float GetMaxHealth()
    {
        return maxHealth.GetValue();
    }

    public float GetDefensivePower()
    {
        return defensivePower.GetValue();
    }

    public float GetAttackPower()
    {
        return attackPower.GetValue();
    }

    public float GetAttackSpeed()
    {
        return attackSpeed.GetValue();
    }

    public float GetCriticalChance()
    {
        return criticalChance.GetValue();
    }

    public float GetCriticalDamageMultiplier()
    {
        return criticalDamageMultiplier.GetValue();
    }

    public float GetChargeAttackSpeed()
    {
        return chargeAttackSpeed.GetValue();
    }

    public float GetChargeAttackDistance()
    {
        return chargeAttackDistance.GetValue();
    }

    public float GetChargeAttackCooldown()
    {
        return chargeAttackCooldown.GetValue();
    }

    public float GetKnockbackPower()
    {
        return knockbackPower.GetValue();
    }

    public float GetKnockbackDuration()
    {
        return knockbackDuration.GetValue();
    }

    public float GetMoveSpeed()
    {
        return moveSpeed.GetValue();
    }

    public float GetDashSpeed()
    {
        return dashSpeed.GetValue();
    }

    public float GetDashMaxDistance()
    {
        return dashMaxDistance.GetValue();
    }

    public float GetDashCooldown()
    {
        return dashCooldown.GetValue();
    }

    public float GetRotateSpeed()
    {
        return rotateSpeed.GetValue();
    }

    public float GetRotateYSpeed()
    {
        return rotateYSpeed.GetValue();
    }

    public float GetSlamMaxDistance()
    {
        return slamMaxDistance.GetValue();
    }

    public float GetSlamCooldown()
    {
        return slamCooldown.GetValue();
    }

    public float GetMaxAwakenGauge()
    {
        return maxAwakenGauge.GetValue();
    }

    public float GetMaxAwakenDuration()
    {
        return maxAwakenDuration.GetValue();
    }

    public float GetMinChargeTime()
    {
        return minChargeTime.GetValue();
    }

    public float GetMaxChargeTime()
    {
        return maxChargeTime.GetValue();
    }

    public float GetWhirlwindCooldown()
    {
        return whirlwindCooldown.GetValue();
    }
}