using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEngine;

public class PlayerNormalChargeAttackState : PlayerChargeState
{
    float _normalDist = 5;
    float _awakenDist = 10;

    public PlayerNormalChargeAttackState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();

        _hitDist = _player.IsAwakening ? _awakenDist : _normalDist;

        // Default Speed + (0.GaugeAmount) * MultiplyValue
        // (0.GaugeAmount) = 0 ~ 0.1;
        _player.AnimatorCompo.speed = 1 + (_player.ChargingGauge / (_maxChargeTime * 10)) * _player.ChargingAttackSpeed;

    }

    public override void Exit()
    {
        base.Exit();

    }

    public override void UpdateState()
    {
        base.UpdateState();
        
        ChargeAttack();
        
        if (_endTriggerCalled)                                                                                
        {
            _stateMachine.ChangeState(PlayerStateEnum.NormalChargeStabAttack);
        }
    }

    private void ChargeAttack()
    {
        //Vector3 pos = _player.transform.position + (_player.transform.forward * _player.ChargingGage * _hitDistance / 4);
        //pos.y += 1f;
        //
        //Vector3 halfSize = new Vector3(0, 0, _player.ChargingGage * _hitDistance / 4);
        //Quaternion rot = Quaternion.Euler((_player.transform.forward * _player.ChargingGage * _hitDistance) - _player.transform.position);
        //
        //Collider[] enemies = Physics.OverlapBox(pos, halfSize, rot, _player._enemyLayer);
        //
        //Attack(enemies.ToList());

        List<RaycastHit> enemies = GetEnemyByWeapon();

        Attack(enemies);
    }

}
