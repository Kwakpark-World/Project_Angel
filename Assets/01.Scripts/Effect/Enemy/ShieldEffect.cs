using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldEffect : PoolableMonoEffect
{
    float time;
    bool isDownEffect;

    public override void InitializePoolItem()
    {
        base.InitializePoolItem();

        PoolManager.Instance.Push(this, duration, true);
    }

    protected override void Update()
    {
        base.Update();
    }

    public override void RegisterEffect()
    {
        base.RegisterEffect();
    }
}
