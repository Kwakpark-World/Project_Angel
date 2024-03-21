using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebuffPotion : PoolableMono
{
    [SerializeField]
    private DebuffType _debuffType;
    [SerializeField]
    private float _speed = 10f;
    public EnemyBrain owner;
    private Rigidbody _rigidbody;

    private void Update()
    {
        // ������ �
        Vector3 initialVelocity = CalculateInitialVelocity(GameManager.Instance.playerTransform.position, transform.position, _speed);
        _rigidbody.velocity = initialVelocity;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == GameManager.Instance.player.gameObject)
        {
            switch (_debuffType)
            {
                case DebuffType.Poison:
                    GameManager.Instance.player.DebuffCompo.SetDebuff(_debuffType, owner.DebuffCompo.DebuffStatData.poisonDuration, owner);

                    break;

                case DebuffType.Freeze:
                    GameManager.Instance.player.DebuffCompo.SetDebuff(_debuffType, owner.DebuffCompo.DebuffStatData.freezeDuration, owner);

                    break;

                case DebuffType.Knockback:
                    GameManager.Instance.player.DebuffCompo.SetDebuff(_debuffType, owner);

                    break;
            }

            PoolManager.Instance.Push(this);
        }
    }

    public override void InitializePoolingItem()
    {
        if (!_rigidbody)
        {
            _rigidbody = GetComponent<Rigidbody>();
        }
    }

    // ������ ��� ���� �ʱ� �ӵ� ���
    private Vector3 CalculateInitialVelocity(Vector3 targetPosition, Vector3 currentPosition, float speed)
    {
        // ���� �Ÿ� ���
        Vector3 displacementXZ = new Vector3(targetPosition.x - currentPosition.x, 0, targetPosition.z - currentPosition.z);

        // ���� �ӵ� ���
        Vector3 velocityXZ = displacementXZ.normalized * speed;

        // ���� �ӵ��� ������� ���� (������ �)
        Vector3 initialVelocity = velocityXZ;

        return initialVelocity;
    }
}