using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEChargeAttackState : PlayerAttackState
{
    public PlayerEChargeAttackState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        _player.AnimatorCompo.speed = 1 + (_player.ChargingGauge / 20) * _player.ChargingAttackSpeed;

    }

    public override void Exit()
    {
        base.Exit();

        _player.AnimatorCompo.speed = 1;
    }

    public override void UpdateState()
    {
        base.UpdateState();

        if (_endTriggerCalled)
        {
            _stateMachine.ChangeState(PlayerStateEnum.Idle);
        }
    }
}
