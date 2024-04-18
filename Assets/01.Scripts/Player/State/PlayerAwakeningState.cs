using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAwakeningState : PlayerState
{
    private bool _isAwakenOn;
    public PlayerAwakeningState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
        
    }

    public override void Enter()
    {
        base.Enter();
        _player.StopImmediately(true);
        _isAwakenOn = false;
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
            OnAwakening();

            _stateMachine.ChangeState(PlayerStateEnum.Idle);
        }
    }

    private void OnAwakening()
    {
        if (_isAwakenOn) return;

        _isAwakenOn = true;
        _player.StartCoroutine(PlayerAwakening());
    }

    private IEnumerator PlayerAwakening()
    {
        while (_player.awakenCurrentGauge >= 0)
        {
            _player.IsAwakening = true;

            if (_player.IsAwakening)
            {
                _player.awakenCurrentGauge = Mathf.Clamp(_player.awakenCurrentGauge, 0, _player.PlayerStatData.GetMaxAwakenGauge());
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
