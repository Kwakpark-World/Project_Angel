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
        Vector3 playerPos = _player.transform.position;

        if (Physics.Raycast(playerPos, _player.transform.forward, out hit, _player.PlayerStatData.GetDashMaxDistance()))
        {
            dashDistance = hit.distance - 1;
        }

        Vector3 mouseTarget = _player.MousePosInWorld;
        float mousePosDist = Vector3.Distance(playerPos, _player.MousePosInWorld);

        mouseTarget.y = _player.transform.position.y + 0.1f;
        playerPos.y += 0.1f;
        if (Physics.Raycast(playerPos, mouseTarget, out hit, _player.PlayerStatData.GetDashMaxDistance()))
        {
            mousePosDist = hit.distance - 2;
        }


        dashDistance = Mathf.Min(dashDistance, mousePosDist);

        return dashDistance;
    }
}

