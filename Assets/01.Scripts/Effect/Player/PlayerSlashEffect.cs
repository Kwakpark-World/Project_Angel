using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSlashEffect : PoolableMonoEffect
{
    public override void InitializePoolingItem()
    {
        base.InitializePoolingItem();
    }

    protected override void Update()
    {
        base.Update();

        transform.position = GameManager.Instance.player._currentWeapon.transform.position;
    }

    public override void RegisterEffect()
    {
        base.RegisterEffect();
    }


}
