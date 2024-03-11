using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shamshiel : Grigori
{
    [SerializeField]
    private float _slownessMultiplier = 0.75f;
    [SerializeField]
    private float _slownessDuration = 5f;

    protected override void Attack()
    {

    }

    protected override void Debuff()
    {
        // Give slowness effect to player.
        // Debug
        player.moveSpeed *= _slownessMultiplier;

        //player.CharStat.IncreaseStatBy(player.moveSpeed * slown)
    }
}
