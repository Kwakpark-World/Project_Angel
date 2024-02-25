using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Satanael : Grigori
{
    private BossBrain _azazel;

    protected override void OnEnable()
    {
        base.OnEnable();

        _azazel = GameObject.Find("Azazel").GetComponent<BossBrain>();
    }

    protected override void Attack()
    {

    }

    protected override void Debuff()
    {
        if (player.playerCurrnetHP / player.PlayerStat.GetStatByType(StatType.maxHealth).GetValue() > _azazel.CurrentHitPoint / _azazel.hitPoint)
        {
            player.playerCurrnetHP = player.PlayerStat.GetStatByType(StatType.maxHealth).GetValue() * _azazel.CurrentHitPoint / _azazel.hitPoint;
        }
        else
        {
            _azazel.CurrentHitPoint = _azazel.hitPoint * player.playerCurrnetHP / player.PlayerStat.GetStatByType(StatType.maxHealth).GetValue();
        }
    }
}
