using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDieState : PlayerState
{
    
    public PlayerDieState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _player.StopImmediately(false);
        _player.IsDie = true;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void UpdateState()
    {
        base.UpdateState();

        if (!_player.IsDie)
        {
            _stateMachine.ChangeState(PlayerStateEnum.Idle);
        }
    }
}
