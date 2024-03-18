using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shamshiel : Grigori
{
    [SerializeField]
    private float _slownessMultiplier = 0.75f;
    [SerializeField]
    private float _slownessDuration = 5f;

    protected override void Debuff()
    {
        GameManager.Instance.player.PlayerStat.IncreaseStatBy(GameManager.Instance.player.PlayerStat.GetMoveSpeed() - GameManager.Instance.player.PlayerStat.GetMoveSpeed() * _slownessMultiplier, _slownessDuration, GameManager.Instance.player.PlayerStat.GetStatByType(PlayerStatType.moveSpeed));
    }
}
