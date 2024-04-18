using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class PlayerWalkState : PlayerGroundState
{
    public PlayerWalkState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
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

        Vector3 moveDir = SetDirection(xInput, yInput);

        _player.SetVelocity(moveDir);

        if (Mathf.Abs(xInput) < 0.05f && Mathf.Abs(yInput) < 0.05f)
        {
            _stateMachine.ChangeState(PlayerStateEnum.Idle);
        }
    }

    private Vector3 SetDirection(float xInput, float yInput)
    {
        Vector3 moveDir = new Vector3(xInput, 0, yInput).normalized;

        moveDir = (Quaternion.Euler(0, CameraManager.Instance.GetCameraByType(CameraType.PlayerCam).transform.eulerAngles.y, 0) * moveDir).normalized;
        moveDir *= _player.PlayerStatData.GetMoveSpeed();

        if (moveDir.sqrMagnitude > 0)
        {
            _player.transform.rotation = Quaternion.Lerp(_player.transform.rotation, Quaternion.LookRotation(moveDir), Time.deltaTime * _player.PlayerStatData.GetRotateSpeed());
        }

        moveDir.y = _rigidbody.velocity.y;
        return moveDir;
    }
}
