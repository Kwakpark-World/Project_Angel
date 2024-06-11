using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public enum PlayerStatType
{
    maxHealth = 0,
    defensivePower = 10,
    defenseCooldown,
    attackPower = 100,
    attackSpeed,
    criticalChance = 110,
    criticalDamageMultiplier,
    chargingAttackSpeed = 120,
    chargingAttackDistance,
    chargingAttackCooldown,
    knockbackPower = 130,
    knockbackDuration,
    moveSpeed = 200,
    dashSpeed = 210,
    dashMaxDistance,
    dashCooldown,
    rotateSpeed = 220,
    slamMaxDistance = 300,
    slamCooldown,
    maxAwakenGauge = 310,
}

[CreateAssetMenu(menuName = "SO/Stat/Player")]
public class PlayerStat : ScriptableObject
{
    [Header("Defensive stats")]
    [Tooltip("최대 체력")]
    public Stat maxHealth;
    [Tooltip("방어력")]
    public Stat defensivePower;
    [Tooltip("방어 쿨다운")]
    public Stat defenseCooldown;

    [Header("Offensive stats")]
    [Tooltip("공격력")]
    public Stat attackPower;
    [Tooltip("공격 속도")]
    public Stat attackSpeed;
    [Tooltip("치명타 확률")]
    public Stat criticalChance;
    [Tooltip("치명타 데미지")]
    public Stat criticalDamageMultiplier;
    [Tooltip("차징 공격 속도")]
    public Stat chargingAttackSpeed;
    [Tooltip("차징 공격 찌르기 이동 거리")]
    public Stat chargingAttackDistance;
    [Tooltip("차징 공격 쿨다운")]
    public Stat chargingAttackCooldown;
    [Tooltip("넉백 위력")]
    public Stat knockbackPower;
    [Tooltip("넉백 지속 시간")]
    public Stat knockbackDuration;

    [Header("Move stats")]
    [Tooltip("이동 속도")]
    public Stat moveSpeed;
    [Tooltip("대시 속도")]
    public Stat dashSpeed;
    [Tooltip("각성 중 대시 최대 거리")]
    public Stat dashMaxDistance;
    [Tooltip("대시 쿨다운")]
    public Stat dashCooldown;
    [Tooltip("회전 속도")]
    public Stat rotateSpeed;

    [Header("Skill stats")]
    [Tooltip("Q 스킬 최대 이동 거리")]
    public Stat slamMaxDistance;
    [Tooltip("Q 스킬 쿨다운")]
    public Stat slamCooldown;
    [Tooltip("최대 각성 게이지")]
    public Stat maxAwakenGauge;

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

    public float GetDefenseCooldown()
    {
        return defenseCooldown.GetValue();
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

    public float GetChargingAttackSpeed()
    {
        return chargingAttackSpeed.GetValue();
    }

    public float GetChargingAttackDistance()
    {
        return chargingAttackDistance.GetValue();
    }

    public float GetChargingAttackCooldown()
    {
        return chargingAttackCooldown.GetValue();
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
}