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

    private bool _isAttackSetting = false;

    public PlayerAttackState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _isAttackSetting = false;

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
        
        /*Gizmos.matrix = Matrix4x4.TRS(_player.transform.TransformPoint(_player.transform.position + _attackOffset), _player.transform.rotation, _player.transform.lossyScale);
        Gizmos.color = Color.white;
        Gizmos.DrawCube(-_player.transform.position + _attackOffset, _attackSize);
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(-_player.transform.position + _attackOffset, _attackSize);*/

        if (!_isAttackSetting)
        {
            _isAttackSetting = true;
            SetAttackSetting();
        }
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
        Vector3 dir = (_weaponRT.position - _weaponRB.position).normalized;

        Vector3 weaponRPos = _weaponRB.position;
        Vector3 weaponLPos = _weaponLB.position;

        RaycastHit[] enemiesR = Physics.RaycastAll(weaponRPos, dir, _hitDist, _player._enemyLayer);
        RaycastHit[] enemiesL = Physics.RaycastAll(weaponLPos, dir, _hitDist, _player._enemyLayer);

        List<RaycastHit> enemies = new List<RaycastHit>();
        
        foreach (var enemy in enemiesL) enemies.Add(enemy);
        foreach (var enemy in enemiesR) enemies.Add(enemy);

        return enemies;
    }

    public Collider[] GetEnemyByRange(Vector3 startPos, Quaternion direction)
    {
        Vector3 pos = startPos + _attackOffset;

        Vector3 halfSize = _attackSize * 0.5f;

        return Physics.OverlapBox(pos, halfSize, direction, _player._enemyLayer);
    }

    protected virtual void SetAttackSetting(){}
}
