using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerState
{
    private float _dashStartTime;
    private Vector3 _dashDirection;

    //private SkinnedMeshRenderer _skinnedMeshRenderer;

    public PlayerDashState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
        //_skinnedMeshRenderer = _player.transform.GetComponentInChildren<SkinnedMeshRenderer>();
    }

    public override void Enter()
    {
        base.Enter();

        _dashStartTime = Time.time;
        _player.RotateToMousePos();
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
        
        if (_dashStartTime + _player.dashDuration <= Time.time && _endTriggerCalled)
        {
            _stateMachine.ChangeState(PlayerStateEnum.Idle);
        }
    }


}
