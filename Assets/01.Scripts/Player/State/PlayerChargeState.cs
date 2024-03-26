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
        _player.StopImmediately(false);
        _player.RotateToMousePos();

        _clickTimer = 0;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void UpdateState()
    {
        base.UpdateState();

        _clickTimer = Mathf.Clamp01(_clickTimer);
        
        if (!_player.PlayerInput.isCharge)
        {
            if (_clickTimer < 0.7f)
                _stateMachine.ChangeState(PlayerStateEnum.MeleeAttack);
            else
                _stateMachine.ChangeState(PlayerStateEnum.ChargeAttack);
        }
        else
        {
            _clickTimer += Time.deltaTime;

            if (_clickTimer >= 1)
            {
                _stateMachine.ChangeState(PlayerStateEnum.ChargeAttack);
            }
        }

    }
}
