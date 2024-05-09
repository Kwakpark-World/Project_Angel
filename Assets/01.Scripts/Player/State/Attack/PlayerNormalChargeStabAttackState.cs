using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class PlayerNormalChargeStabAttackState : PlayerChargeState
{
    private float _width = 10f;
    private float _height = 5f;
    private float _dist = 5f;

    private bool _isStabMove;
    private bool _isEffectOn = false;

    private ParticleSystem _thisParticle;


    public PlayerNormalChargeStabAttackState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        _isEffectOn = false;
        _isStabMove = false;
    }

    public override void Exit()
    {
        base.Exit();

        _player.enemyNormalHitDuplicateChecker.Clear();

        _player.ChargingGauge = 0;
        _player.AnimatorCompo.speed = 1;

        _thisParticle.Stop();

    }

    public override void UpdateState()
    {
        base.UpdateState();

        if (_isHitAbleTriggerCalled)
        {
            ChargeAttackStab();
        }

        if (_effectTriggerCalled)
        {
            if (!_isEffectOn)
            {
                _isEffectOn = true;
                ChargeAttackStabEffect();
            }
        }        

        if (_actionTriggerCalled)
        {
            MoveToFront();
        }

        if (_endTriggerCalled)
        {
            _stateMachine.ChangeState(PlayerStateEnum.Idle);
        }
    }


    private void MoveToFront()
    {
        if (!_isStabMove)
        {
            _isStabMove = true;

            float stabDistance = _player.ChargingGauge * _player.PlayerStatData.GetChargingAttackDistance();
            _player.SetVelocity(_player.transform.forward * stabDistance);
        }
    }

    protected override void SetAttackSetting()
    {
        _hitDist = _dist;
        _hitHeight = _height;
        _hitWidth = _width;

        Vector3 size = new Vector3(_hitWidth, _hitHeight, _hitDist);

        Vector3 offset = Vector3.zero;

        _attackOffset = offset;
        _attackSize = size;
    }

    private void ChargeAttackStabEffect()
    {
        _thisParticle = _player._effectParent.Find(_effectString).GetComponent<ParticleSystem>();
        
        Vector3 pos = _weaponRB.transform.position;
        _thisParticle.transform.position = pos;

        _thisParticle.Play();
        //Vector3 pos = _player._weapon.transform.position;
        //EffectManager.Instance.PlayEffect(PoolingType.Effect_PlayerAttack_Charged_Sting_Normal, pos);

    }

    private void ChargeAttackStab()
    {
        Collider[] enemies = GetEnemyByRange(_player.transform.position, _player.transform.rotation);

        Attack(enemies.ToList());
    }
}
