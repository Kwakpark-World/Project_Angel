using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebuffPotion : PoolableMono
{
    [SerializeField]
    private DebuffType _debuffType;
    [SerializeField]
    private float _speed = 10f;
    public EnemyBrain owner;
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
        if (other.gameObject == GameManager.Instance.player.gameObject)
        {
            switch (_debuffType)
            {
                case DebuffType.Poison:
                    GameManager.Instance.player.DebuffCompo.SetDebuff(_debuffType, owner.DebuffCompo.DebuffStatData.poisonDuration, owner);

                    break;

                case DebuffType.Freeze:
                    GameManager.Instance.player.DebuffCompo.SetDebuff(_debuffType, owner.DebuffCompo.DebuffStatData.freezeDuration, owner);

                    break;

                case DebuffType.Knockback:
                    GameManager.Instance.player.DebuffCompo.SetDebuff(_debuffType, owner);

                    break;
            }

            PoolManager.Instance.Push(this);
        }
    }

    public override void InitializePoolingItem()
    {
        Vector3 direction = new Vector3(GameManager.Instance.playerTransform.position.x - transform.position.x, 0f, GameManager.Instance.playerTransform.position.z - transform.position.z).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = lookRotation;
    }
}