using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDefenseState : PlayerState
{
    private float _defenseTimer;

    public PlayerDefenseState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _player.IsDefense = true;
        _player.StopImmediately(false);

    }

    public override void Exit()
    {
        base.Exit();
        _player.IsDefense = false;
    }

    public override void UpdateState()
    {
        base.UpdateState();

        _defenseTimer += Time.deltaTime;
        
        if (!_player.PlayerInput.isDefense || _defenseTimer >= _player.defenseTime)
        {
            _defenseTimer = 0;
            _player.defensePrevTime = Time.time;

            _stateMachine.ChangeState(PlayerStateEnum.Idle);
        }
    }
}
