using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ENemyDebuff : PoolableMono
{
    private float poisonDamage = 2f;

    private float speed = 10f;

    public EnemyAI enemyAI;

    private bool canDamage = false;
    private Rigidbody rb;
    public DebuffType _debuffType;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // �÷��̾ ���� ȸ��
        Vector3 direction = (GameManager.Instance.player.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 4); // ȸ�� �ӵ��� ���������� ����

        // ������ �
        Vector3 initialVelocity = CalculateInitialVelocity(GameManager.Instance.player.transform.position, transform.position, speed);
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
        float KnockBackPower = 5f;
        if (other.CompareTag("Player"))
        {
            if (canDamage == false)
            {
                if (GameManager.Instance.player != null)
                {
                    if (_debuffType == DebuffType.Poison)
                    {
                        GameManager.Instance.player.PlayerStat.Debuff(_debuffType,3);
                    }

                    else if (_debuffType == DebuffType.Knockback)
                    {
                        GameManager.Instance.player.RigidbodyCompo.AddForce((other.bounds.ClosestPoint(transform.position) - enemyAI.transform.position).normalized * KnockBackPower, ForceMode.Impulse);
                    }

                    else if (_debuffType == DebuffType.Freeze)
                    {
                        GameManager.Instance.player.PlayerStat.Debuff(_debuffType, 3);
                    }
                }
            }
            PoolManager.instance.Push(this);

        }
    }

    public override void InitializePoolingItem()
    {

    }
}