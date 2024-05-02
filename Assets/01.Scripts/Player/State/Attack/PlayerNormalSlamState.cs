using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerNormalSlamState : PlayerAttackState
{
    private float _width = 11f;
    private float _height = 3f;
    private float _dist = 6f;
    private Vector3 _offset;

    private float _jumpForce = 10f;
    private float _dropForce = 22f;
    private float _forwardDist = 20f;

    private float _awakenAttackDist = 15f;
    private float _normalAttackDist = 12f;

    private bool _isEffectOn = false;
    
    public PlayerNormalSlamState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _player.StopImmediately(false);
        _player.RotateToMousePos();
        _isEffectOn = false;

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

        if (_effectTriggerCalled)
        {
            if (!_isEffectOn)
            {
                _isEffectOn = true;
                SlamEffect();
            }
        }

        if (_endTriggerCalled )
        {
            if (_player.IsGroundDetected())
            {
                SlamAttack();   

                _stateMachine.ChangeState(PlayerStateEnum.Idle);
            } 
        }
    }

    protected override void SetAttackSetting()
    {
        _offset = _player.transform.forward;

        _hitDist = _dist;
        _hitHeight = _height;
        _hitWidth = _width;

        Vector3 size = new Vector3(_hitWidth, _hitHeight, _hitDist);

        Vector3 offset = _offset;

        _attackOffset = offset;
        _attackSize = size;
    }

    private void SlamAttack()
    {
        
    }

    private void SlamEffect()
    {
        Vector3 pos = _player.transform.position + _offset;
        EffectManager.Instance.PlayEffect(PoolingType.Effect_PlayerAttack_Slam_Normal, pos);
    }

    private void JumpToFront()
    {
        Vector3 move = Vector3.one;
        move.y *= _jumpForce;

        move += _player.transform.forward * _forwardDist;
        _player.SetVelocity(move);
    }

    private void AttackDrop()
    {
        _player.SetVelocity(Vector3.down * _dropForce);
    }
}
