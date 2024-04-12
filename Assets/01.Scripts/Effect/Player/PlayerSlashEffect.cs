using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSlashEffect : PoolableMonoEffect
{
    ParticleSystem particle;

    public override void InitializePoolingItem()
    {
        base.InitializePoolingItem();

        particle = transform.GetComponentInChildren<ParticleSystem>();
    }

    protected override void Update()
    {
        base.Update();

        transform.position = GameManager.Instance.PlayerInstance._weapon.transform.position;

        if (GameManager.Instance.PlayerInstance.StateMachine.CurrentState == GameManager.Instance.PlayerInstance.StateMachine.GetState(PlayerStateEnum.Idle))
        {
            particle.Clear();
            PoolManager.Instance.Push(this);
        }
    }

    public override void RegisterEffect()
    {
        base.RegisterEffect();
    }


}
