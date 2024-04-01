using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMeleeAttackState : PlayerState
{
    private int _comboCounter; // ���� �޺�
    private float _lastAttackTime; // ���������� ������ �ð�
    private float _comboWindow = 0.8f; // �޺��� �����?������ �ð� 
   
    private readonly int _comboCounterHash = Animator.StringToHash("ComboCounter");

    private HashSet<RaycastHit> _enemyDuplicateCheck = new HashSet<RaycastHit>();
    private float _hitDistance = 5f; // 2.4�� ��ũ��.

    private Transform _weaponRayPoint;

    private float _awakenAttackDist = 4.2f;
    private float _defaultAttackDist = 2.8f;

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

        _weaponRayPoint = _player._currentWeapon.transform.Find("Point");
        _hitDistance = _player.IsAwakening ? _awakenAttackDist : _defaultAttackDist;
        
        if (_comboCounter >= 2 || Time.time >= _lastAttackTime + _comboWindow)
            _comboCounter = 0; // �޺� �ʱ�ȭ

        _player.UsingAnimatorCompo.SetInteger(_comboCounterHash, _comboCounter);
        _player.UsingAnimatorCompo.speed = _player.attackSpeed;

        Vector3 move = _player.attackMovement[_comboCounter];
        _player.SetVelocity(new Vector3(move.x, move.y, move.z));

        _player.StartDelayAction(0.1f, () =>
        {
            _player.StopImmediately(false);
        });


    }

    public override void Exit()
    {
        _player.PlayerInput.MeleeAttackEvent -= ComboAttack;

        _player.IsAttack = false;
        _isAwakenSlashEffectOn = false;
        _slashEffectOn = false;

        _lastAttackTime = Time.time;
        
        ++_comboCounter;
        _player.UsingAnimatorCompo.speed = 1f;

        _enemyDuplicateCheck.Clear();



        base.Exit();
    }

    public override void UpdateState()
    {
        base.UpdateState();


        Vector3 dir = (_weaponRayPoint.position - _player._currentWeapon.transform.position).normalized;

        Debug.DrawRay(_player._currentWeapon.transform.position, dir * _hitDistance, Color.blue);
        if (_isHitAbleTriggerCalled)
        {
            RaycastHit[] enemies = Physics.RaycastAll(_player._currentWeapon.transform.position, dir, _hitDistance, _player._enemyLayer);
            Debug.Log(enemies.Length);
            foreach(var enemy in enemies)
            {
                if (_enemyDuplicateCheck.Add(enemy))
                {
                    if (enemy.transform.TryGetComponent<Brain>(out Brain brain))
                    {
                        brain.OnHit(_player.attackPower);
                        if (!_player.IsAwakening)
                            _player.awakenCurrentGage++;
                    }
                }
            }
        }

        if (_isHitAbleTriggerCalled)
        {
            if (!_slashEffectOn)
            {
                _player._currentSlashParticle.Play();

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

        if (_endTriggerCalled)
        {
            _player._currentSlashParticle.Stop();
            _player._currentSlashParticle.Clear();

            _stateMachine.ChangeState(PlayerStateEnum.Idle);
        }
    }

    private void ComboAttack()
    {
        if (_actionTriggerCalled)
        {
            if (_player.IsGroundDetected())
            {
                _stateMachine.ChangeState(PlayerStateEnum.MeleeAttack);
            }
        }
    }   

    public void UpgradeActivePoison()
    {
        //TODO: use poison every attack
    }

    public void KillInactivePoison()
    {

    }
}
