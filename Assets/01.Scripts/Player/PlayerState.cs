using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    protected PlayerStateMachine _stateMachine;
    protected Player _player;
    protected Rigidbody _rigidbody; // �̰� �ɼ� �־ �ǰ� ��� ��

    protected int _animBoolHash; // �� ���º� �ִϸ��̼� �ؽð�
    protected readonly int _yVelocityHash = Animator.StringToHash("y_velocity");

    public bool _actionTriggerCalled { get; private set; } = false;
    public bool _endTriggerCalled { get; private set; } = false;

    public PlayerState(Player player, PlayerStateMachine stateMachine, string animBoolName)
    {
        _player = player;
        _stateMachine = stateMachine;
        _animBoolHash = Animator.StringToHash(animBoolName);
        _rigidbody = _player.RigidbodyCompo;
    }

    public virtual void Enter()
    {
        _endTriggerCalled = false;
        _actionTriggerCalled = false;
        _player.AnimatorCompo.SetBool(_animBoolHash, true);
    }

    public virtual void UpdateState()
    {
        _player.AnimatorCompo.SetFloat(_yVelocityHash, _rigidbody.velocity.y);
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
}
