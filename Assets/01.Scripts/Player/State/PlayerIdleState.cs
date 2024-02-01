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
        _player.PlayerInput.QSkillEvent += QSkillHandle;
        _player.PlayerInput.ESkillEvent += ESkillHandle;
    }

    public override void Exit()
    {
        _player.PlayerInput.QSkillEvent -= QSkillHandle;
        _player.PlayerInput.ESkillEvent -= ESkillHandle;
        base.Exit();
    }

    public override void UpdateState()
    {
        base.UpdateState();

        //x 축으로 값이 눌렸다면 이동상태로 변경해주면 됨.
        float xInput = _player.PlayerInput.XInput;
        float yInput = _player.PlayerInput.YInput;

        if (Mathf.Abs(xInput) > 0.05f || Mathf.Abs(yInput) > 0.05f)
        {
            _stateMachine.ChangeState(PlayerStateEnum.Move);
        }

    }

    private void ESkillHandle()
    {
        _stateMachine.ChangeState(PlayerStateEnum.ESkill);
    }

    private void QSkillHandle()
    {
        _stateMachine.ChangeState(PlayerStateEnum.QSkill);
    }


}
