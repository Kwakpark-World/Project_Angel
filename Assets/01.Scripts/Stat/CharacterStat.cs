using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public enum PlayerStatType
{
    maxHealth,
    defensivePower,
    defenseCooldown,
    attackPower,
    attackSpeed,
    criticalChance,
    criticalMultiplier,
    moveSpeed,
    rotateSpeed,
    dashSpeed,
    dashDuration,
    dashCooldown,
    qSkillCooldown,
    maxAwakenGauge,
    chargingAttackSpeed,
    chargingAttackDistance
}

public class CharacterStat : ScriptableObject
{
    [Header("Defensive stats")]
    public Stat maxHealth; // �ִ� ü��
    public Stat defensivePower; // ����
    public Stat defenseCooldown; // ��� ��ٿ�

    [Header("Offensive stats")]
    public Stat attackPower; // ���ݷ�
    public Stat attackSpeed; // ���� �ӵ�
    public Stat criticalChance; // ġ��Ÿ Ȯ��
    public Stat criticalMultiplier; // ġ��Ÿ ���� 
    public Stat chargingAttackSpeed; // ��¡ ���� �ӵ� ����
    public Stat chargingAttackDistance; // ��¡ ���� ��� �̵��Ÿ� ����

    [Header("Move stats")]
    public Stat moveSpeed; // �̵� �ӵ�
    public Stat rotateSpeed; // ȸ�� �ӵ�
    public Stat dashSpeed; // ��� �ӵ�
    public Stat dashDuration; // ��� ���ӽð�
    public Stat dashCooldown; // ��� ��ٿ�

    [Header("Skill stats")]
    public Stat qSkillCooldown; // Q ��ų ��ٿ�
    public Stat maxAwakenGauge; // �ִ� ���� ������

    protected PlayerController owner;

    protected Dictionary<PlayerStatType, FieldInfo> fieldInfoDictionary = new Dictionary<PlayerStatType, FieldInfo>();

    public virtual void SetOwner(PlayerController owner)
    {
        this.owner = owner;
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

    public float GetCriticalMultiplier()
    {
        return criticalMultiplier.GetValue();
    }

    public float GetMoveSpeed()
    {
        return moveSpeed.GetValue();
    }

    public float GetRotateSpeed()
    {
        return rotateSpeed.GetValue();
    }

    public float GetDashSpeed()
    {
        return dashSpeed.GetValue();
    }

    public float GetDashDuration()
    {
        return dashDuration.GetValue();
    }

    public float GetDashCooldown()
    {
        return dashCooldown.GetValue();
    }

    public float GetQSkillCooldown()
    {
        return qSkillCooldown.GetValue();
    }

    public float GetMaxAwakenGauge()
    {
        return maxAwakenGauge.GetValue();
    }

    public float GetChargingAttackSpeed()
    {
        return chargingAttackSpeed.GetValue();
    }

    public float GetChargingAttackDistance()
    {
        return chargingAttackDistance.GetValue();
    }
}