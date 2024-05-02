using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PlayerAttackState : PlayerState
{
    protected Transform _weaponRT, _weaponRB, _weaponLT, _weaponLB;
    public float _hitDist = 4f;
    protected float _hitWidth = 6f;
    protected float _hitHeight = 3f;
    protected Vector3 _attackOffset = Vector3.zero;
    protected Vector3 _attackSize = Vector3.one;

    private float _shieldEnemyCheckDist = 30f;
    private const string _shieldTagString = "Shield";

    public PlayerAttackState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        SetAttackSetting();

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
        foreach (var enemy in enemies)
        {
            if (enemy.transform.TryGetComponent<Brain>(out Brain brain))
            {
                if (_player.enemyNormalHitDuplicateChecker.Add(brain))
                {
                    brain.OnHit(_player.PlayerStatData.GetAttackPower());

                    if (!_player.IsAwakening)
                        _player.awakenCurrentGauge++;
                }
            }
        }
    }

    public void Attack(List<RaycastHit> enemies)
    {
        foreach (var enemy in enemies)
        {
            if (enemy.transform.TryGetComponent<Brain>(out Brain brain))
            {
                if (_player.enemyNormalHitDuplicateChecker.Add(brain))
                {
                    brain.OnHit(_player.PlayerStatData.GetAttackPower());

                    if (!_player.IsAwakening)
                        _player.awakenCurrentGauge++;
                }
            }
        }
    }

    public List<RaycastHit> GetEnemyByWeapon()
    {
        // 상시 레이캐스트 올 쏘고 거기 방패병 있으면 방패병 뒤 부터 쭉 데미지 받는 곳에서 뺴주기

        Vector3 dir = (_weaponRT.position - _weaponRB.position).normalized;

        Vector3 weaponRPos = _weaponRB.position;
        Vector3 weaponLPos = _weaponLB.position;

        RaycastHit[] enemiesR = Physics.RaycastAll(weaponRPos, dir, _hitDist, _player._enemyLayer);
        RaycastHit[] enemiesL = Physics.RaycastAll(weaponLPos, dir, _hitDist, _player._enemyLayer);

        List<RaycastHit> enemies = new List<RaycastHit>();
        
        foreach (var enemy in enemiesL) enemies.Add(enemy);
        foreach (var enemy in enemiesR) enemies.Add(enemy);

        ShieldEnemyCheck();

        return enemies;
    }

    public void ShieldEnemyCheck()
    {
        Vector3 pos = _player.transform.position;
        Vector3 dir = _player.transform.forward;

        int index = 0;
        RaycastHit[] enemies = Physics.RaycastAll(pos, dir, _shieldEnemyCheckDist, _player._enemyLayer);
        for (int i = enemies.Length - 1; i >= 0; i--)
        {
            index = i;
            
            if (enemies[i].collider.gameObject.CompareTag(_shieldTagString))
            {
                if (enemies[i].transform.TryGetComponent<Brain>(out Brain brain))
                    _player.enemyNormalHitDuplicateChecker.Add(brain);

                break;
            }

            if (index == 0) return;
        }

        for (int i = 0; i < index; i++)
        {
            if (enemies[i].transform.TryGetComponent<Brain>(out Brain brain))
            {
                _player.enemyNormalHitDuplicateChecker.Add(brain);
            }
        }
    }

    public Collider[] GetEnemyByRange(Vector3 startPos, Vector3 direction)
    {
        Vector3 pos = startPos + _attackOffset;

        Vector3 halfSize = _attackSize * 0.5f;

        Quaternion rot = Quaternion.Euler((direction * _hitDist) - startPos).normalized;

        return Physics.OverlapBox(pos, halfSize, rot, _player._enemyLayer);
    }

    protected virtual void SetAttackSetting(){}
}
