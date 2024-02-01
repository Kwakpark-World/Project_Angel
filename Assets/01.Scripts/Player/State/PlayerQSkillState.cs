using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerQSkillState : PlayerState
{
    private float _jumpForce = 12f;
    private float _dropForce = 12f;
    
    public PlayerQSkillState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _player.SetVelocity(Vector3.up * _jumpForce);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void UpdateState()
    {
        base.UpdateState();
        if (!_player.IsGroundDetected()) 
        {
            _player.SetVelocity(Vector3.down * _dropForce);


        }
    }
}
