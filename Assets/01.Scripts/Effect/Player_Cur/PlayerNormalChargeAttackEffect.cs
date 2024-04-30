using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNormalChargeAttackEffect : PlayerEffect
{
    Quaternion _defaultRot;

    public override void InitializePoolingItem()
    {
        base.InitializePoolingItem();

        // 이 이펙트는 각성 상태 이펙트가 아님
        if (_player.IsAwakening)
        {
            PoolManager.Instance.Push(this);
            return;
        }

        transform.parent = _player.transform;
        transform.localRotation = _defaultRot;
    }

    protected override void Update()
    {
        base.Update();

        // 이 상태가 아니면 없애는 것
        if (!_player.StateMachine.CompareState(_playerState))
            PoolManager.Instance.Push(this, 0.5f, true);


        // 대쉬하면 공격 멈추는 거임
        if (_player.StateMachine.CompareState(PlayerStateEnum.NormalDash))
            PoolManager.Instance.Push(this, true);
        
    }

    public override void RegisterEffect()
    {
        base.RegisterEffect();
        _defaultRot = transform.rotation;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (LayerMask.LayerToName(other.gameObject.layer) == "Enemy")
        {
            if(other.gameObject.TryGetComponent<Brain>(out Brain brain))
            {
                brain.OnHit(_player.PlayerStatData.GetAttackPower());
            }
        }
    }
}
