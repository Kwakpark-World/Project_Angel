using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyArrow : PoolableMono
{
    public float speed = 10f; // 화살의 속도
    private Rigidbody _rb;

    void Update()
    {
        // 플레이어를 향해 회전
        Vector3 direction = (GameManager.Instance.playerTransform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 11f); // 회전 속도를 고정값으로 설정

        // 일직선 운동
        Vector3 initialVelocity = CalculateInitialVelocity(GameManager.Instance.playerTransform.position, transform.position, speed);
        _rb.velocity = initialVelocity;
    }

    // 일직선 운동을 위한 초기 속도 계산
    Vector3 CalculateInitialVelocity(Vector3 targetPosition, Vector3 currentPosition, float speed)
    {
        // 수평 거리 계산
        Vector3 displacementXZ = new Vector3(targetPosition.x - currentPosition.x, 0, targetPosition.z - currentPosition.z);

        // 수평 속도 계산
        Vector3 velocityXZ = displacementXZ.normalized * speed;

        // 수직 속도는 고려하지 않음 (일직선 운동)
        Vector3 initialVelocity = velocityXZ;

        return initialVelocity;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PoolManager.Instance.Push(this);
        }
    }

    public override void InitializePoolingItem()
    {
        if (!_rb)
        {
            _rb = GetComponent<Rigidbody>();
        }
    }
}
