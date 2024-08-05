using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNormalSlamSkillEffect : PlayerEffect
{
    public override void InitializePoolItem()
    {
        base.InitializePoolItem();

        PoolManager.Instance.Push(this, duration);

        Vector3 dir = Vector3.zero;
        dir.x = -90f; // default Effect angle
        dir.z = _player.transform.eulerAngles.y; // x�� ���ư��� ����Ʈ�� Yȸ���� �ƴ� Zȸ���� ����ߵ�?

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
