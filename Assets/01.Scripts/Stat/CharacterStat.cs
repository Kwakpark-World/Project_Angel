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
    public Stat maxHealth; // 최대 체력
    public Stat defensivePower; // 방어력
    public Stat defenseCooldown; // 방어 쿨다운

    [Header("Offensive stats")]
    public Stat attackPower; // 공격력
    public Stat attackSpeed; // 공격 속도
    public Stat criticalChance; // 치명타 확률
    public Stat criticalMultiplier; // 치명타 배율 
    public Stat chargingAttackSpeed; // 차징 공격 속도 배율
    public Stat chargingAttackDistance; // 차징 공격 찌르기 이동거리 배율

    [Header("Move stats")]
    public Stat moveSpeed; // 이동 속도
    public Stat rotateSpeed; // 회전 속도
    public Stat dashSpeed; // 대시 속도
    public Stat dashDuration; // 대시 지속시간
    public Stat dashCooldown; // 대시 쿨다운

    [Header("Skill stats")]
    public Stat qSkillCooldown; // Q 스킬 쿨다운
    public Stat maxAwakenGauge; // 최대 각성 게이지

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