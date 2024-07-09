using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public enum PlayerStatType
{
    maxHealth = 0,
    defensivePower = 10,
    defenseDuration,
    defenseCooldown,
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
    slamMaxDistance = 300,
    slamCooldown,
    maxAwakenGauge = 310,
    awakenTime,
    minChargeTime = 320,
    maxChargeTime,
    whirlwindCoolDown = 330,
}

[CreateAssetMenu(menuName = "SO/Stat/Player")]
public class PlayerStat : ScriptableObject
{
    [Header("Defensive stats")]
    [Tooltip("�ִ� ü��")]
    public Stat maxHealth;
    [Tooltip("����")]
    public Stat defensivePower;
    [Tooltip("��� ���� �ð�")]
    public Stat defenseDuration;
    [Tooltip("��� ��ٿ�")]
    public Stat defenseCooldown;

    [Header("Offensive stats")]
    [Tooltip("���ݷ�")]
    public Stat attackPower;
    [Tooltip("���� �ӵ�")]
    public Stat attackSpeed;
    [Tooltip("ġ��Ÿ Ȯ��")]
    public Stat criticalChance;
    [Tooltip("ġ��Ÿ ������")]
    public Stat criticalDamageMultiplier;
    [Tooltip("��¡ ���� �ӵ�")]
    public Stat chargeAttackSpeed;
    [Tooltip("��¡ ���� ��� �̵� �Ÿ�")]
    public Stat chargeAttackDistance;
    [Tooltip("��¡ ���� ��ٿ�")]
    public Stat chargeAttackCooldown;
    [Tooltip("�˹� ����")]
    public Stat knockbackPower;
    [Tooltip("�˹� ���� �ð�")]
    public Stat knockbackDuration;

    [Header("Move stats")]
    [Tooltip("�̵� �ӵ�")]
    public Stat moveSpeed;
    [Tooltip("��� �ӵ�")]
    public Stat dashSpeed;
    [Tooltip("���� �� ��� �ִ� �Ÿ�")]
    public Stat dashMaxDistance;
    [Tooltip("��� ��ٿ�")]
    public Stat dashCooldown;
    [Tooltip("ȸ�� �ӵ�")]
    public Stat rotateSpeed;

    [Header("Skill stats")]
    [Tooltip("Q ��ų �ִ� �̵� �Ÿ�")]
    public Stat slamMaxDistance;
    [Tooltip("Q ��ų ��ٿ�")]
    public Stat slamCooldown;
    [Tooltip("�ִ� ���� ������")]
    public Stat maxAwakenGauge;
    [Tooltip("���� ���� �ð�")]
    public Stat awakenTime;
    [Tooltip("�ּ� ��¡ �ð�")]
    public Stat minChargeTime;
    [Tooltip("�ִ� ��¡ �ð�")]
    public Stat maxChargeTime;
    [Tooltip("E ��ų ��ٿ�")]
    public Stat whirlwindCoolDown;

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

    public float GetDefenseDuration()
    {
        return defenseDuration.GetValue();
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

    public float GetAwakenTime()
    {
        return awakenTime.GetValue();
    }

    public float GetMinChargeTime()
    {
        return minChargeTime.GetValue();
    }

    public float GetMaxChargeTime()
    {
        return maxChargeTime.GetValue();
    }

    public float GetWhirlWindCooldown()
    {
        return whirlwindCoolDown.GetValue();
    }
}