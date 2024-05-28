using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAwakenSlamState : PlayerAttackState
{
    private readonly int _comboCounterHash = Animator.StringToHash("SlamComboCounter");
    private int _comboCounter;
    private string _comboEffectString;

    private float _attackPrevTime;
    private float _comboWindow = 0.8f;

    private bool _isCombo = false;
    private bool _isEffectOn = false;

    public PlayerAwakenSlamState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
        
    }

    public override void Enter()
    {
        base.Enter();
        _player.PlayerInput.SlamSkillEvent += ComboSkill;
        _player.RotateToMousePos();

        _isEffectOn = false;

        _player.SetVelocity(_player.transform.forward * (_comboCounter + 1) * 5f);

        SetCombo();

        _comboEffectString = $"Effect_PlayerAttack_Slam_Awaken_{_comboCounter}";

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

        if (_effectTriggerCalled)
        {
            if (!_isEffectOn)
            {
                _isEffectOn = true;

                Vector3 pos = _player.transform.position;
                EffectManager.Instance.PlayEffect((PoolType)Enum.Parse(typeof(PoolType), _comboEffectString), pos);
            }
        }

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
        if (_comboCounter >= 2) return;
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
