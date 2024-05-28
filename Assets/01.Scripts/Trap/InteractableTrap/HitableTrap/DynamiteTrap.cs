using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamiteTrap : HitableTrap
{
    public Vector3 center;
    public Vector3 size;
    public Vector3 rotation;

    private float _damage = 15f;

    protected override void StartTrap()
    {
        // 터지는 사운드나 이펙트?

        base.StartTrap();
    }

    protected override void PlayTrap()
    {
        AttackObject();

        base.PlayTrap();
    }


    protected override void EndTrap()
    {
        PoolManager.Instance.Push(this);

        EndHit();
        base.EndTrap();
    }

    protected override void SetPlayerRangeParameter()
    {
        _attackCenter = transform.position + center;
        _attackHalfSize = size / 2;
        _attackRotation = transform.rotation * Quaternion.Euler(rotation);

        _trapDamage = _damage;
    }

    private void OnDrawGizmos()
    {

        // Attack Range
        Gizmos.color = Color.red;

        Gizmos.matrix = Matrix4x4.Rotate(transform.rotation * Quaternion.Euler(rotation));

        Gizmos.DrawWireCube(transform.position + center, size);
    }
}
