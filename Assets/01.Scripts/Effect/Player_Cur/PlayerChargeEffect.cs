using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChargeEffect : PlayerEffect
{
    public override void InitializePoolingItem()
    {
        base.InitializePoolingItem();

        Vector3 scale = Vector3.one;
        Quaternion rot = Quaternion.identity;
        rot.y = _player.transform.rotation.y;

        transform.localScale = scale;
        transform.rotation = rot;
    }

    protected override void Update()
    {
        base.Update();

        if (!_player.PlayerInput.isCharge)
        {
            PoolManager.Instance.Push(this);
        }
        else
            transform.position = _player._weapon.transform.Find("RightPointTop").position;
    }

    public override void RegisterEffect()
    {
        base.RegisterEffect();
    }
}
