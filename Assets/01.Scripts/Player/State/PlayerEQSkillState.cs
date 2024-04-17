using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEQSkillState : PlayerAttackState
{
    private int _comboCounter;
    private float _lastAttackTime;
    private float _comboWindow = 0.8f;

    private bool _isCombo = false;

    private readonly int _comboCounterHash = Animator.StringToHash("QComboCounter");

    public PlayerEQSkillState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _player.PlayerInput.QSkillEvent += ComboSkill;
        _player.RotateToMousePos();

        _isCombo = false;

        if (_comboCounter >= 3 || Time.time >= _lastAttackTime + _comboWindow)
            _comboCounter = 0;

        _player.AnimatorCompo.SetInteger(_comboCounterHash, _comboCounter);

        _player.StartDelayAction(0.1f, () =>
        {
            _player.StopImmediately(false);
        });
    }

    public override void Exit()
    {
        base.Exit();
        _isCombo = false;

        _lastAttackTime = Time.time;

        ++_comboCounter;
    }

    public override void UpdateState()
    {
        base.UpdateState();

        if (_endTriggerCalled)
        {
            if (_isCombo)
                _stateMachine.ChangeState(PlayerStateEnum.EQSkill);
            else
                _stateMachine.ChangeState(PlayerStateEnum.Idle);
        }
    }

    private void ComboSkill()
    {
        _isCombo = true;
    }
}
