using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNormalSlamSkillEffect : PlayerEffect
{
    public override void InitializePoolItem()
    {
        base.InitializePoolItem();
        
        if (GameManager.Instance.PlayerInstance.IsAwakening)
        {
            PoolManager.Instance.Push(this);
        }

        PoolManager.Instance.Push(this, duration);

        Vector3 dir = Vector3.zero;
        dir.x = -90f; // default Effect angle
        dir.z = _player.transform.eulerAngles.y; // x가 돌아가서 이펙트의 Y회전이 아닌 Z회전을 해줘야됨

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
