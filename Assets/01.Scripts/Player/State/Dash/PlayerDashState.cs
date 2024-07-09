using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerState
{
    public PlayerDashState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        CameraManager.Instance._currentCam.IsCamRotateStop = true;
        _player.IsDefense = true;
    }

    public override void Exit()
    {
        base.Exit();
        CameraManager.Instance._currentCam.IsCamRotateStop = false;
        _player.IsDefense = false;
    }

    public override void UpdateState()
    {
        base.UpdateState();
        if (_player.IsPlayerStop == PlayerControlEnum.Stop)
            return;
    }


}
