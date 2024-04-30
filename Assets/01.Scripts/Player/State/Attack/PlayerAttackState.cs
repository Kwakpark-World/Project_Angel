using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : PlayerState
{
    protected Transform _weaponRT, _weaponRB, _weaponLT, _weaponLB;
    public float _hitDist = 4f;
    protected float _hitWidth = 6f;
    protected float _hitHeight = 3f;
    protected Vector3 _attackOffset = Vector3.zero;
    protected Vector3 _attackSize = Vector3.one;

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

    public void Attack(List<Collider> enemies, out HashSet<Brain> saves)
    {
        HashSet<Collider> enemyDuplicateCheck = new HashSet<Collider>();
        HashSet<Brain> enemySave = new HashSet<Brain>();

        foreach (var enemy in enemies)
        {
            if (enemyDuplicateCheck.Add(enemy))
            {
                if (enemy.transform.TryGetComponent<Brain>(out Brain brain))
                {
                    brain.OnHit(_player.PlayerStatData.GetAttackPower());

                    enemySave.Add(brain);

                    if (!_player.IsAwakening)
                        _player.awakenCurrentGauge++;
                }
            }
        }

        saves = enemySave;
    }

    public void Attack(List<RaycastHit> enemies, out HashSet<Brain> saves)
    {
        HashSet<RaycastHit> enemyDuplicateCheck = new HashSet<RaycastHit>();
        HashSet<Brain> enemySave = new HashSet<Brain>();

        foreach (var enemy in enemies)
        {
            if (enemyDuplicateCheck.Add(enemy))
            {
                if (enemy.transform.TryGetComponent<Brain>(out Brain brain))
                {
                    brain.OnHit(_player.PlayerStatData.GetAttackPower());

                    enemySave.Add(brain);

                    if (!_player.IsAwakening)
                        _player.awakenCurrentGauge++;
                }
            }
        }

        saves = enemySave;
    }

    public List<RaycastHit> GetEnemyByWeapon()
    {
        Vector3 rightDir = (_weaponRB.position - _weaponRT.position).normalized;
        Vector3 leftDir = (_weaponLB.position - _weaponLT.position).normalized;

        Vector3 weaponPos = _player._weapon.transform.position;

        List<RaycastHit> enemies = new List<RaycastHit>();
        RaycastHit[] enemiesR = Physics.RaycastAll(weaponPos, rightDir, _hitDist, _player._enemyLayer);
        RaycastHit[] enemiesL = Physics.RaycastAll(weaponPos, leftDir, _hitDist, _player._enemyLayer);

        foreach (var enemy in enemiesL) enemies.Add(enemy);
        foreach (var enemy in enemiesR) enemies.Add(enemy);

        return enemies;
    }
}
