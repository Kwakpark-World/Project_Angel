using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ENemyDebuff : PoolableMono
{
    public PlayerStat PlayerStat { get; private set; }

    private float poisonDamage = 2f;

    private float speed = 20f;
    private bool canDamage = false;
    private Rigidbody rb;
    public DebuffType _debuffType;

    public enum DebuffType
    {
        Slow,
        poison,
        push
    }

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

    public void PoisonPortion()
    {   
        StartCoroutine(PoisonDamage(1));   
    }

    public IEnumerator PoisonDamage(float time)
    {
        while(time < 15)
        {
            PlayerStat.Hit(poisonDamage);
            yield return new WaitForSeconds(time);
        }
    }

    public void SlowPortion()
    {
        float slowPos = 3f;
        PlayerStat.IncreaseStatBy(-slowPos, 3f, PlayerStat.GetStatByType(PlayerStatType.moveSpeed));           
    }

    public void PushPortion()
    {
        float slowPower = 3f;
        GameManager.Instance.player.RigidbodyCompo.AddForce(-GameManager.Instance.player.transform.forward * slowPower , ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PoolManager.instance.Push(this);
            if (canDamage)
            {
                if (GameManager.Instance.player != null)
                {
                    if (_debuffType == DebuffType.poison)
                    {
                        PoisonPortion();
                    }

                    else if (_debuffType == DebuffType.push)
                    {
                        PushPortion();
                    }

                    else if (_debuffType == DebuffType.Slow)
                    {
                        SlowPortion();
                    }

                    
                }
            }
            
        }
    }

   

    public override void InitializePoolingItem()
    {

    }
}