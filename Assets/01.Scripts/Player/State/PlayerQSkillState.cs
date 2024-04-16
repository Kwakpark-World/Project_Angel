using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerQSkillState : PlayerAttackState
{
    private float _jumpForce = 10f;
    private float _dropForce = 22f;

    private bool _isAttacked = false;

    private float _attackHeight = 2f;

    private float _awakenAttackDist = 15f;
    private float _defaultAttackDist = 12f;
    
    public PlayerQSkillState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _player.StopImmediately(false);
        _player.RotateToMousePos();
        _hitDistance = _player.IsAwakening ? _awakenAttackDist : _defaultAttackDist;

        Vector3 move = Vector3.one;
        move.y *= _jumpForce;

        move += _player.transform.forward * 20f;


        _player.SetVelocity(move);
    }

    public override void Exit()
    {
        _isAttacked = false;

        base.Exit();
    }

    public override void UpdateState()
    {
        base.UpdateState();

        if (_actionTriggerCalled )
        {
            _player.SetVelocity(Vector3.down * _dropForce);
        }

        if (_endTriggerCalled )
        {
            if (_player.IsGroundDetected())
            {
                if (!_isAttacked)
                {
                    Vector3 pos = _player.transform.position;
                    pos.y += 1f;
                    if (_player.IsAwakening)
                    {
                        EffectManager.Instance.PlayEffect(PoolingType.PlayerEQSkillEffect, pos);
                    }
                    else
                    {
                        EffectManager.Instance.PlayEffect(PoolingType.PlayerQSkillEffect, pos);
                    }

                    QAttack();
                }

                _stateMachine.ChangeState(PlayerStateEnum.Idle);
            } 
        }
    }

    private void QAttack()
    {
        _isAttacked = true;

        Vector3 pos = _player.transform.position;
        pos.y += _attackHeight / 2f;

        Vector3 size = new Vector3(_hitDistance, _attackHeight, _hitDistance);

        Collider[] enemies = Physics.OverlapBox(pos, size, Quaternion.identity, _player._enemyLayer);

        Attack(enemies.ToList());
    }
}
