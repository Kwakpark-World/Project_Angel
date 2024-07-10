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
        _player.PlayerInput.WhirlwindEvent += WhirlwindSkillHandle;
    }

    public override void Exit()
    {
        base.Exit();
        _player.IsGroundState = true;

        _player.PlayerInput.SlamSkillEvent -= SlamSkillHandle;
        _player.PlayerInput.AwakeningSkillEvent -= AwakeningSkillHandle;
        _player.PlayerInput.MeleeAttackEvent -= HandlePrimaryAttackEvent;
        _player.PlayerInput.WhirlwindEvent -= WhirlwindSkillHandle;

    }

    public override void UpdateState()
    {
        base.UpdateState();
    }

    private void HandlePrimaryAttackEvent()
    {
        if (_player.IsPlayerStop == PlayerControlEnum.Stop) return;

        _stateMachine.ChangeState(PlayerStateEnum.Charging);
    }

    private void AwakeningSkillHandle()
    {
        if (_player.IsPlayerStop == PlayerControlEnum.Stop) return;
        if (_player.CurrentAwakenGauge <= 0) return;
        if (_player.IsAwakened)
        {
            _player.awakenTime = _player.PlayerStatData.GetMaxAwakenDuration();
            return;
        }
    }

    private void SlamSkillHandle()
    {
        if (_player.IsPlayerStop == PlayerControlEnum.Stop) return;
        if (_player.slamPrevTime + _player.PlayerStatData.GetSlamCooldown() > Time.time) return;
        if (!_player.IsGroundDetected()) return;
        if (_player.CurrentAwakenGauge <= 0) return;

        _player.slamPrevTime = Time.time;
        _player.awakenTime = 0;
        if (!_player.IsAwakened)
        {
            _player.StateMachine.ChangeState(PlayerStateEnum.Awakening);
            return;
        }

        _stateMachine.ChangeState(PlayerStateEnum.NormalSlam);
        //if (_player.IsAwakened)
        //    _stateMachine.ChangeState(PlayerStateEnum.AwakenSlam);
        //else
    }

    private void WhirlwindSkillHandle()
    {
        if (_player.IsPlayerStop == PlayerControlEnum.Stop) return;
        if (_player.whirlwindPrevTime + _player.PlayerStatData.GetWhirlwindCooldown() > Time.time) return;
        if (!_player.IsGroundDetected()) return;
        if (_player.CurrentAwakenGauge <= 0) return;


        _player.awakenTime = 0;
        _player.whirlwindPrevTime = Time.time;
        if (!_player.IsAwakened)
        {
            _player.StateMachine.ChangeState(PlayerStateEnum.Awakening);
            return;
        }

        _stateMachine.ChangeState(PlayerStateEnum.AwakenChargeAttack);
        //if (_player.IsAwakening)
        //    _stateMachine.ChangeState(PlayerStateEnum.AwakenSlam);
        //else
    }
}
