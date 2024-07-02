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

        Vector3 moveVel = Vector3.zero;
        moveVel.y = _player.RigidbodyCompo.velocity.y;

        _player.SetVelocity(moveVel);
             
        //x ������ ���� ���ȴٸ� �̵����·� �������ָ� ��.
        float xInput = _player.PlayerInput.XInput;
        float yInput = _player.PlayerInput.YInput;

        if (Mathf.Abs(xInput) > 0.05f || Mathf.Abs(yInput) > 0.05f)
        {
            _stateMachine.ChangeState(PlayerStateEnum.Walk);
        }
    }
}
