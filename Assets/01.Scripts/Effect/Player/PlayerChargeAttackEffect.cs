using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChargeAttackEffect : PoolableMonoEffect
{
    float time;
    ParticleSystem particle;

    public override void InitializePoolingItem()
    {
        base.InitializePoolingItem();

        if (GameManager.Instance.player.IsAwakening)
        {
            PoolManager.Instance.Push(this);
            return;
        }
            

        Quaternion rot = GameManager.Instance.player.transform.rotation;
        rot.x = 0;
        rot.z = 0;

        transform.rotation = rot;

        particle = GetComponentInChildren<ParticleSystem>();

        time = 0;
        transform.localScale = Vector3.zero;
    }

    protected override void Update()
    {
        base.Update();
        

        if (GameManager.Instance.player.PlayerInput.isCharge)
        {
            if (time > 0.3f)
                particle.Pause();
            else
                time += Time.deltaTime;

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
            }

            if (time < 0.3f) return;
            time = 0;
            
            particle.Play();

            PoolManager.Instance.Push(this, 3);
        }
        
    }

    public override void RegisterEffect()
    {
        base.RegisterEffect();
    }

}
