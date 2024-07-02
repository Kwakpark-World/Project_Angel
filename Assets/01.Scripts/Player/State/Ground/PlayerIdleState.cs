using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerGroundState
{
    public PlayerIdleState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();
        _player.StopImmediately(false);
    }

    public override void Exit()
    {
        
        base.Exit();
    }

    public override void UpdateState()
    {
        base.UpdateState();

        _player.SetVelocity(Vector3.zero);
             
        //x ������ ���� ���ȴٸ� �̵����·� �������ָ� ��.
        float xInput = _player.PlayerInput.XInput;
        float yInput = _player.PlayerInput.YInput;

        if (Mathf.Abs(xInput) > 0.05f || Mathf.Abs(yInput) > 0.05f)
        {
            _stateMachine.ChangeState(PlayerStateEnum.Walk);
        }
    }
}
