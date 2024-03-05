using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public enum PlayerStatType
{
    maxHealth,
    currentHealth,
    armor,
    speed,
    damage,
    criticalChance,
    criticalDamage,
}

public class CharacterStat : ScriptableObject
{
    [Header("Defensive stats")]
    public Stat maxHealth; //체력
    public Stat currentHealth;
    public Stat armor; //방어도
    public Stat speed;

    [Header("Offensive stats")]
    public Stat damage;
    public Stat criticalChance;
    public Stat criticalDamage;

    protected Entity _owner;

    protected Dictionary<PlayerStatType, FieldInfo> _fieldInfoDictionary = new Dictionary<PlayerStatType, FieldInfo>();

    public virtual void SetOwner(Entity owner)
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

    public float GetDamage()
    {
        return damage.GetValue();
    }

    public float ArmoredDamage(float incomingDamage, bool isChilled)
    {
        return 0;
    }

    public float GetMaxHealthValue()
    {
        return 0;
    }
}