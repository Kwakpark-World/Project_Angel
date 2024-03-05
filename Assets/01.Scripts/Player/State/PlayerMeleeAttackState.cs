using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMeleeAttackState : PlayerState
{
    private int _comboCounter; // ���� �޺�
    private float _lastAttackTime; // ���������� ������ �ð�
    private float _comboWindow = 0.8f; // �޺��� ����� ������ �ð� 
    
    private readonly int _comboCounterHash = Animator.StringToHash("ComboCounter");

    public PlayerMeleeAttackState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();
        _player.PlayerInput.MeleeAttackEvent += ComboAttack;

        _player.IsAttack = true;
        
        if (_comboCounter >= 2 || Time.time >= _lastAttackTime + _comboWindow)
            _comboCounter = 0; // �޺� �ʱ�ȭ

        _player.AnimatorCompo.SetInteger(_comboCounterHash, _comboCounter);

        _player.AnimatorCompo.speed = _player.attackSpeed;

        float xInput = _player.PlayerInput.XInput;

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
        ++_comboCounter;
        _lastAttackTime = Time.time;
        _player.AnimatorCompo.speed = 1f;
        base.Exit();
    }

    public override void UpdateState()
    {
        base.UpdateState();

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
