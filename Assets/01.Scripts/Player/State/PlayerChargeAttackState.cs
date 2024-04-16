using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEngine;

public class PlayerChargeAttackState : PlayerAttackState
{
    float defaultDist = 5;
    float awakenDist = 10;

    public PlayerChargeAttackState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();

        _hitDistance = _player.IsAwakening ? awakenDist : defaultDist;
        ChargeAttack();

    }
    public override void Exit()
    {
        _player.ChargingGage = 0;

        base.Exit();
    }
    public override void UpdateState()
    {
        base.UpdateState();
        if (_endTriggerCalled)                                                                                
        {
            _stateMachine.ChangeState(PlayerStateEnum.ChargeStabAttack);
        }
    }

    private void ChargeAttack()
    {
        Vector3 pos = _player.transform.position + (_player.transform.forward * _player.ChargingGage * _hitDistance / 4);
        pos.y += 1f;

        Vector3 halfSize = new Vector3(0, 0, _player.ChargingGage * _hitDistance / 4);
        Quaternion rot = Quaternion.Euler((_player.transform.forward * _player.ChargingGage * _hitDistance) - _player.transform.position);

        Collider[] enemies = Physics.OverlapBox(pos, halfSize, rot, _player._enemyLayer);
        
        Attack(enemies.ToList());
    }
}
