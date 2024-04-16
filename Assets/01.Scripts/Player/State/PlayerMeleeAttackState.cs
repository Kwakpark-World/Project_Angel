using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMeleeAttackState : PlayerAttackState
{
    private int _comboCounter;
    private float _lastAttackTime;
    private float _comboWindow = 0.8f;

    private bool _isCombo;

    private readonly int _comboCounterHash = Animator.StringToHash("ComboCounter");

    private float _hitDistance = 5f; // 2.4�� ��ũ��.

    private float _awakenAttackDist = 4.4f;
    private float _defaultAttackDist = 3f;

    private bool _isAwakenSlashEffectOn = false;
    private bool _slashEffectOn = false;

    public PlayerMeleeAttackState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();
        _player.PlayerInput.MeleeAttackEvent += ComboAttack;
        _player.IsAttack = true;
        _isAwakenSlashEffectOn = false;
        _slashEffectOn = false;
        _isCombo = false;

        _hitDistance = _player.IsAwakening ? _awakenAttackDist : _defaultAttackDist;

        if (_comboCounter >= 7 || Time.time >= _lastAttackTime + _comboWindow)
            _comboCounter = 0;

        _player.AnimatorCompo.SetInteger(_comboCounterHash, _comboCounter);
        _player.AnimatorCompo.speed = _player.attackSpeed;

        Vector3 move = _player.attackMovement[_comboCounter];
        _player.SetVelocity(new Vector3(move.x, move.y, move.z));

        _player.StartDelayAction(0.1f, () =>
        {
            _player.StopImmediately(false);
        });


    }

    public override void Exit()
    {
        base.Exit();

        _player.PlayerInput.MeleeAttackEvent -= ComboAttack;

        _player.IsAttack = false;
        _isAwakenSlashEffectOn = false;
        _slashEffectOn = false;
        _isCombo = false;

        _lastAttackTime = Time.time;

        ++_comboCounter;
        _player.AnimatorCompo.speed = 1f;

    }

    public override void UpdateState()
    {
        base.UpdateState();

        
        if (_isHitAbleTriggerCalled)
        {
            MeleeAttack();

            if (!_slashEffectOn)
            {
                Vector3 pos = _player._weapon.transform.position;

                //EffectManager.Instance.PlayEffect(PoolingType.PlayerSlashEffect, pos);

                _slashEffectOn = true;
            }
        }

        if (_effectTriggerCalled)
        {
            if (_player.IsAwakening)
            {
                if (!_isAwakenSlashEffectOn)
                {
                    _isAwakenSlashEffectOn = true;

                    Vector3 pos = _player.transform.position;
                    float range = 2f;

                    pos += _player.transform.forward * range;

                    EffectManager.Instance.PlayEffect(PoolingType.PlayerMeleeAttackEffect, pos);
                }
            }
        }

        if (_actionTriggerCalled)
        {
            if (_player.IsGroundDetected())
            {
                if (_isCombo)
                {
                    _stateMachine.ChangeState(PlayerStateEnum.MeleeAttack);
                }
            }
        }

        if (_endTriggerCalled)
        {
            _stateMachine.ChangeState(PlayerStateEnum.Idle);
        }
    }

    private void MeleeAttack()
    {
        Vector3 rightDir = (_weaponRB.position - _weaponRT.position).normalized;
        Vector3 leftDir = (_weaponLB.position - _weaponLT.position).normalized;

        Vector3 weaponPos = _player._weapon.transform.position;

        List<RaycastHit> enemies = new List<RaycastHit>();
        RaycastHit[] enemiesR = Physics.RaycastAll(weaponPos, rightDir, _hitDistance, _player._enemyLayer);
        RaycastHit[] enemiesL = Physics.RaycastAll(weaponPos, leftDir, _hitDistance, _player._enemyLayer);

        foreach (var enemy in enemiesL) enemies.Add(enemy);
        foreach (var enemy in enemiesR) enemies.Add(enemy);

        Attack(enemies);
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
