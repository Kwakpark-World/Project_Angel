using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAwakenDashState : PlayerDashState
{
    public PlayerAwakenDashState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();

        _player.transform.position += _player.transform.forward * CalcDistance();
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

    private float CalcDistance()
    {
        float dashDistance = _player.PlayerStatData.GetDashMaxDistance();

        RaycastHit hit;
        if (Physics.Raycast(_player.transform.position, _player.transform.forward, out hit, _player.PlayerStatData.GetDashMaxDistance()))
        {
            dashDistance = hit.distance;
        }

        return dashDistance;
    }
}

