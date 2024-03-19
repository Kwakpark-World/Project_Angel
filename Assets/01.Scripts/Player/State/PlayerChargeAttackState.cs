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

        if (_player.IsAwakening)
            Debug.Log("Effect");
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
            _player.SetAnimCollider(_player.DefaultCollider.height, -0.1f, 0.01f);
            _stateMachine.ChangeState(PlayerStateEnum.Idle);
        }
    }
}
