using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerEDashState : PlayerState
{
    private float _dashDistanceMax = 10f;

    public PlayerEDashState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();

        float dashDistance = _dashDistanceMax;

        RaycastHit hit;
        if (Physics.Raycast(_player.transform.position, _player.transform.forward, out hit, _dashDistanceMax))
        {
            dashDistance = hit.distance;
        }

        _player.transform.position += _player.transform.forward * dashDistance;
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

    
}

