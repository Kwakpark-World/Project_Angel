using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerNormalSlamState : PlayerAttackState
{
    private float _width = 5f;
    private float _height = 10f;
    private float _dist = 13f;
    private Vector3 _offset;

    private float _jumpForce = 10f;
    private float _dropForce = 22f;
    private float _forwardDist = 20f;

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

        JumpToFront();
    }

    public override void Exit()
    {
        base.Exit();

        _player.enemyNormalHitDuplicateChecker.Clear();
    }

    public override void UpdateState()
    {
        base.UpdateState();

        Debug.Log(_player.transform.forward * 2);
        
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
            } 
            _stateMachine.ChangeState(PlayerStateEnum.Idle);
        }
    }

    protected override void SetAttackSetting()
    {
        _hitDist = _dist;
        _hitHeight = _height;
        _hitWidth = _width;

        Vector3 size = new Vector3(_hitWidth, _hitHeight, _hitDist);

        _offset = _player.transform.forward * 5;
        _offset.y += 1f;

        _attackOffset = _offset;
        _attackSize = size;
    }

    private void SlamAttack()
    {
        Collider[] enemies = GetEnemyByRange(_player.transform.position, _player.transform.rotation);

        Attack(enemies.ToList());
    }

    private void SlamEffect()
    {
        Vector3 pos = _player.transform.position + _attackOffset;
        pos.y = 0;

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
