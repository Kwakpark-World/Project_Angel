using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyArrow : PoolableMono
{
    public EnemyBrain enemyBrain;
    public float speed = 10f; // ȭ���� �ӵ�
    private Rigidbody _rb;
    private bool _canDamage = true;

    void Update()
    {
        // �÷��̾ ���� ȸ��
        Vector3 direction = (GameManager.Instance.playerTransform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 7); // ȸ�� �ӵ��� ���������� ����

        // ������ �
        Vector3 initialVelocity = CalculateInitialVelocity(GameManager.Instance.playerTransform.position, transform.position, speed);
        _rb.velocity = initialVelocity;
    }

    // ������ ��� ���� �ʱ� �ӵ� ���
    Vector3 CalculateInitialVelocity(Vector3 targetPosition, Vector3 currentPosition, float speed)
    {
        // ���� �Ÿ� ���
        Vector3 displacementXZ = new Vector3(targetPosition.x - currentPosition.x, 0, targetPosition.z - currentPosition.z);

        // ���� �ӵ� ���
        Vector3 velocityXZ = displacementXZ.normalized * speed;

        // ���� �ӵ��� �������� ���� (������ �)
        Vector3 initialVelocity = velocityXZ;

        return initialVelocity;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == GameManager.Instance.player.gameObject)
        {
            if(_canDamage)
            {
                if(GameManager.Instance.player != null)
                {
                    GameManager.Instance.player.PlayerStat.Hit(enemyBrain.EnemyStatistic.GetAttackPower());
                }
            }
        }
        
        PoolManager.Instance.Push(this);
    }

    public override void InitializePoolingItem()
    {
        if (!_rb)
        {
            _rb = GetComponent<Rigidbody>();
        }
    }
}
