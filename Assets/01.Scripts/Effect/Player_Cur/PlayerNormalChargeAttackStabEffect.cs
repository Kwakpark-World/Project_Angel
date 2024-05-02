using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNormalChargeAttackStabEffect : PlayerEffect
{
    public override void InitializePoolingItem()
    {
        base.InitializePoolingItem();

        transform.localRotation = Quaternion.LookRotation(-_player.transform.forward);

        // �� ����Ʈ�� ���� ���� ����Ʈ�� �ƴ�
        if (_player.IsAwakening)
        {
            PoolManager.Instance.Push(this);
            return;
        }

        // �� ����Ʈ ������ �������
        PoolManager.Instance.Push(this, duration);

    }

    protected override void Update()
    {
        base.Update();

        // �� ���°� �ƴϸ� ���ִ� ��
        if (!_player.StateMachine.CompareState(_playerState))
            PoolManager.Instance.Push(this, 0.5f);
        

        // �뽬�ϸ� ���� ���ߴ� ����
        if (_player.StateMachine.CompareState(PlayerStateEnum.NormalDash))
            PoolManager.Instance.Push(this);

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
                brain.OnHit(_player.PlayerStatData.GetAttackPower());
            }
        }
    }

}
