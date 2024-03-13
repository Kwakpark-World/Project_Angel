using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChargeState : PlayerState
{
    private float _clickTimer;

    public PlayerChargeState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();
        _clickTimer = 0;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void UpdateState()
    {
        base.UpdateState();

        if (!_player.PlayerInput.isCharge)
        {
            if (_clickTimer < 0.7f)
                _stateMachine.ChangeState(PlayerStateEnum.MeleeAttack);
            else
                _stateMachine.ChangeState(PlayerStateEnum.ChargeAttack);
        }
        else
            _clickTimer += Time.deltaTime;

    }
}
