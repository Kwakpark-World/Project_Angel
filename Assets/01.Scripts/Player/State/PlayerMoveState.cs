using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerGroundState
{
    public PlayerMoveState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
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

        float xInput = _player.PlayerInput.XInput;
        float yInput = _player.PlayerInput.YInput;

        Vector3 moveDir = new Vector3(xInput, 0, yInput).normalized;

        moveDir = Quaternion.Euler(0, 45, 0) * moveDir;
        moveDir *= _player.moveSpeed;

        if (moveDir.sqrMagnitude > 0)
        {
            //_player.transform.rotation = Quaternion.LookRotation(moveDir);
            _player.transform.rotation = Quaternion.Lerp(_player.transform.rotation, Quaternion.LookRotation(moveDir), Time.deltaTime * _player.rotationSpeed);
        }

        moveDir.y = _rigidbody.velocity.y;

        _player.SetVelocity(moveDir);

        if (Mathf.Abs(xInput) < 0.05f && Mathf.Abs(yInput) < 0.05f)
        {
            _stateMachine.ChangeState(PlayerStateEnum.Idle);
        }
    }
}
