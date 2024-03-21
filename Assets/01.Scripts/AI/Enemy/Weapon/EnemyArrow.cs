using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyArrow : PoolableMono
{
    [SerializeField]
    private float _speed = 10f;
    [SerializeField]
    private float _rotateSpeed = 100f;
    public EnemyBrain owner;
    private Rigidbody _rigidbody;

    private void Update()
    {
        Vector3 initialVelocity = CalculateInitialVelocity(GameManager.Instance.playerTransform.position, transform.position, _speed);
        _rigidbody.velocity = initialVelocity;

        transform.Rotate(transform.forward, _rotateSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == GameManager.Instance.player.gameObject)
        {
            GameManager.Instance.player.PlayerStatData.Hit(owner.EnemyStatData.GetAttackPower());
        }

        PoolManager.Instance.Push(this);
    }

    public override void InitializePoolingItem()
    {
        if (!_rigidbody)
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        Vector3 direction = (GameManager.Instance.playerTransform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = lookRotation;
    }

    private Vector3 CalculateInitialVelocity(Vector3 targetPosition, Vector3 currentPosition, float speed)
    {
        Vector3 displacementXZ = new Vector3(targetPosition.x - currentPosition.x, 0, targetPosition.z - currentPosition.z);

        Vector3 velocityXZ = displacementXZ.normalized * speed;

        Vector3 initialVelocity = velocityXZ;

        return initialVelocity;
    }
}
