using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEChargeEffect : PoolableMonoEffect
{
    public override void InitializePoolingItem()
    {
        base.InitializePoolingItem();

        Vector3 scale = Vector3.one;
        transform.localScale = scale * 0.5f;
    }

    public override void RegisterEffect()
    {
        base.RegisterEffect();

        
    }

    protected override void Update()
    {
        base.Update();

        if (!GameManager.Instance.player.PlayerInput.isCharge)
        {
            PoolManager.Instance.Push(this);
        }
        else
            transform.position = GameManager.Instance.player._currentWeapon.transform.Find("Point").position;
    }
}
