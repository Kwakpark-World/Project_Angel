using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyAreaPotion : PoolableMono
{
    [HideInInspector]
    public EnemyBrain owner;
    [SerializeField]
    private BuffType _potionBuffType;
    [SerializeField]
    private LayerMask _environmentLayer;
    [SerializeField]
    private ParticleSystem trail;
    [SerializeField]
    private int _lifetime = 5;
    [SerializeField]
    private float _speed = 10f;
    [HideInInspector]
    public Vector3 direction;
    [HideInInspector]
    public float speed;
    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        _rigidbody.velocity = direction * speed;
        gameObject.transform.position = trail.transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        Vector3 areaPos = transform.position;
        EnemyBuffArea buffArea = null;

        if ((1 << other.gameObject.layer & _environmentLayer) != 0)
        {
            areaPos = transform.position;
        }
        else if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, Mathf.Abs(other.transform.position.y) + 1f, _environmentLayer))
        {
            areaPos = hit.point;
        }

        switch (_potionBuffType)
        {
            case BuffType.Potion_Poison:
                buffArea = PoolManager.Instance.Pop(PoolType.Weapon_BuffArea_Poison, areaPos) as EnemyBuffArea;

                break;

            case BuffType.Potion_Freeze:
                buffArea = PoolManager.Instance.Pop(PoolType.Weapon_BuffArea_Freeze, areaPos) as EnemyBuffArea;

                break;

            case BuffType.Potion_Paralysis:
                buffArea = PoolManager.Instance.Pop(PoolType.Weapon_BuffArea_Paralysis, areaPos) as EnemyBuffArea;

                break;
        }

        buffArea.owner = owner;

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
                    GameManager.Instance.PlayerInstance.BuffCompo.PlayBuff(_potionBuffType, owner.BuffCompo.BuffStatData.paralysisDuration, owner);

                    break;
            }
        }

        PoolManager.Instance.Push(this);
    }

    public override void InitializePoolItem()
    {
        Quaternion lookRotation = Quaternion.LookRotation(-direction);
        transform.rotation = lookRotation;

        PoolManager.Instance.Push(this, _lifetime);
    }

    public void SetDefaultSpeed()
    {
        speed = _speed;
    }
}
