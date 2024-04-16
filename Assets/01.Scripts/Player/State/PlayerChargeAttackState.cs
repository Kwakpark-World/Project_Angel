using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChargeAttackState : PlayerState
{
    float useDist = 5;
    float defaultDist = 5;
    float awakenDist = 10;

    public PlayerChargeAttackState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();


        useDist = _player.IsAwakening ? awakenDist : defaultDist;
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
        Vector3 pos = _player.transform.position + (_player.transform.forward * _player.ChargingGage * useDist / 4);
        pos.y += 1f;

        Vector3 halfSize = new Vector3(0, 0, _player.ChargingGage * useDist / 4);
        Quaternion rot = Quaternion.Euler((_player.transform.forward * _player.ChargingGage * useDist) - _player.transform.position);

        Collider[] enemies = Physics.OverlapBox(pos, halfSize, rot, _player._enemyLayer);

        HashSet<Collider> enemyDuplicateCheck = new HashSet<Collider>();

        foreach (var enemy in enemies)
        {
            if (enemyDuplicateCheck.Add(enemy))
            {
                if (enemy.transform.TryGetComponent<Brain>(out Brain brain))
                {
                    Debug.Log($"hit {enemy.transform.gameObject}");

                    brain.OnHit(_player.attackPower);
                    if (!_player.IsAwakening)
                        _player.awakenCurrentGage++;
                }
            }
        }
    }
}
