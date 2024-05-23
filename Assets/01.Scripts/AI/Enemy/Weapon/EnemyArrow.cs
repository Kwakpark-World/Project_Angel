using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyArrow : PoolableMono
{
    [HideInInspector]
    public EnemyBrain owner;
    [SerializeField]
    private LayerMask _environmentLayer;
    [SerializeField]
    private int _lifetime = 5;
    [SerializeField]
    private float _speed = 10f;
    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        _rigidbody.velocity = transform.forward * _speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == GameManager.Instance.PlayerInstance.gameObject)
        {
            GameManager.Instance.PlayerInstance.OnHit(owner.EnemyStatData.GetAttackPower());
            PoolManager.Instance.Push(this);
        }
        else if ((1 << other.gameObject.layer & _environmentLayer) != 0)
        {
            PoolManager.Instance.Push(this);
        }
    }

    public override void InitializePoolItem()
    {
        //Vector3 direction = new Vector3(GameManager.Instance.PlayerInstance.transform.position.x - transform.position.x, 0f, GameManager.Instance.PlayerInstance.transform.position.z - transform.position.z).normalized;
        Vector3 direction = (GameManager.Instance.PlayerInstance.playerCenter.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = lookRotation;

        PoolManager.Instance.Push(this, _lifetime);
    }
}
