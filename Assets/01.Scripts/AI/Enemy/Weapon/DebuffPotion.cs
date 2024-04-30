using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebuffPotion : PoolableMono
{
    [HideInInspector]
    public EnemyBrain owner;
    [SerializeField]
    private BuffType _debuffType;
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
            switch (_debuffType)
            {
                case BuffType.Poison:
                    GameManager.Instance.PlayerInstance.BuffCompo.PlayBuff(_debuffType, owner.BuffCompo.BuffStatData.poisonDuration, owner);

                    break;

                case BuffType.Freeze:
                    GameManager.Instance.PlayerInstance.BuffCompo.PlayBuff(_debuffType, owner.BuffCompo.BuffStatData.freezeDuration, owner);

                    break;

                case BuffType.Knockback:
                    GameManager.Instance.PlayerInstance.BuffCompo.PlayBuff(_debuffType, owner);

                    break;
            }

            PoolManager.Instance.Push(this);
        }
    }

    public override void InitializePoolingItem()
    {
        Vector3 direction = new Vector3(GameManager.Instance.PlayerInstance.transform.position.x - transform.position.x, 0f, GameManager.Instance.PlayerInstance.transform.position.z - transform.position.z).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = lookRotation;

        PoolManager.Instance.Push(this, _lifetime);
    }
}