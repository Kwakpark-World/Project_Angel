using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    protected PlayerStateEnum _stateEnum;

    protected PlayerStateMachine _stateMachine;
    protected Player _player;
    protected Rigidbody _rigidbody;

    protected int _animBoolHash; // 각 상태별 애니메이션 해시값 StateEnumString
    protected string _effectString;
    protected readonly int _yVelocityHash = Animator.StringToHash("y_velocity");

    public bool _actionTriggerCalled { get; private set; } = false;
    public bool _endTriggerCalled { get; private set; } = false;
    public bool _isHitAbleTriggerCalled { get; private set; } = false;
    public bool _effectTriggerCalled { get; private set; } = false;
    public bool _effectTriggerEndCalled { get; private set; } = false;
    public bool _TickCheckTriggerCalled;
    public bool _animationMoveFreezeToggleTrigger;

    public PlayerState(Player player, PlayerStateMachine stateMachine, string animBoolName)
    {
        _player = player;
        _stateMachine = stateMachine;
        _effectString = $"Player{animBoolName}Effect";
        _animBoolHash = Animator.StringToHash(animBoolName);
        _rigidbody = _player.RigidbodyCompo;
        _stateEnum = (PlayerStateEnum)Enum.Parse(typeof(PlayerStateEnum), animBoolName);
    }

    public virtual void Enter()
    {
        _endTriggerCalled = false;
        _actionTriggerCalled = false;
        _isHitAbleTriggerCalled = false;
        _effectTriggerCalled = false;
        _TickCheckTriggerCalled = false;
        _effectTriggerEndCalled = false;
        _animationMoveFreezeToggleTrigger = false;

        _player.AnimatorCompo.SetBool(_animBoolHash, true);
    }

    public virtual void UpdateState()
    {
        if (_player.IsPlayerStop)
        {
            _endTriggerCalled = true;
            _player.StopImmediately(true);
            _stateMachine.ChangeState(PlayerStateEnum.Idle);
        }

        _player.AnimatorCompo.SetFloat(_yVelocityHash, _rigidbody.velocity.y);
    }

    public virtual void FixedUpdateState()
    {

    }

    public virtual void Exit()
    {
        _player.AnimatorCompo.SetBool(_animBoolHash, false);
    }

    public void AnimationEndTrigger()
    {
        _endTriggerCalled = true;
    }

    public void AnimationActionTrigger()
    {
        _actionTriggerCalled = true;
    }

    public void AnimationHitAbleTrigger()
    {
        _isHitAbleTriggerCalled = true;
    }

    public void AnimationEffectTrigger()
    {
        _effectTriggerCalled = true;
    }

    public void AnimationEffectEndTrigger()
    {
        _effectTriggerEndCalled = true;
    }

    public void AnimationTickCheckTrigger()
    {
        _TickCheckTriggerCalled = true;
    }

    public void AnimationMoveFreezeToggleTrigger()
    {
        _animationMoveFreezeToggleTrigger = !_animationMoveFreezeToggleTrigger;
    }
}
