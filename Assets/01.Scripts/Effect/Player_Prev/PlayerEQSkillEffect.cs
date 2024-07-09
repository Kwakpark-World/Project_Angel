using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEQSkillEffect : PoolableMonoEffect
{
    public override void InitializePoolItem()
    {
        base.InitializePoolItem();

        if (!GameManager.Instance.PlayerInstance.IsAwakened)
        {
            PoolManager.Instance.Push(this);
        }

        PoolManager.Instance.Push(this, 4);
    }

    protected override void Update()
    {
        Debug.Log("#");
        base.Update();
    }

    public override void RegisterEffect()
    {
        base.RegisterEffect();
    }
}
