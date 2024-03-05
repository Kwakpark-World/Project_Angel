using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDefenseState : PlayerState
{
    public PlayerDefenseState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _player.IsDefense = true;

    }

    public override void Exit()
    {
        base.Exit();
        _player.IsDefense = false;

    }

    public override void UpdateState()
    {
        base.UpdateState();

        if (!_player.PlayerInput.isDefense)
            _stateMachine.ChangeState(PlayerStateEnum.Idle);
        

    }
}
