using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAwakenChargeAttackState : PlayerChargeState
{
    private bool _isEffectOn = false;

    public PlayerAwakenChargeAttackState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _isEffectOn = false;

        _player.AnimatorCompo.speed = 1 + (_player.ChargingGauge / (_maxChargeTime * 10)) * _player.PlayerStatData.GetChargingAttackSpeed();

    }

    public override void Exit()
    {
        base.Exit();

        _player.AnimatorCompo.speed = 1;
    }

    public override void UpdateState()
    {
        base.UpdateState();

        if (_effectTriggerCalled)
        {
            if (!_isEffectOn)
            {
                _isEffectOn = true;
                ChargeAttackEffect();
            }
        }

        if (_isHitAbleTriggerCalled)
        {
            ChargeAttack();
        }

        if (_endTriggerCalled)
        {
            _stateMachine.ChangeState(PlayerStateEnum.Idle);
        }
    }

    protected override void SetAttackSetting()
    {

    }

    private void ChargeAttackEffect()
    {
        Vector3 pos = _player.transform.position;
        EffectManager.Instance.PlayEffect(PoolingType.Effect_PlayerAttack_Charged_Awaken, pos);
    }

    private void ChargeAttack()
    {

    }
}
