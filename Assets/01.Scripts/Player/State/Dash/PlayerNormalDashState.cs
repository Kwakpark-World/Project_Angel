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

        _dashDirection = _player.transform.forward;

        _player.SetVelocity(_dashDirection * _player.PlayerStatData.GetDashSpeed());

        if (_endTriggerCalled)
        {
            _stateMachine.ChangeState(PlayerStateEnum.Idle);
        }
    }

    private void SlamSkillHandle()
    {
        if (_player._slamPrevTime + _player.PlayerStatData.GetSlamSkillCooldown() > Time.time) return;

        _player._slamPrevTime = Time.time;

        if (_player.IsAwakening)
            _stateMachine.ChangeState(PlayerStateEnum.AwakenSlam);
        else
            _stateMachine.ChangeState(PlayerStateEnum.NormalSlam);
    }
}
