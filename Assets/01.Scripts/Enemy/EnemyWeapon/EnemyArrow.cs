using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyArrow : PoolableMono
{
    public float speed = 10f; // 화살의 속도
    private Rigidbody rb;
    private bool canDamage = true;

    public EnemyAI enemyAI;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // 플레이어를 향해 회전
        Vector3 direction = (GameManager.Instance.player.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 7); // 회전 속도를 고정값으로 설정

        // 일직선 운동
        Vector3 initialVelocity = CalculateInitialVelocity(GameManager.Instance.player.transform.position, transform.position, speed);
        rb.velocity = initialVelocity;
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
            PoolManager.instance.Push(this);
            if(canDamage)
            {
                if(GameManager.Instance.player != null)
                {
                    GameManager.Instance.player.PlayerStat.Hit(enemyAI.EnemyStatistic.GetAttackPower());
                }

                
            }
        }
    }

    public override void InitializePoolingItem()
    {

    }
}
