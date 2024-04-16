using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMeleeAttackState : PlayerState
{
    private int _comboCounter;
    private float _lastAttackTime;
    private float _comboWindow = 0.8f;

    private bool _isCombo;

    private readonly int _comboCounterHash = Animator.StringToHash("ComboCounter");

    private float _hitDistance = 5f;

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
        _isCombo = false;

        _weaponRayPoint = _player._currentWeapon.transform.Find("Point");
        _hitDistance = _player.IsAwakening ? _awakenAttackDist : _defaultAttackDist;

        if (_comboCounter >= 2 || Time.time >= _lastAttackTime + _comboWindow)
            _comboCounter = 0;

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
        base.Exit();

        _player.PlayerInput.MeleeAttackEvent -= ComboAttack;

        _player.IsAttack = false;
        _isAwakenSlashEffectOn = false;
        _slashEffectOn = false;
        _isCombo = false;

        _lastAttackTime = Time.time;

        ++_comboCounter;
        _player.UsingAnimatorCompo.speed = 1f;

        _player.enemyNormalHitDuplicateChecker.Clear();
    }

    public override void UpdateState()
    {
        base.UpdateState();

        Vector3 dir = (_weaponRayPoint.position - _player._currentWeapon.transform.position).normalized;

        Debug.DrawRay(_player._currentWeapon.transform.position, dir * _hitDistance, Color.blue);
        if (_isHitAbleTriggerCalled)
        {
            RaycastHit[] enemies = Physics.RaycastAll(_player._currentWeapon.transform.position, dir, _hitDistance, _player._enemyLayer);

            foreach(var enemy in enemies)
            {
                if (enemy.transform.TryGetComponent<Brain>(out Brain brain))
                {
                    if (_player.enemyNormalHitDuplicateChecker.Add(brain))
                    {
                        if (_player.enemyNormalHitDuplicateChecker.Count == 1)
                        {
                            brain.OnHit(1f); // Call chaining method here.
                        }

                        brain.OnHit(_player.attackPower);

                        if (!_player.IsAwakening)
                            _player.awakenCurrentGage++;
                    }
                }
            }

            if (!_slashEffectOn)
            {
                Vector3 pos = _player._currentWeapon.transform.position;

                EffectManager.Instance.PlayEffect(PoolingType.Effect_PlayerAttack_Normal, pos);

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

                    EffectManager.Instance.PlayEffect(PoolingType.Effect_PlayerAttack_Awaken, pos);
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
