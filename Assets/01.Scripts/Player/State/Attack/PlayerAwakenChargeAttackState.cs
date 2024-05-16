using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerAwakenChargeAttackState : PlayerChargeState
{
    private float _width = 10f;
    private float _height = 3f;
    private float _dist = 10f;
    private Vector3 _offset;

    private const string _animName = "PlayerAttack_Charged_Awaken";

    private bool _isEffectOn = false;
    private ParticleSystem[] _thisParticles;

    public PlayerAwakenChargeAttackState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _isEffectOn = false;

        _player.AnimatorCompo.speed = 1 + (_player.ChargingGauge / (_maxChargeTime * 10)) * _player.PlayerStatData.GetChargingAttackSpeed();
        _thisParticles = _player.effectParent.Find(_effectString).GetComponentsInChildren<ParticleSystem>();

    }

    public override void Exit()
    {
        base.Exit();

        _player.AnimatorCompo.speed = 1;
        _player.enemyNormalHitDuplicateChecker.Clear();

        foreach (var particle in _thisParticles)
        {
            particle.Stop();
        }
    }

    public override void UpdateState()
    {
        base.UpdateState();

        if (_effectTriggerCalled)
        {
            if (!_isEffectOn)
            {
                _isEffectOn = true;
                ChargeAttackEffect();
            }
        }

        if (_isHitAbleTriggerCalled)
        {
            ChargeAttack();
        }

        if (_endTriggerCalled)
        {
            _stateMachine.ChangeState(PlayerStateEnum.Idle);
        }
    }

    protected override void SetAttackSetting()
    {
        _hitDist = _dist;
        _hitHeight = _height;
        _hitWidth = _width;

        Vector3 size = new Vector3(_hitWidth, _hitHeight, _hitDist);

        _offset = Vector3.zero;

        _attackOffset = _offset;
        _attackSize = size;
    }

    private void ChargeAttackEffect()
    {

        float playerDuration = float.MaxValue;
        foreach (var anim in _player.playerAnims)
        {
            if (anim.name == _animName)
            {
                playerDuration = anim.length - 0.84f;
                break;
            }
        }
        if (playerDuration == float.MaxValue)
            Debug.LogError($"Effect : {_effectString} string is not match, this Effect Duration is");

        playerDuration /= _player.AnimatorCompo.speed;

        foreach (var particle in _thisParticles)
        {
            particle.Stop();
            var main = particle.main;
            main.duration = playerDuration;
            particle.Play();
        }

        //Vector3 pos = _player.transform.position;
        //EffectManager.Instance.PlayEffect(PoolingType.Effect_PlayerAttack_Charged_Awaken, pos);
    }

    private void ChargeAttack()
    {
        if (_TickCheckTriggerCalled) // default One Hit + Tick(Anim Event)
        {
            _TickCheckTriggerCalled = false;
            _player.enemyNormalHitDuplicateChecker.Clear();
        }

        Collider[] enemies = GetEnemyByRange(_player.transform.position, _player.transform.rotation);

        Attack(enemies.ToList());
    }
}
