using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChargeAttackState : PlayerState
{
    public PlayerChargeAttackState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void UpdateState()
    {
        base.UpdateState();

        if (_actionTriggerCalled)
        {
            _player.SetAnimCollider(1f, 0.1f, 0.1f);
        }

        if (_endTriggerCalled)
        {
            _player.SetCollider();
            _stateMachine.ChangeState(PlayerStateEnum.Idle);
        }
    }
}
