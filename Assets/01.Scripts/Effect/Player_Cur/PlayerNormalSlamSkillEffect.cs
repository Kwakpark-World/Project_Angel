using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNormalSlamSkillEffect : PlayerEffect
{
    public override void InitializePoolingItem()
    {
        base.InitializePoolingItem();
        
        if (GameManager.Instance.PlayerInstance.IsAwakening)
        {
            PoolManager.Instance.Push(this);
        }

        PoolManager.Instance.Push(this, duration, true);

        Vector3 dir = Vector3.zero;
        dir.x = -90f; // default Effect angle
        dir.z = _player.transform.eulerAngles.y; // x�� ���ư��� ����Ʈ�� Yȸ���� �ƴ� Zȸ���� ����ߵ�c

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