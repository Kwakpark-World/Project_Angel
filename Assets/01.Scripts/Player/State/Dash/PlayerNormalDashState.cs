using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNormalDashState : PlayerDashState
{
    private Vector3 _dashDirection;

    public PlayerNormalDashState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _player.PlayerInput.SlamSkillEvent += SlamSkillHandle;
    }

    public override void Exit()
    {
        base.Exit();
        _player.PlayerInput.SlamSkillEvent -= SlamSkillHandle;
    }

    public override void UpdateState()
    {
        base.UpdateState();

        _dashDirection = _player.transform.forward * _player.PlayerStatData.GetDashSpeed();
        _dashDirection.y += _player.RigidbodyCompo.velocity.y;

        _player.SetVelocity(_dashDirection);
        if (_endTriggerCalled)
        {
            _stateMachine.ChangeState(PlayerStateEnum.Idle);
        }
    }

    private void SlamSkillHandle()
    {
        if (_player.IsPlayerStop == PlayerControlEnum.Stop) return;
        if (_player.slamPrevTime + _player.PlayerStatData.GetSlamCooldown() > Time.time) return;
        if (_player.CurrentAwakenGauge <= 19) return;

        _player.awakenTime = 0;
        _player.slamPrevTime = Time.time;
        if (!_player.IsAwakened)
        {
            _player.StateMachine.ChangeState(PlayerStateEnum.Awakening);
            return;
        }

        _player.slamPrevTime = Time.time;

        _player.CurrentAwakenGauge -= 20;
        _stateMachine.ChangeState(PlayerStateEnum.NormalSlam);
        //if (_player.IsAwakened)
        //    _stateMachine.ChangeState(PlayerStateEnum.AwakenSlam);
        //else
    }
}
