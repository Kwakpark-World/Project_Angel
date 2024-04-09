using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEQSkillEffect : PoolableMonoEffect
{
    public override void InitializePoolingItem()
    {
        base.InitializePoolingItem();

        if (!GameManager.Instance.PlayerInstance.IsAwakening)
        {
            PoolManager.Instance.Push(this);
        }

        PoolManager.Instance.Push(this, 4);
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
