using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEChargeAttackState : PlayerState
{
    public PlayerEChargeAttackState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
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

        if (_endTriggerCalled)
        {
            _stateMachine.ChangeState(PlayerStateEnum.Idle);
        }
    }
}
