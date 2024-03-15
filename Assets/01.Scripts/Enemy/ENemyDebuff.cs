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
        // 플레이어를 향해 회전
        Vector3 direction = (GameManager.Instance.player.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 4); // 회전 속도를 고정값으로 설정

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