using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMeleeAttackEffect : PoolableMonoEffect
{
    public override void InitializePoolingItem()
    {
        base.InitializePoolingItem();

        //

        Quaternion rot = GameManager.Instance.PlayerInstance.transform.rotation;
        rot.x = 0;
        rot.z = 0;

        transform.rotation = rot;

        PoolManager.Instance.Push(this, 2);
    }

    public override void RegisterEffect()
    {
        base.RegisterEffect();
    }
}
