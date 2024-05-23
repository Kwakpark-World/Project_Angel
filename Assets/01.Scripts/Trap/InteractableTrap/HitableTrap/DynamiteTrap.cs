using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamiteTrap : HitableTrap
{
    public Vector3 center;
    public Vector3 size;
    public Vector3 rotation;

    private float _damage = 15f;

    protected override void Update()
    {
        base.Update();
    }

    protected override void StartTrap()
    {
        base.StartTrap();

        // ���峪 ����Ʈ?
    }

    protected override void PlayTrap()
    {
        base.PlayTrap();

        // ���� ���� �ȿ� ������Ʈ �����ͼ� �� �ְ�
    }

    protected override void EndTrap()
    {
        base.EndTrap();
        PoolManager.Instance.Push(this);

        EndHit();
    }

    protected override void SetPlayerAttackParameter()
    {
        _playerAttackCenter = transform.position + center;
        _playerAttackHalfSize = size / 2;
        _playerAttackRotation = transform.rotation * Quaternion.Euler(rotation);

        _trapDamage = _damage;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.matrix = Matrix4x4.Rotate(transform.rotation * Quaternion.Euler(rotation));

        Gizmos.DrawWireCube(transform.position + center, size / 2);
    }
}
