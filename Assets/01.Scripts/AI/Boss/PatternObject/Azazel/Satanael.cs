using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Satanael : Grigori
{
    protected override void Debuff()
    {
        float playerHitPointPercentage = GameManager.Instance.player.CurrentHealth / GameManager.Instance.player.PlayerStatData.GetMaxHealth();
        float azazelHitPointPercentage = owner.CurrentHealth / owner.EnemyStatData.GetMaxHealth();

        if (playerHitPointPercentage > azazelHitPointPercentage)
        {
            GameManager.Instance.player.CurrentHealth -= GameManager.Instance.player.PlayerStatData.GetMaxHealth() * (playerHitPointPercentage - azazelHitPointPercentage);
        }
        else
        {
            owner.CurrentHealth -= owner.EnemyStatData.GetMaxHealth() * (azazelHitPointPercentage - playerHitPointPercentage);
        }
    }
}
