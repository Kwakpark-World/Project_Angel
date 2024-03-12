using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyArrow : PoolableMono
{
    private Transform target; // �÷��̾��� ��ġ
    public float speed = 10f; // ȭ���� �ӵ�
    private Rigidbody rb;

    void Start()
    {
        target = GameManager.Instance.player.transform;
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // �÷��̾ ���� ȸ��
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 7); // ȸ�� �ӵ��� ���������� ����

        // ������ �
        Vector3 initialVelocity = CalculateInitialVelocity(target.position, transform.position, speed);
        rb.velocity = initialVelocity;
    }

    // ������ ��� ���� �ʱ� �ӵ� ���
    Vector3 CalculateInitialVelocity(Vector3 targetPosition, Vector3 currentPosition, float speed)
    {
        // ���� �Ÿ� ���
        Vector3 displacementXZ = new Vector3(targetPosition.x - currentPosition.x, 0, targetPosition.z - currentPosition.z);

        // ���� �ӵ� ���
        Vector3 velocityXZ = displacementXZ.normalized * speed;

        // ���� �ӵ��� ������� ���� (������ �)
        Vector3 initialVelocity = velocityXZ;

        return initialVelocity;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PoolManager.instance.Push(this);
        }
    }

    public override void InitializePoolingItem()
    {

    }
}
