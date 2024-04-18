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
        _player.IsGroundState = true;

        _player.PlayerInput.QSkillEvent += QSkillHandle;
        _player.PlayerInput.ESkillEvent += ESkillHandle;
        _player.PlayerInput.MeleeAttackEvent += HandlePrimaryAttackEvent;
    }

    public override void Exit()
    {
        _player.IsGroundState = true;

        _player.PlayerInput.QSkillEvent -= QSkillHandle;
        _player.PlayerInput.ESkillEvent -= ESkillHandle;
        _player.PlayerInput.MeleeAttackEvent -= HandlePrimaryAttackEvent;
        base.Exit();
    }

    public override void UpdateState()
    {
        base.UpdateState();
    }

    private void HandlePrimaryAttackEvent()
    {
        _stateMachine.ChangeState(PlayerStateEnum.Charging);
    }

    private void ESkillHandle()
    {
        if (_player.IsAwakening) return;
        if (_player.awakenCurrentGauge < _player.PlayerStatData.GetMaxAwakenGauge()) return;

        _stateMachine.ChangeState(PlayerStateEnum.Awakening);
    }

    private void QSkillHandle()
    {
        if (_player.slamPrevTime + _player.PlayerStatData.GetSlamSkillCooldown() > Time.time) return;

        _player.slamPrevTime = Time.time;

        if (_player.IsAwakening)
            _stateMachine.ChangeState(PlayerStateEnum.AwakenSlam);
        else
            _stateMachine.ChangeState(PlayerStateEnum.NormalSlam);
    }
}
