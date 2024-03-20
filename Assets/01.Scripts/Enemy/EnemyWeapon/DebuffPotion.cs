using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebuffPotion : PoolableMono
{
    [SerializeField]
    private DebuffType _debuffType;
    [SerializeField]
    private float _speed = 10f;
    [SerializeField]
    private float _rotateSpeed = 4f;
    [SerializeField]
    private float _poisonDuration = 3f;
    [SerializeField]
    private float _freezeDuration = 3f;
    [SerializeField]
    private float _knockbackForce = 5f;
    public EnemyBrain owner;
    private Rigidbody _rigidbody;

    private void Update()
    {
        // 일직선 운동
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
                    GameManager.Instance.player.PlayerStat.Debuff(_debuffType, _poisonDuration);

                    break;

                case DebuffType.Freeze:
                    GameManager.Instance.player.PlayerStat.Debuff(_debuffType, _freezeDuration);

                    break;

                case DebuffType.Knockback:
                    GameManager.Instance.player.RigidbodyCompo.AddForce((other.bounds.ClosestPoint(transform.position) - owner.transform.position).normalized * _knockbackForce, ForceMode.Impulse);

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

    // 일직선 운동을 위한 초기 속도 계산
    private Vector3 CalculateInitialVelocity(Vector3 targetPosition, Vector3 currentPosition, float speed)
    {
        // 수평 거리 계산
        Vector3 displacementXZ = new Vector3(targetPosition.x - currentPosition.x, 0, targetPosition.z - currentPosition.z);

        // 수평 속도 계산
        Vector3 velocityXZ = displacementXZ.normalized * speed;

        // 수직 속도는 고려하지 않음 (일직선 운동)
        Vector3 initialVelocity = velocityXZ;

        return initialVelocity;
    }
}