using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMeleeAttackState : PlayerAttackState
{
    private readonly int _comboCounterHash = Animator.StringToHash("ComboCounter");

    private int _comboCounter;
    
    private float _attackPrevTime;
    private float _comboWindow = 0.8f;

    private float _awakenAttackDist = 4.4f;
    private float _normalAttackDist = 3f;


    private bool _isCombo;

    public PlayerMeleeAttackState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();
        _player.PlayerInput.MeleeAttackEvent += ComboAttack;
        _player.IsAttack = true;
        
        _player.AnimatorCompo.speed = _player.PlayerStatData.GetAttackSpeed();

        _hitDist = _player.IsAwakening ? _awakenAttackDist : _normalAttackDist;

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

        _player.AnimatorCompo.speed = 1f;

        _attackPrevTime = Time.time;

        ++_comboCounter;

    }

    public override void UpdateState()
    {
        base.UpdateState();

        
        if (_isHitAbleTriggerCalled)
        {
            MeleeAttack();
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

    private void MeleeAttack()
    {
        List<RaycastHit> enemies = GetEnemyByWeapon();

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

    private void SetCombo()
    {
        _isCombo = false;

        if (_comboCounter >= 7 || Time.time >= _attackPrevTime + _comboWindow)
            _comboCounter = 0;

        _player.AnimatorCompo.SetInteger(_comboCounterHash, _comboCounter);

    }
}
