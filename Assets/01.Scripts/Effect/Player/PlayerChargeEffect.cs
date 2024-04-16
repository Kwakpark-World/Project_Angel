using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChargeEffect : PoolableMonoEffect
{
    public override void InitializePoolingItem()
    {
        base.InitializePoolingItem();

        Vector3 scale = Vector3.one;
        transform.localScale = scale * 0.5f;
    }

    protected override void Update()
    {
        base.Update();

        if (!GameManager.Instance.PlayerInstance.PlayerInput.isCharge)
        {
            PoolManager.Instance.Push(this);
        }
        else
            transform.position = GameManager.Instance.PlayerInstance._weapon.transform.Find("RightPointTop").position;
    }

    public override void RegisterEffect()
    {
        base.RegisterEffect();
    }
}
