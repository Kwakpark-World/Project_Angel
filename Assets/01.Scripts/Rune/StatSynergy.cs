using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StatSynergy : RuneManager
{
    public BuffType buffType;
    private List<BuffType> activeBuffs;

    public void BuffManager()
    {
        activeBuffs = new List<BuffType>();
    }

    public void AddBuff(BuffType buff)
    {
        activeBuffs.Add(buff);
        UpdateAccelration();
        UpdateAttack();
        UpdateDefence();
        UpdateHealth();
    }

    public void RemoveBuff(BuffType buff)
    {
        activeBuffs.Remove(buff);
        UpdateAccelration();
        UpdateAttack();
        UpdateDefence();
        UpdateHealth();
    }

    private void UpdateAccelration()
    {
        int accelerationCount = activeBuffs.Count(buff => buff == BuffType.Rune_Synergy_Acceleration);

        if(accelerationCount >= 3) 
        {
            GameManager.Instance.PlayerInstance.PlayerStatData.attackSpeed.AddModifier(2f);
            GameManager.Instance.PlayerInstance.PlayerStatData.moveSpeed.AddModifier(2f);

            GameManager.Instance.PlayerInstance.PlayerStatData.maxHealth.RemoveModifier(5f);
        }
        else
        {
            GameManager.Instance.PlayerInstance.PlayerStatData.attackSpeed.RemoveModifier(2f);
            GameManager.Instance.PlayerInstance.PlayerStatData.moveSpeed.RemoveModifier(2f);

            GameManager.Instance.PlayerInstance.PlayerStatData.maxHealth.AddModifier(5f);
        }
    }

    private void UpdateAttack()
    {
        int AttackCount = activeBuffs.Count(buff => buff == BuffType.Rune_Synergy_Attack);

        if (AttackCount >= 3)
        {
            GameManager.Instance.PlayerInstance.PlayerStatData.attackPower.AddModifier(3f);

            GameManager.Instance.PlayerInstance.PlayerStatData.defensivePower.RemoveModifier(3f);
        }
        else
        {
            GameManager.Instance.PlayerInstance.PlayerStatData.attackPower.RemoveModifier(3f);

            GameManager.Instance.PlayerInstance.PlayerStatData.defensivePower.AddModifier(3f);
        }
    }

    private void UpdateDefence()
    {
        int DefenceCount = activeBuffs.Count(buff => buff == BuffType.Rune_Synergy_Defense);

        if (DefenceCount >= 3)
        {
            GameManager.Instance.PlayerInstance.PlayerStatData.attackSpeed.RemoveModifier(2f);
            GameManager.Instance.PlayerInstance.PlayerStatData.moveSpeed.RemoveModifier(2f);

            GameManager.Instance.PlayerInstance.PlayerStatData.maxHealth.AddModifier(5f);
        }
        else
        {
            GameManager.Instance.PlayerInstance.PlayerStatData.attackSpeed.AddModifier(2f);
            GameManager.Instance.PlayerInstance.PlayerStatData.moveSpeed.AddModifier(2f);

            GameManager.Instance.PlayerInstance.PlayerStatData.maxHealth.RemoveModifier(5f);
        }
    }

    private void UpdateHealth()
    {
        int HealthCount = activeBuffs.Count(buff => buff == BuffType.Rune_Synergy_Health);

        if (HealthCount >= 3)
        {
            GameManager.Instance.PlayerInstance.PlayerStatData.maxHealth.AddModifier(10f);

            GameManager.Instance.PlayerInstance.PlayerStatData.attackPower.RemoveModifier(2f);
        }
        else
        {
            GameManager.Instance.PlayerInstance.PlayerStatData.maxHealth.RemoveModifier(10f);

            GameManager.Instance.PlayerInstance.PlayerStatData.attackPower.AddModifier(2f);
        }
    }
}
