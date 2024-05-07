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
    }

    public override void Exit()
    {
        _player.IsGroundState = true;

        _player.PlayerInput.SlamSkillEvent -= SlamSkillHandle;
        _player.PlayerInput.AwakeningSkillEvent -= AwakeningSkillHandle;
        _player.PlayerInput.MeleeAttackEvent -= HandlePrimaryAttackEvent;
        base.Exit();
    }

    public override void UpdateState()
    {
        base.UpdateState();

        PlayerDefense();
    }

    private void PlayerDefense()
    {
        if (_player.PlayerInput.isDefense)
        {
            if (_player.IsGroundDetected())
            {
                if (_player.PlayerStatData.GetDefenseCooldown() + _player.defensePrevTime > Time.time) return;
                _player.StateMachine.ChangeState(PlayerStateEnum.Defense);
            }
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
        if (_player._slamPrevTime + _player.PlayerStatData.GetSlamSkillCooldown() > Time.time) return;
        if (!_player.IsGroundDetected()) return;

        _player._slamPrevTime = Time.time;

        if (_player.IsAwakening)
            _stateMachine.ChangeState(PlayerStateEnum.AwakenSlam);
        else
            _stateMachine.ChangeState(PlayerStateEnum.NormalSlam);
    }
}
