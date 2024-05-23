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

    public override void InitializePoolItem()
    {
        base.InitializePoolItem();
    }

    protected override void Initialize()
    {
        base.Initialize();
    }

    public override void OnDie()
    {
        base.OnDie();

        Debuff();
    }
}
