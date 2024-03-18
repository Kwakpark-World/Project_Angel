using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerESkillState : PlayerState
{
    private float _awakeningTime = 10f;

    public PlayerESkillState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _player.StartCoroutine(PlayerAwakening());
    }

    public override void Exit()
    {
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

    private IEnumerator PlayerAwakening()
    {
        _player.IsAwakening = true;
        yield return new WaitForSeconds(_awakeningTime);
        _player.IsAwakening = false;
    }
}
