using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDieState : PlayerState
{
    private bool _isCollider;

    public PlayerDieState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _player.IsDie = true;
        _isCollider = false;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void UpdateState()
    {
        base.UpdateState();

        if (_isCollider) return;
        if (_actionTriggerCalled)
        {
            _isCollider = true;
            _player.StartCoroutine(ColliderChange());
        }
    }

    private IEnumerator ColliderChange()
    {
        while (_player.ColliderCompo.height > 0)
        {
            _player.ColliderCompo.height -= 0.07f;
            yield return new WaitForSeconds(0.1f);
        }

        yield return null;
    }
}
