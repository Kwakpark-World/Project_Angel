using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerQSkillState : PlayerState
{
    private float _jumpForce = 10f;
    private float _dropForce = 22f;

    private bool _isAttacked = false;

    private float _attackDist = 12f;
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
        _attackDist = _player.IsAwakening ? _awakenAttackDist : _defaultAttackDist;

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
        //Collider[] enemies = Physics.OverlapSphere(_player.transform.position, 10f, _player._enemyLayer);
        Vector3 pos = _player.transform.position;
        pos.y += _attackHeight / 2f;

        Collider[] enemies = Physics.OverlapBox(pos, new Vector3(_attackDist, _attackHeight, _attackDist), Quaternion.identity, _player._enemyLayer);

        HashSet<Collider> enemyDuplicateCheck = new HashSet<Collider>();

        foreach(var enemy in enemies)
        {
            if (enemyDuplicateCheck.Add(enemy))
            {
                if (enemy.transform.TryGetComponent<Brain>(out Brain brain))
                {
                    Debug.Log($"hit {enemy.transform.gameObject}");

                    brain.OnHit(_player.attackPower);
                    if (!_player.IsAwakening)
                        _player.awakenCurrentGage++;
                }
            }
        }
    }
}
