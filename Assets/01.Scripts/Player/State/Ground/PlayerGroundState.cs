using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGroundState : PlayerState
{
    protected PlayerGroundState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();
        _player.IsGroundState = true;

        _player.PlayerInput.SlamSkillEvent += SlamSkillHandle;
        _player.PlayerInput.AwakeningSkillEvent += AwakeningSkillHandle;
        _player.PlayerInput.MeleeAttackEvent += HandlePrimaryAttackEvent;
        _player.PlayerInput.DefenseEvent += PlayerDefense;
    }

    public override void Exit()
    {
        base.Exit();
        _player.IsGroundState = true;

        _player.PlayerInput.SlamSkillEvent -= SlamSkillHandle;
        _player.PlayerInput.AwakeningSkillEvent -= AwakeningSkillHandle;
        _player.PlayerInput.MeleeAttackEvent -= HandlePrimaryAttackEvent;
        _player.PlayerInput.DefenseEvent -= PlayerDefense;

    }

    public override void UpdateState()
    {
        base.UpdateState();
    }

    private void PlayerDefense()
    {
        if (_player.IsGroundDetected())
        {
            if (_player.PlayerStatData.GetDefenseCooldown() + _player.defensePrevTime > Time.time) return;
            _player.StateMachine.ChangeState(PlayerStateEnum.Defense);
        }
        
    }

    private void HandlePrimaryAttackEvent()
    {
        _stateMachine.ChangeState(PlayerStateEnum.Charging);
    }

    private void AwakeningSkillHandle()
    {
        if (_player.IsAwakening) return;
        if (_player.awakenCurrentGauge < _player.PlayerStatData.GetMaxAwakenGauge()) return;

        _stateMachine.ChangeState(PlayerStateEnum.Awakening);
    }

    private void SlamSkillHandle()
    {
        if (_player.slamPrevTime + _player.PlayerStatData.GetSlamCooldown() > Time.time) return;
        if (!_player.IsGroundDetected()) return;

        _player.slamPrevTime = Time.time;

        if (_player.IsAwakening)
            _stateMachine.ChangeState(PlayerStateEnum.AwakenSlam);
        else
            _stateMachine.ChangeState(PlayerStateEnum.NormalSlam);
    }
}
