using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPotion : PoolableMono
{
    [HideInInspector]
    public EnemyBrain owner;
    [SerializeField]
    private BuffType _potionBuffType;
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
            switch (_potionBuffType)
            {
                case BuffType.Potion_Poison:
                    GameManager.Instance.PlayerInstance.BuffCompo.PlayBuff(_potionBuffType, owner.BuffCompo.BuffStatData.poisonDuration, owner);

                    break;

                case BuffType.Potion_Freeze:
                    GameManager.Instance.PlayerInstance.BuffCompo.PlayBuff(_potionBuffType, owner.BuffCompo.BuffStatData.freezeDuration, owner);

                    break;

                case BuffType.Potion_Paralysis:
                    GameManager.Instance.PlayerInstance.BuffCompo.PlayBuff(_potionBuffType, owner);

                    break;
            }

            PoolManager.Instance.Push(this);
        }
    }

    public override void InitializePoolItem()
    {
        Vector3 direction = new Vector3(GameManager.Instance.PlayerInstance.transform.position.x - transform.position.x, 0f, GameManager.Instance.PlayerInstance.transform.position.z - transform.position.z).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = lookRotation;

        PoolManager.Instance.Push(this, _lifetime);
    }
}