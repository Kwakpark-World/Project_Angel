using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNormalDashState : PlayerDashState
{
    private Vector3 _dashDirection;

    public PlayerNormalDashState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
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

        _player.SetVelocity(_dashDirection * _player.PlayerStatData.GetDashSpeed());

        if (_endTriggerCalled)
        {
            _stateMachine.ChangeState(PlayerStateEnum.Idle);
        }
    }


}
