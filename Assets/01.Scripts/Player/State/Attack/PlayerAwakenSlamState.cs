using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAwakenSlamState : PlayerAttackState
{
    private readonly int _comboCounterHash = Animator.StringToHash("SlamComboCounter");
    private int _comboCounter;

    private float _attackPrevTime;
    private float _comboWindow = 0.8f;

    private bool _isCombo = false;

    public PlayerAwakenSlamState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _player.PlayerInput.SlamSkillEvent += ComboSkill;
        _player.RotateToMousePos();

        SetCombo();

        _player.StartDelayAction(0.1f, () =>
        {
            _player.StopImmediately(false);
        });
    }

    public override void Exit()
    {
        base.Exit();

        _attackPrevTime = Time.time;
        ++_comboCounter;
    }

    public override void UpdateState()
    {
        base.UpdateState();

        if (_endTriggerCalled)
        {
            if (_isCombo)
                _stateMachine.ChangeState(PlayerStateEnum.AwakenSlam);
            else
                _stateMachine.ChangeState(PlayerStateEnum.Idle);
        }
    }

    private void ComboSkill()
    {
        _isCombo = true;
    }

    private void SetCombo()
    {
        _isCombo = false;

        if (_comboCounter >= 3 || Time.time >= _attackPrevTime + _comboWindow)
            _comboCounter = 0;

        _player.AnimatorCompo.SetInteger(_comboCounterHash, _comboCounter);
    }
}
