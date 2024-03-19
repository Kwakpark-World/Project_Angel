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
    public Stat maxHealth; // �ִ� ü��
    public Stat currentHealth; // ���� ü��
    public Stat defensivePower; // ����
    public Stat moveSpeed; // �̵� �ӵ�

    [Header("Offensive stats")]
    public Stat attackPower; // ���ݷ�
    public Stat criticalChance; // ġ��Ÿ Ȯ��
    public Stat criticalMultiplier; // ġ��Ÿ ���� 

    protected PlayerController _owner;

    protected Dictionary<PlayerStatType, FieldInfo> _fieldInfoDictionary = new Dictionary<PlayerStatType, FieldInfo>();

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
}