using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerESkillState : PlayerState
{
    private bool _isAwakenOn;
    public PlayerESkillState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
        
    }

    public override void Enter()
    {
        base.Enter();
        _player.StopImmediately(true);
        _isAwakenOn = false;

        Vector3 pos = _player.transform.position;
        pos.y += 1.5f;

        //EffectManager.Instance.PlayEffect(PoolingType.Effect_PlayerAwakening, pos);
    }

    public override void Exit()
    {
        base.Exit();
        _isAwakenOn = false;
    }

    public override void UpdateState()
    {
        base.UpdateState();

        if (_endTriggerCalled)
        {
            if (!_isAwakenOn)
            {
                _isAwakenOn = true;
                _player.StartCoroutine(PlayerAwakening());
            }

            _stateMachine.ChangeState(PlayerStateEnum.Idle);
        }
    }

    private IEnumerator PlayerAwakening()
    {
        while (_player.awakenCurrentGauge >= 0)
        {
            _player.IsAwakening = true;

            if (_player.IsAwakening)
            {
                _player.awakenCurrentGauge = Mathf.Clamp(_player.awakenCurrentGauge, 0, _player.awakenMaxGauge);
                _player.awakenCurrentGauge -= 10 * Time.deltaTime;
            }
            yield return null;
        }

        if (!_player.IsDie)
        {
            _player.IsAwakening = false;
            _stateMachine.ChangeState(PlayerStateEnum.Idle);
        }
    }

}
