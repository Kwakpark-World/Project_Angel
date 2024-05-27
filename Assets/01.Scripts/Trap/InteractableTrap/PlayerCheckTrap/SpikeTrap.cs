using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrap : PlayerCheckTrap
{
    [Header("Check Parameter")]
    public Vector3 checkCenter;
    public Vector3 checkSize;
    public Vector3 checkRotation;

    [Header("Attack Parameter")]
    public Vector3 attackCenter;
    public Vector3 attackSize;
    public Vector3 attackRotation;


    private float _damage = 8f;

    protected override void StartTrap()
    {
        // 올라오기
        OnSpike();
    }

    protected override void PlayTrap()
    {
        AttackObject();
    }

    protected override void EndTrap()
    {
        OffSpike();
    }

    private void OnSpike()
    {
        
    }
    
    private void OffSpike()
    {

    }

    protected override void SetPlayerRangeParameter()
    {
        SetCheckParameter();
        SetAttackParameter();

        _trapDamage = _damage;
    }

    private void SetCheckParameter()
    {
        _playerCheckCenter = transform.position + checkCenter;
        _playerCheckHalfSize = checkSize / 2;
        _playerCheckRotation = transform.rotation * Quaternion.Euler(checkRotation);
    }
    
    private void SetAttackParameter()
    {
        _attackCenter = transform.position + attackCenter;
        _attackHalfSize = attackSize / 2;
        _attackRotation = transform.rotation * Quaternion.Euler(attackRotation);
    }

    private void OnDrawGizmos()
    {

        // PlayerCheck Range
        Gizmos.color = Color.green;

        Gizmos.matrix = Matrix4x4.Rotate(transform.rotation * Quaternion.Euler(checkRotation));

        Gizmos.DrawWireCube(transform.position + checkCenter, checkSize / 2);

        // PlayerAttack Range
        Gizmos.color = Color.red;

        Gizmos.matrix = Matrix4x4.Rotate(transform.rotation * Quaternion.Euler(attackRotation));

        Gizmos.DrawWireCube(transform.position + attackCenter, attackSize / 2);
    }


}
