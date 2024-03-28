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

        if (_endTriggerCalled)                                                                                
        {
            _stateMachine.ChangeState(PlayerStateEnum.Idle);
        }
    }
}
