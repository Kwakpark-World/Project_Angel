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

    private LayerMask _trapLayer = LayerMask.GetMask("Trap");
    private HashSet<HitableTrap> _hitableTrapDuplicateChecker = new HashSet<HitableTrap>();

    public bool isCritical;

    public PlayerAttackState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        _weaponRT = _player.weapon.transform.Find("RightPointTop");
        _weaponRB = _player.weapon.transform.Find("RightPointBottom");
        _weaponLT = _player.weapon.transform.Find("LeftPointTop");
        _weaponLB = _player.weapon.transform.Find("LeftPointBottom");
    }

    public override void Exit()
    {
        base.Exit();

        EndHitTrap();
    }

    public override void UpdateState()
    {
        base.UpdateState();
        if (_player.IsPlayerStop == PlayerControlEnum.Stop) return;

        IsCritical();
        /*Gizmos.matrix = Matrix4x4.TRS(_player.transform.TransformPoint(_player.transform.position + _attackOffset), _player.transform.rotation, _player.transform.lossyScale);
        Gizmos.color = Color.white;
        Gizmos.DrawCube(-_player.transform.position + _attackOffset, _attackSize);
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(-_player.transform.position + _attackOffset, _attackSize);*/

        SetAttackSetting();

    }

    public void Attack(List<Collider> enemies)
    {
        float modifierValue = _player.PlayerStatData.GetAttackPower() * _player.PlayerStatData.GetCriticalDamageMultiplier();
        foreach (var enemy in enemies)
        {
            if (enemy.transform.TryGetComponent<Brain>(out Brain brain))
            {
                if (_player.enemyNormalHitDuplicateChecker.Add(brain))
                {
                    if (IsCritical())
                        _player.PlayerStatData.attackPower.AddModifier(modifierValue);

                    brain.OnHit(GetRandomDamage(), true, isCritical, _player.PlayerStatData.GetKnockbackPower());
                    HitEnemyAction(brain);

                    if (!_player.IsAwakened)
                    {
                        _player.CurrentAwakenGauge++;
                    }
                }
                isCritical = false;
                _player.PlayerStatData.attackPower.RemoveModifier(modifierValue);
            }
        }
    }

    public void Attack(List<RaycastHit> enemies)
    {
        float modifierValue = _player.PlayerStatData.GetAttackPower() * _player.PlayerStatData.GetCriticalDamageMultiplier() - _player.PlayerStatData.GetAttackPower();
        if (IsCritical())
        {
            _player.PlayerStatData.attackPower.AddModifier(modifierValue);
        }

        foreach (var enemy in enemies)
        {
            if (enemy.transform.TryGetComponent<Brain>(out Brain brain))
            {
                if (_player.enemyNormalHitDuplicateChecker.Add(brain))
                {
                    brain.OnHit(GetRandomDamage(), true, isCritical, _player.PlayerStatData.GetKnockbackPower());
                    HitEnemyAction(brain);

                    if (!_player.IsAwakened)
                    {
                        _player.CurrentAwakenGauge++;
                    }
                }
            }
        }


        isCritical = false;
        _player.PlayerStatData.attackPower.RemoveModifier(modifierValue);
    }

    public List<RaycastHit> GetEnemyByRaycast()
    {
        Vector3 dir = (_weaponRT.position - _weaponRB.position).normalized;

        Vector3 weaponRPos = _weaponRB.position;
        Vector3 weaponLPos = _weaponLB.position;

        RaycastHit[] enemiesR = Physics.RaycastAll(weaponRPos, dir, _hitDist, _player.enemyLayer);
        RaycastHit[] enemiesL = Physics.RaycastAll(weaponLPos, dir, _hitDist, _player.enemyLayer);

        List<RaycastHit> enemies = new List<RaycastHit>();

        foreach (var enemy in enemiesL) enemies.Add(enemy);
        foreach (var enemy in enemiesR) enemies.Add(enemy);

        return enemies;
    }

    public Collider[] GetEnemyByOverlapBox(Vector3 startPos, Quaternion direction)
    {
        Vector3 pos = startPos + _attackOffset;

        Vector3 halfSize = _attackSize * 0.5f;

        HitTrapByOverlapBox(startPos, direction);

        return Physics.OverlapBox(pos, halfSize, direction, _player.enemyLayer);
    }

    public void HitTrapByOverlapBox(Vector3 startPos, Quaternion direction)
    {
        Vector3 pos = startPos + _attackOffset;

        Vector3 halfSize = _attackSize * 0.5f;

        Collider[] traps = Physics.OverlapBox(pos, halfSize, direction, _trapLayer);

        foreach (var trap in traps)
        {
            if (trap.TryGetComponent<HitableTrap>(out HitableTrap hitTrap))
            {
                if (_hitableTrapDuplicateChecker.Add(hitTrap))
                {
                    hitTrap.HitTrap();
                }
            }
        }
    }

    private void EndHitTrap()
    {
        foreach (var trap in _hitableTrapDuplicateChecker)
        {
            trap.EndHit();
        }
        _hitableTrapDuplicateChecker.Clear();
    }

    private bool IsCritical()
    {
        float rand = Random.Range(0f, 100f);
        if (rand <= _player.PlayerStatData.GetCriticalChance())
        {
            isCritical = true;
            return true;
        }

        return false;
    }

    private float GetRandomDamage()
    {
        float upDownValuePer = 0.1f;
        upDownValuePer = Mathf.Clamp01(upDownValuePer);

        float value = _player.PlayerStatData.GetAttackPower() * upDownValuePer;

        return Random.Range(_player.PlayerStatData.GetAttackPower() - value, _player.PlayerStatData.GetAttackPower() + value);
    }

    protected virtual void HitEnemyAction(Brain enemy) { }

    protected virtual void SetAttackSetting() { }
}
