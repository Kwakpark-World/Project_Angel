using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyArrow : MonoBehaviour
{
    public Transform target; // �÷��̾��� ��ġ
    public float speed = 10f; // ȭ���� �ӵ�
    private float gravity = 9.8f; // �߷� ���ӵ�
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // �÷��̾ ���� ȸ��
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);

        // ������ �
        Vector3 initialVelocity = CalculateInitialVelocity(target.position, transform.position, speed);
        rb.velocity = initialVelocity;
    }

    // ������ ��� ���� �ʱ� �ӵ� ���
    Vector3 CalculateInitialVelocity(Vector3 targetPosition, Vector3 currentPosition, float speed)
    {
        float displacementY = targetPosition.y - currentPosition.y;
        float displacementXZ = Mathf.Sqrt((targetPosition - currentPosition).sqrMagnitude - displacementY * displacementY);

        // ���� �Ÿ��� ���� �ð� ���
        float time = displacementXZ / speed;

        // ���� �ӵ� ���
        float velocityY = displacementY / time - 0.5f * gravity * time;

        // ���� �ӵ� ���
        Vector3 velocityXZ = (targetPosition - currentPosition).normalized * speed;

        return velocityXZ + Vector3.up * velocityY;
    }
}
