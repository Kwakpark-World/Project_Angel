using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyArrow : PoolableMono
{
    [HideInInspector]
    public EnemyBrain owner;
    [SerializeField]
    private int _lifetime = 5;
    [SerializeField]
    private float _speed = 10f;
    [SerializeField]
    private float _rotateSpeed = 100f;
    private Transform _particleTransform;
    private Rigidbody _rigidbody;

    private void Awake()
    {
        _particleTransform = transform.GetChild(0);
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        _particleTransform.Rotate(-_particleTransform.forward, _rotateSpeed * Time.deltaTime);

        _rigidbody.velocity = -transform.forward * _speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == GameManager.Instance.PlayerInstance.gameObject)
        {
            GameManager.Instance.PlayerInstance.OnHit(owner.EnemyStatData.GetAttackPower());
            PoolManager.Instance.Push(this);
        }

        // If arrow collision with environment, push to pool this.
    }

    public override void InitializePoolingItem()
    {
        Vector3 direction = new Vector3(GameManager.Instance.PlayerInstance.transform.position.x - transform.position.x, 0f, GameManager.Instance.PlayerInstance.transform.position.z - transform.position.z).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(-direction);
        transform.rotation = lookRotation;

        PoolManager.Instance.Push(this, _lifetime);
    }
}
