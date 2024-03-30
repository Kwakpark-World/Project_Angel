using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEChargeAttackEffect : PoolableMonoEffect
{
    float time;
    ParticleSystem particle;

    public override void InitializePoolingItem()
    {
        base.InitializePoolingItem();

        if (!GameManager.Instance.player.IsAwakening)
        {
            PoolManager.Instance.Push(this);
        }

        PoolManager.Instance.Push(this, 4);

        Quaternion rot = GameManager.Instance.player.transform.rotation;
        rot.x = 0;
        rot.z = 0;

        transform.rotation = rot;
        transform.localScale = Vector3.zero;

        time = 0;
        particle = GetComponent<ParticleSystem>();
        particle.Pause();
    }

    protected override void Update()
    {
        base.Update();

        if (GameManager.Instance.player.PlayerInput.isCharge)
        {
            if (GameManager.Instance.player.ChargingGage > 0.5f)
            {
                transform.localScale = Vector3.one * GameManager.Instance.player.ChargingGage * 0.5f;
            }
        }
        else
        {
            if (GameManager.Instance.player.ChargingGage < 0.5f)
            {
                PoolManager.Instance.Push(this);
                return;
            }

            particle.Play();

            PoolManager.Instance.Push(this, 3);
        }
    }

    public override void RegisterEffect()
    {
        base.RegisterEffect();
    }
}
