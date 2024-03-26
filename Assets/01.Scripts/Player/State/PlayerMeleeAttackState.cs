using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMeleeAttackState : PlayerState
{
    private int _comboCounter; // 현재 콤보
    private float _lastAttackTime; // 마지막으로 공격한 시간
    private float _comboWindow = 0.8f; // 콤보가 끊기기 까지의 시간 
   
    private readonly int _comboCounterHash = Animator.StringToHash("ComboCounter");

    private HashSet<RaycastHit> _enemyDuplicateCheck = new HashSet<RaycastHit>();
    private float _hitDistance = 2.8f; // 2.4가 검크기.

    private Transform _weaponRayPoint;
    
    public PlayerMeleeAttackState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
        
    }

    public override void Enter()
    {
        base.Enter();
        _player.PlayerInput.MeleeAttackEvent += ComboAttack;
        _player.IsAttack = true;

        _weaponRayPoint = _player._currentWeapon.transform.Find("Point");
        
        if (_comboCounter >= 2 || Time.time >= _lastAttackTime + _comboWindow)
            _comboCounter = 0; // 콤보 초기화

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
        if (_isHitAbleCalled)
        {
            RaycastHit[] enemies = Physics.RaycastAll(_player._currentWeapon.transform.position, dir, _hitDistance, _player._enemyLayer);
    
            foreach(var enemy in enemies)
            {
                if (_enemyDuplicateCheck.Add(enemy))
                {
                    if (enemy.transform.TryGetComponent<Brain>(out Brain brain))
                    {
                        brain.OnHit(_player.attackPower);
                    }
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
        if (_actionTriggerCalled)
        {
            if (_player.IsGroundDetected())
            {
                _stateMachine.ChangeState(PlayerStateEnum.MeleeAttack);
            }
        }
    }    
}
