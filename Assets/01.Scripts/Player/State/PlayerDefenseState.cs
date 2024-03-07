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


    }

    public override void Exit()
    {
        base.Exit();
        _player.IsDefense = false;
        _defenseTimer = 0;
    }

    public override void UpdateState()
    {
        base.UpdateState();

        Debug.Log(_defenseTimer);
        _defenseTimer += Time.deltaTime;
        
        if (!_player.PlayerInput.isDefense)
            _stateMachine.ChangeState(PlayerStateEnum.Idle);
        
        if (_defenseTimer >= _player.defenseTime)
            _stateMachine.ChangeState(PlayerStateEnum.Idle);
    }
}
