using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Satanael : Grigori
{
    protected override void Debuff()
    {
        float playerHitPointPercentage = GameManager.Instance.player.PlayerStat.GetCurrentHealth() / GameManager.Instance.player.PlayerStat.GetMaxHealthValue();
        float azazelHitPointPercentage = owner.EnemyStatistic.GetCurrentHealth() / owner.EnemyStatistic.GetMaxHealthValue();

        if (playerHitPointPercentage > azazelHitPointPercentage)
        {
            GameManager.Instance.player.PlayerStat.currentHealth.AddModifier(-(GameManager.Instance.player.PlayerStat.GetMaxHealthValue() * (playerHitPointPercentage - azazelHitPointPercentage)));
        }
        else
        {
            owner.EnemyStatistic.currentHealth.AddModifier(-(owner.EnemyStatistic.GetMaxHealthValue() * (azazelHitPointPercentage - playerHitPointPercentage)));
        }
    }
}
