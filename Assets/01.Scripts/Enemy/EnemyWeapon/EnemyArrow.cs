using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyArrow : MonoBehaviour
{
    public Transform target; // 플레이어의 위치
    public float speed = 10f; // 화살의 속도
    private float gravity = 9.8f; // 중력 가속도
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // 플레이어를 향해 회전
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);

        // 포물선 운동
        Vector3 initialVelocity = CalculateInitialVelocity(target.position, transform.position, speed);
        rb.velocity = initialVelocity;
    }

    // 포물선 운동을 위한 초기 속도 계산
    Vector3 CalculateInitialVelocity(Vector3 targetPosition, Vector3 currentPosition, float speed)
    {
        float displacementY = targetPosition.y - currentPosition.y;
        float displacementXZ = Mathf.Sqrt((targetPosition - currentPosition).sqrMagnitude - displacementY * displacementY);

        // 수평 거리에 따른 시간 계산
        float time = displacementXZ / speed;

        // 수직 속도 계산
        float velocityY = displacementY / time - 0.5f * gravity * time;

        // 수평 속도 계산
        Vector3 velocityXZ = (targetPosition - currentPosition).normalized * speed;

        return velocityXZ + Vector3.up * velocityY;
    }
}
