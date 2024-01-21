using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerState
{
    private float _dashStartTime;
    private float _dashDirection;

    //private SkinnedMeshRenderer _skinnedMeshRenderer;

    public PlayerDashState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
        //_skinnedMeshRenderer = _player.transform.GetComponentInChildren<SkinnedMeshRenderer>();
    }

    public override void Enter()
    {
        base.Enter();

        float xInput = _player.PlayerInput.XInput;
        _dashDirection = Mathf.Abs(xInput) > 0.05f ? xInput : _player.FacingDirection;
        _dashStartTime = Time.time;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void UpdateState()
    {
        base.UpdateState();
        _player.SetVelocity(_player.dashSpeed * _dashDirection, 0, 0);

        if (_dashStartTime + _player.dashDuration <= Time.time)
        {
            _stateMachine.ChangeState(PlayerStateEnum.Idle);
        }
    }


}
