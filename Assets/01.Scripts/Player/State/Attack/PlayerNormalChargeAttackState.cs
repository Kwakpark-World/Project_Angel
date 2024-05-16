using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerNormalChargeAttackState : PlayerChargeState
{
    private float _width = 11f;
    private float _height = 3f;
    private float _dist = 6f;
    private Vector3 _offset;
    
    private bool _isEffectOn = false;

    private ParticleSystem _thisParticle;

    public PlayerNormalChargeAttackState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();
        _isEffectOn = false;

        _player.AnimatorCompo.speed = 1 + (_player.ChargingGauge / (_maxChargeTime * 10)) * _player.PlayerStatData.GetChargingAttackSpeed();
        _thisParticle = _player._effectParent.Find(_effectString).GetComponent<ParticleSystem>();

    }

    public override void Exit()
    {
        base.Exit();
        _player.enemyNormalHitDuplicateChecker.Clear();
        _thisParticle.Stop();

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
            _stateMachine.ChangeState(PlayerStateEnum.NormalChargeStabAttack);
        }
    }

    protected override void SetAttackSetting()
    {
        _hitDist = _dist;
        _hitHeight = _height;
        _hitWidth = _width;

        Vector3 size = new Vector3(_hitWidth, _hitHeight, _hitDist);

        _offset = _player.transform.forward;

        _attackOffset = _offset;
        _attackSize = size;
    }

    private void ChargeAttackEffect()
    {
        _thisParticle.Play();

        //Vector3 pos = _player._weapon.transform.position;
        //EffectManager.Instance.PlayEffect(PoolingType.Effect_PlayerAttack_Charged_Normal, pos);

    }

    private void ChargeAttack()
    { 
        Collider[] enemies = GetEnemyByRange(_player.transform.position, _player.transform.rotation);
      
        Attack(enemies.ToList());
    }

}
