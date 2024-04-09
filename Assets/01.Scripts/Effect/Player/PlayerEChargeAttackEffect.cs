using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class PlayerEChargeAttackEffect : PoolableMonoEffect
{
    ParticleSystem particle;
    BoxCollider boxCol;


    public override void InitializePoolingItem()
    {
        base.InitializePoolingItem();

        if (!GameManager.Instance.PlayerInstance.IsAwakening)
        {
            PoolManager.Instance.Push(this);
        }

        PoolManager.Instance.Push(this, 4);

        Quaternion rot = GameManager.Instance.PlayerInstance.transform.rotation;
        rot.x = 0;
        rot.z = 0;

        transform.rotation = rot;
        transform.localScale = Vector3.zero;

        particle = GetComponent<ParticleSystem>();
        particle.Pause();

        boxCol = GetComponent<BoxCollider>();

        boxCol.enabled = false;
    }

    protected override void Update()
    {
        base.Update();

        if (GameManager.Instance.PlayerInstance.PlayerInput.isCharge)
        {
            if (GameManager.Instance.PlayerInstance.ChargingGage > 0.5f)
            {
                transform.localScale = Vector3.one * GameManager.Instance.PlayerInstance.ChargingGage * 0.5f;
            }
        }
        else
        {
            if (GameManager.Instance.PlayerInstance.ChargingGage < 0.5f)
            {
                PoolManager.Instance.Push(this);
                return;
            }

            particle.Play();
            boxCol.enabled = true;

            PoolManager.Instance.Push(this, 3);
        }

        if (GameManager.Instance.PlayerInstance.StateMachine.CurrentState == GameManager.Instance.PlayerInstance.StateMachine.GetState(PlayerStateEnum.EDash))
        {
            PoolManager.Instance.Push(this);
        }
    }

    public override void RegisterEffect()
    {
        base.RegisterEffect();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (LayerMask.LayerToName(other.gameObject.layer) == "Enemy")
        {
            if (other.gameObject.TryGetComponent<Brain>(out Brain brain))
            {
                brain.OnHit(GameManager.Instance.PlayerInstance.attackPower);
            }
        }
    }
}
