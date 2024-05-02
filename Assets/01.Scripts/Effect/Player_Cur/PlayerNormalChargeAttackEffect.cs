using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNormalChargeAttackEffect : PlayerEffect
{

    public override void InitializePoolingItem()
    {
        base.InitializePoolingItem();

        // �� ����Ʈ�� ���� ���� ����Ʈ�� �ƴ�
        if (_player.IsAwakening)
        {
            PoolManager.Instance.Push(this);
            return;
        }

        // �� ����Ʈ ������ �������
        PoolManager.Instance.Push(this, duration, true);

        transform.parent = _player.transform;
        transform.localRotation = _defaultRot;
    }

    protected override void Update()
    {
        base.Update();

        // �� ���°� �ƴϸ� ���ִ� ��
        if (!_player.StateMachine.CompareState(_playerState))
            PoolManager.Instance.Push(this, 0.5f, true);


        // �뽬�ϸ� ���� ���ߴ� ����
        if (_player.StateMachine.CompareState(PlayerStateEnum.NormalDash))
            PoolManager.Instance.Push(this, true);
        
    }

    public override void RegisterEffect()
    {
        base.RegisterEffect();
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
