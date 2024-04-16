using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : PlayerState
{
    protected Transform _weaponRT, _weaponRB, _weaponLT, _weaponLB;
    
    public PlayerAttackState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        _weaponRT = _player._weapon.transform.Find("RightPointTop");
        _weaponRB = _player._weapon.transform.Find("RightPointBottom");
        _weaponLT = _player._weapon.transform.Find("LeftPointTop");
        _weaponLB = _player._weapon.transform.Find("LeftPointBottom");


    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void UpdateState()
    {
        base.UpdateState();
    }

    public void Attack(List<Collider> enemies)
    {
        HashSet<Collider> enemyDuplicateCheck = new HashSet<Collider>();

        foreach (var enemy in enemies)
        {
            if (enemyDuplicateCheck.Add(enemy))
            {
                if (enemy.transform.TryGetComponent<Brain>(out Brain brain))
                {
                    brain.OnHit(_player.attackPower);
                    if (!_player.IsAwakening)
                        _player.awakenCurrentGage++;
                }
            }
        }
    }

    public void Attack(List<RaycastHit> enemies)
    {
        HashSet<RaycastHit> enemyDuplicateCheck = new HashSet<RaycastHit>();

        foreach (var enemy in enemies)
        {
            if (enemyDuplicateCheck.Add(enemy))
            {
                if (enemy.transform.TryGetComponent<Brain>(out Brain brain))
                {
                    brain.OnHit(_player.attackPower);
                    if (!_player.IsAwakening)
                        _player.awakenCurrentGage++;
                }
            }
        }
    }
}
