using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChargeStabAttackState : PlayerAttackState
{
    private bool _isStabMove;

    public PlayerChargeStabAttackState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        _isStabMove = false;
    }

    public override void Exit()
    {
        base.Exit();
        _isStabMove = false;

        _player.ChargingGauge = 0;
        _player.AnimatorCompo.speed = 1;
    }

    public override void UpdateState()
    {
        base.UpdateState();

        if (_actionTriggerCalled && !_isStabMove)
        {
            _isStabMove = true;

            float stabDistance = _player.ChargingGauge * _player.ChargingAttackStabDistance;
            _player.SetVelocity(_player.transform.forward * stabDistance);
        }

        if (_endTriggerCalled)
        {
            _stateMachine.ChangeState(PlayerStateEnum.Idle);
        }
    }
}
