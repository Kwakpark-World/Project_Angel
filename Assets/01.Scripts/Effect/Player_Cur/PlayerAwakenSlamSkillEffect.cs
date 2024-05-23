using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAwakenSlamSkillEffect : PlayerEffect
{
    public override void InitializePoolItem()
    {
        base.InitializePoolItem();
        
        if (!GameManager.Instance.PlayerInstance.IsAwakening)
        {
            PoolManager.Instance.Push(this);
        }

        PoolManager.Instance.Push(this, duration);

        Vector3 dir = Vector3.zero;
        dir.y = _player.transform.eulerAngles.y; 

        Quaternion rot = Quaternion.Euler(dir);

        transform.rotation = rot;
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
