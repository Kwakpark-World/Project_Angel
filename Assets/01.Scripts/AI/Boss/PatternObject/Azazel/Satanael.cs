using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Satanael : Grigori
{
    protected override void Attack()
    {

    }

    protected override void Debuff()
    {
        float playerHitPointPercentage = player.PlayerStat.GetCurrentHealth() / player.PlayerStat.GetMaxHealthValue();
        float azazelHitPointPercentage = owner.EnemyStatistic.GetCurrentHealth() / owner.EnemyStatistic.GetMaxHealthValue();

        if (playerHitPointPercentage > azazelHitPointPercentage)
        {
            player.PlayerStat.currentHealth.AddModifier(-(player.PlayerStat.GetMaxHealthValue() * (playerHitPointPercentage - azazelHitPointPercentage)));
        }
        else
        {
            owner.EnemyStatistic.currentHealth.AddModifier(-(owner.EnemyStatistic.GetMaxHealthValue() * (azazelHitPointPercentage - playerHitPointPercentage)));
        }
    }
}
