using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerESkillState : PlayerState
{

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

        _player.awakenCurrentGage = Mathf.Clamp(_player.awakenCurrentGage, 0, _player.awakenMaxGage);
        if (_endTriggerCalled)
        {
            _stateMachine.ChangeState(PlayerStateEnum.Idle);
        }
    }

    private IEnumerator PlayerAwakening()
    {
        while (_player.awakenCurrentGage >= 0)
        {
            if (_player.IsAwakening)
                _player.awakenCurrentGage -= 10 * Time.deltaTime;
            yield return null;
        }

        if (!_player.IsDie)
        {
            _player.IsAwakening = false;
            _player.SetPlayerModelAndAnim();
            _stateMachine.ChangeState(PlayerStateEnum.Idle);
        }
    }

}
