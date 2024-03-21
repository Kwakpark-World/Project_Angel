using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Satanael : Grigori
{
    protected override void Debuff()
    {
        float playerHitPointPercentage = GameManager.Instance.player.PlayerStatData.GetCurrentHealth() / GameManager.Instance.player.PlayerStatData.GetMaxHealthValue();
        float azazelHitPointPercentage = owner.EnemyStatData.GetCurrentHealth() / owner.EnemyStatData.GetMaxHealthValue();

        if (playerHitPointPercentage > azazelHitPointPercentage)
        {
            GameManager.Instance.player.PlayerStatData.currentHealth.AddModifier(-(GameManager.Instance.player.PlayerStatData.GetMaxHealthValue() * (playerHitPointPercentage - azazelHitPointPercentage)));
        }
        else
        {
            owner.EnemyStatData.currentHealth.AddModifier(-(owner.EnemyStatData.GetMaxHealthValue() * (azazelHitPointPercentage - playerHitPointPercentage)));
        }
    }
}
