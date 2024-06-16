using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class PlayerNormalSlamState : PlayerAttackState
{
    private float _width = 5f;
    private float _height = 10f;
    private float _dist = 13f;
    private Vector3 _offset;

    private float _jumpForce = 10f;
    private float _dropForce = 44f;

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

        
        if (_actionTriggerCalled )
        {
            AttackDrop();
        }

        if (_effectTriggerCalled)
        {
            if (!_isEffectOn)
            {
                _isEffectOn = true;
                CameraManager.Instance.ShakeCam(0.2f, 0.4f, 5f);
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
        //_offset.y += 1f;

        _attackOffset = _offset;
        _attackSize = size;
    }

    private void SlamAttack()
    {
        Collider[] enemies = GetEnemyByOverlapBox(_player.transform.position, _player.transform.rotation);

        Attack(enemies.ToList());
    }

    private void SlamEffect()
    {
        float yOffset = 0f;
        Vector3 rayPos = _player.transform.position;
        rayPos.y += 1f;
        RaycastHit hit;

        if (Physics.Raycast(rayPos, Vector3.down, out hit, 300f, LayerMask.GetMask("Ground")))
        {
            yOffset = hit.transform.position.y;
        }
        Vector3 pos = _player.transform.position + _attackOffset;
        pos.y = yOffset;

        EffectManager.Instance.PlayEffect(PoolType.Effect_PlayerAttack_Slam_Normal, pos);
    }

    private void JumpToFront()
    {
        Vector3 move = Vector3.one;
        move.y *= _jumpForce;

        float slamDist = Vector3.Distance(_player.transform.position, _player.MousePosInWorld) * 2f;

        move += _player.transform.forward * Mathf.Min(slamDist, _player.PlayerStatData.GetSlamMaxDistance());
        _player.SetVelocity(move);
    }

    private void AttackDrop()
    {
        _player.SetVelocity(Vector3.down * _dropForce);
    }
}
