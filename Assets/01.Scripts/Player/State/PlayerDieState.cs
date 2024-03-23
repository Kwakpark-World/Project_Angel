using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDieState : PlayerState
{
    private bool _isCollider;

    public PlayerDieState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _player.IsDie = true;
        _isCollider = false;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void UpdateState()
    {
        base.UpdateState();

        if (_isCollider) return;
        if (_actionTriggerCalled)
        {
            _isCollider = true;
        }

        if (!_player.IsDie)
        {
            _stateMachine.ChangeState(PlayerStateEnum.Idle);
        }
    }
}
