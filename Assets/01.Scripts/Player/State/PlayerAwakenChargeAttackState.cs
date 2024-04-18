using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAwakenChargeAttackState : PlayerChargeState
{
    public PlayerAwakenChargeAttackState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

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

        if (_endTriggerCalled)
        {
            _stateMachine.ChangeState(PlayerStateEnum.Idle);
        }
    }
}
