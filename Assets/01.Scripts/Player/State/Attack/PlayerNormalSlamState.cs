using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerNormalSlamState : PlayerAttackState
{
    private float _jumpForce = 10f;
    private float _dropForce = 22f;

    private float _attackHeight = 2f;

    private float _awakenAttackDist = 15f;
    private float _normalAttackDist = 12f;

    private bool _isAttacked = false;
    
    public PlayerNormalSlamState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _player.StopImmediately(false);
        _player.RotateToMousePos();
        _isAttacked = false;
        
        _hitDist = _player.IsAwakening ? _awakenAttackDist : _normalAttackDist;

        JumpToFront();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void UpdateState()
    {
        base.UpdateState();

        if (_actionTriggerCalled )
        {
            AttackDrop();
        }

        if (_endTriggerCalled )
        {
            if (_player.IsGroundDetected())
            {
                if (!_isAttacked)
                {
                    SlamAttack();
                }

                _stateMachine.ChangeState(PlayerStateEnum.Idle);
            } 
        }
    }

    private void SlamAttack()
    {
        
    }

    private void JumpToFront()
    {
        Vector3 move = Vector3.one;
        move.y *= _jumpForce;

        move += _player.transform.forward * 20f;
        _player.SetVelocity(move);
    }

    private void AttackDrop()
    {
        _player.SetVelocity(Vector3.down * _dropForce);
    }
}
