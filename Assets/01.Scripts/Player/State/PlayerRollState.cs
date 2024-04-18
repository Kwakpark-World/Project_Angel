using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRollState : PlayerDashState
{
    private Vector3 _dashDirection;

    public PlayerRollState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void UpdateState()
    {
        base.UpdateState();

        _dashDirection = _player.transform.forward;

        _player.SetVelocity(_dashDirection * _player.dashSpeed);
    }


}
