using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class PlayerDashState : PlayerState
{

    public PlayerDashState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        _player.PlayerInput.DashEvent += HandleRollOnceMoreEvent;
        
        CameraManager.Instance._currentCam.IsCamRotateStop = true;

        _player.IsDefense = true;
    }

    public override void Exit()
    {
        base.Exit();
        _player.PlayerInput.DashEvent -= HandleRollOnceMoreEvent;

        CameraManager.Instance._currentCam.IsCamRotateStop = false;
        _player.IsDefense = false;

    }

    public override void UpdateState()
    {
        base.UpdateState();
        if (_player.IsPlayerStop == PlayerControlEnum.Stop)
            return;

        _player.AnimatorCompo.SetBool("RollOnceMore", _player.isOnRollOnceMore);
    }

    private void HandleRollOnceMoreEvent()
    {
        if (!_player.isRollOnceMore) return;
        if (_player.isOnRollOnceMore) return;
        if (!_actionTriggerCalled) return;

        _player.isOnRollOnceMore = true;
        _player.dashPrevTime = Time.time;

        if (!_player.isRollToDash)
            _player.StateMachine.ChangeState(PlayerStateEnum.NormalDash);
        else
            _player.StateMachine.ChangeState(PlayerStateEnum.AwakenDash);
    }

}
