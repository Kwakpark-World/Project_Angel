using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Grigori : Brain
{
    [HideInInspector]
    public BossBrain owner = null;

    protected override void Update()
    {
        base.Update();
    }

    protected abstract void Debuff();

    public override void InitializePoolingItem()
    {
        base.InitializePoolingItem();
    }

    protected override void Initialize()
    {
        base.Initialize();
    }

    public override void OnHit()
    {
        if (EnemyStatData.GetCurrentHealth() <= 0f)
        {
            OnDie();
        }
    }

    public override void OnDie()
    {
        Debuff();
        PoolManager.Instance.Push(this);
    }
}
