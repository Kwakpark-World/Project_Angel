using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerMeleeAttackState : PlayerAttackState
{
    private readonly int _comboCounterHash = Animator.StringToHash("ComboCounter");

    private int _comboCounter;

    private float _attackPrevTime;
    private float _comboWindow = 0.8f;

    private float _width = 0.8f;
    private float _height = 0.2f;
    private float _dist = 2.5f;
    private Vector3 _offset;

    private float _awakenAttackDist = 4.4f;
    private float _normalAttackDist = 2.5f;

    private bool _isCombo;
    private bool _isEffectOn;

    private ParticleSystem _thisParticle; 

    public PlayerMeleeAttackState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();
        _player.PlayerInput.MeleeAttackEvent += ComboAttack;

        _player.IsAttack = true;
        _isEffectOn = false;

        _player.AnimatorCompo.speed = _player.PlayerStatData.GetAttackSpeed();

        SetCombo();

        _hitDist = _player.IsAwakening ? _awakenAttackDist : _normalAttackDist;

        _player.StartDelayAction(0.1f, () =>
        {
            _player.StopImmediately(false);
        });

        _thisParticle = _player.weapon.transform.Find(_effectString).GetComponent<ParticleSystem>(); 


    }

    public override void Exit()
    {
        base.Exit();

        _player.PlayerInput.MeleeAttackEvent -= ComboAttack;
        _player.IsAttack = false;

        _player.AnimatorCompo.speed = 1f;

        _attackPrevTime = Time.time;

        ++_comboCounter;
        _player.enemyNormalHitDuplicateChecker.Clear();
        _thisParticle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
    }

    public override void UpdateState()
    {
        base.UpdateState();


        if (_isHitAbleTriggerCalled)
        {
            MeleeAttack();
        }

        if (_effectTriggerCalled)
        {
            if (!_isEffectOn)
            {
                _isEffectOn = true;
                ChargingEffect();
            }
        }

        if (_effectTriggerEndCalled)
        {
            _thisParticle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }

        if (_actionTriggerCalled)
        {
            if (_player.IsGroundDetected())
            {
                if (_isCombo)
                {
                    _stateMachine.ChangeState(PlayerStateEnum.MeleeAttack);
                    return;
                }
            }
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

        Vector3 size = new Vector3(_hitWidth, _hitDist, _hitHeight);

        _offset = _player.weapon.transform.up;

        _attackOffset = _offset;
        _attackSize = size;
    }

    private void MeleeAttack()
    {
        //List<RaycastHit> enemies = GetEnemyByWeapon();
        //
        //Attack(enemies);

        Collider[] enemies = GetEnemyByRange(_player.weapon.transform.position, _player.weapon.transform.rotation);

        Attack(enemies.ToList());
    }

    private void ChargingEffect()
    {
        _thisParticle.Play();
        //Vector3 pos = Vector3.zero;
        //EffectManager.Instance.PlayEffect(PoolingType.Effect_PlayerAttack_Charging_Normal, pos);
    }

    private void SetCombo()
    {
        _isCombo = false;

        if (_comboCounter >= 7 || Time.time >= _attackPrevTime + _comboWindow)
            _comboCounter = 0;

        _player.AnimatorCompo.SetInteger(_comboCounterHash, _comboCounter);

    }

    private void ComboAttack()
    {
        _isCombo = true;
    }

    public void UpgradeActivePoison()
    {
        //TODO: use poison every attack
    }

    public void KillInactivePoison()
    {

    }
}
