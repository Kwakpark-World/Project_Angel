using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMeleeAttackEffect : PoolAbleMonoEffect
{
    public override void InitializePoolingItem()
    {

    }

    protected override void RegisterEffect()
    {
        base.RegisterEffect();

        EffectManager.Instance.RegisterEffect(EffectType.Particle, this);
    }
}
