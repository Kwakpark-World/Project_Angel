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
        _player.StopImmediately(true);

        _player.StartCoroutine(PlayerAwakening());

        Vector3 pos = _player.transform.position;
        pos.y += 1.5f;

        EffectManager.Instance.PlayEffect(PoolingType.PlayerESkillEffect, pos);
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
            _player.SetPlayerModelAndAnim();
            _player.fakePlayer?.SetPlayerModelAndAnim();
            _stateMachine.ChangeState(PlayerStateEnum.Idle);
        }
    }

    private IEnumerator PlayerAwakening()
    {
        _player.IsAwakening = true;
        yield return new WaitForSeconds(_awakeningTime);
        _player.IsAwakening = false;

        _player.SetPlayerModelAndAnim();
        _player.fakePlayer?.SetPlayerModelAndAnim();
        _stateMachine.ChangeState(PlayerStateEnum.Idle);
    }
}
