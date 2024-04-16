using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerGroundState : PlayerState
{
    protected PlayerGroundState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();
        _player.PlayerInput.QSkillEvent += QSkillHandle;
        _player.PlayerInput.ESkillEvent += ESkillHandle;
        _player.PlayerInput.MeleeAttackEvent += HandlePrimaryAttackEvent;
    }

    public override void Exit()
    {
        _player.PlayerInput.QSkillEvent -= QSkillHandle;
        _player.PlayerInput.ESkillEvent -= ESkillHandle;
        _player.PlayerInput.MeleeAttackEvent -= HandlePrimaryAttackEvent;
        base.Exit();
    }

    public override void UpdateState()
    {
        base.UpdateState();

        if (!_player.IsGroundDetected())
        {
            if (_player.IsStair) return;

            //_stateMachine.ChangeState(PlayerStateEnum.Fall);
        }
    }

    private void HandlePrimaryAttackEvent()
    {
        _stateMachine.ChangeState(PlayerStateEnum.Charge);
    }

    private void ESkillHandle()
    {
        if (_player.IsAwakening) return;
        if (_player.awakenCurrentGage < _player.awakenMaxGage) return;

        _stateMachine.ChangeState(PlayerStateEnum.ESkill);
    }

    private void QSkillHandle()
    {
        if (_player.qPrevTime + _player.qSkillCoolTime > Time.time) return;

        _player.qPrevTime = Time.time;

        if (_player.IsAwakening)
            _stateMachine.ChangeState(PlayerStateEnum.EQSkill);
        else
            _stateMachine.ChangeState(PlayerStateEnum.QSkill);
    }
}
