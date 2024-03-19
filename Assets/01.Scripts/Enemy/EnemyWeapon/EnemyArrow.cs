using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyArrow : PoolableMono
{
    public EnemyBrain owner;
    public float speed = 10f;
    private Rigidbody _rb;

    private void Update()
    {
        Vector3 direction = (GameManager.Instance.playerTransform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 7);

        Vector3 initialVelocity = CalculateInitialVelocity(GameManager.Instance.playerTransform.position, transform.position, speed);
        _rb.velocity = initialVelocity;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == GameManager.Instance.player.gameObject)
        {
            GameManager.Instance.player.PlayerStat.Hit(owner.EnemyStatistic.GetAttackPower());
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

    private Vector3 CalculateInitialVelocity(Vector3 targetPosition, Vector3 currentPosition, float speed)
    {
        Vector3 displacementXZ = new Vector3(targetPosition.x - currentPosition.x, 0, targetPosition.z - currentPosition.z);

        Vector3 velocityXZ = displacementXZ.normalized * speed;

        Vector3 initialVelocity = velocityXZ;

        return initialVelocity;
    }
}
