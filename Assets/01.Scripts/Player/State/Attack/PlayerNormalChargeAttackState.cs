using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerNormalChargeAttackState : PlayerChargeState
{
    private float _normalWidth = 11f;
    private float _awakenWidth = 11f;

    private float _normalHeight = 3f;
    private float _awakenHeight = 3f;

    private float _normalDist = 6;
    private float _awakenDist = 10;

    private Vector3 _normalAttackOffset = Vector3.forward;
    private Vector3 _awakenAttackOffset;

    private bool _isEffectOn = false;

    public PlayerNormalChargeAttackState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();
        _isEffectOn = false;

        _hitDist = _player.IsAwakening ? _awakenDist : _normalDist;
        _hitHeight = _player.IsAwakening ? _awakenHeight : _normalHeight;
        _hitWidth = _player.IsAwakening ? _awakenWidth : _normalWidth;

        Vector3 size = new Vector3(_hitWidth, _hitHeight, _hitDist);
        Vector3 offset = _player.IsAwakening ? _awakenAttackOffset : _normalAttackOffset;

        _attackOffset = offset;
        _attackSize = size;

        _player.AnimatorCompo.speed = 1 + (_player.ChargingGauge / (_maxChargeTime * 10)) * _player.PlayerStatData.GetChargingAttackSpeed();
    }

    public override void Exit()
    {
        base.Exit();
        _player.enemyNormalHitDuplicateChecker.Clear();
    }

    public override void UpdateState()
    {
        base.UpdateState();
        
        // 가설. 여기서 이게 계속 돎 그렇기에 때리는 함수에 값을 넘기고 
        if (_isHitAbleTriggerCalled)
            ChargeAttack();

        if (_effectTriggerCalled)
        {
            if (!_isEffectOn)
            {
                _isEffectOn = true;
                ChargeAttackEffect();
            }
        }

        if (_endTriggerCalled)                                                                                
        {
            _stateMachine.ChangeState(PlayerStateEnum.Idle);
        }
    }

    private void ChargeAttackEffect()
    {
        Vector3 pos = _player._weapon.transform.position;
        EffectManager.Instance.PlayEffect(PoolingType.Effect_PlayerAttack_Charged_Normal, pos);

    }

    private void ChargeAttack()
    {
        Vector3 offset = _player.transform.forward;
        offset.x += _attackOffset.x;
        offset.y += _attackOffset.y;
        offset.z += _attackOffset.z;

        Vector3 pos = _weaponRT.position + offset;

        Vector3 halfSize = _attackSize * 0.5f;

        Quaternion rot = Quaternion.Euler((_player.transform.forward * _hitDist) - _player.transform.position).normalized;
        
        Collider[] enemies = Physics.OverlapBox(pos, halfSize, rot, _player._enemyLayer);

        Attack(enemies.ToList());
    }

}
