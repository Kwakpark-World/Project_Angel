using BTVisual;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyType
{
    Knight,
    Archer,
    Witch,
    Sorcerer
}

[RequireComponent(typeof(Rigidbody), typeof(NavMeshAgent), typeof(EnemyAnimator))]
public abstract class Brain : PoolableMono
{
    private static float _rotateSpeed = 10f;

    public EnemyType enemyTypes;
    public BehaviourTreeRunner treeRunner;
    public ParticleSystem HitParticle;
    [HideInInspector]
    public float normalAttackTimer;

    #region Components
    public Rigidbody RigidbodyCompo { get; private set; }
    public NavMeshAgent NavMeshAgentCompo { get; private set; }
    public EnemyAnimator AnimatorCompo { get; private set; }
    #endregion

    [field: SerializeField] public MonsterStat EnemyStatistic { get; private set; }

    protected virtual void Start()
    {
        Initialize();
    }

    protected virtual void Update()
    {
        if (NavMeshAgentCompo.hasPath)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(NavMeshAgentCompo.steeringTarget - transform.position), _rotateSpeed * Time.deltaTime);
        }
    }

    public override void InitializePoolingItem()
    {
        EnemyStatistic.InitializeAllModifiers();

        normalAttackTimer = Time.time;
    }

    protected virtual void Initialize()
    {
        RigidbodyCompo = GetComponent<Rigidbody>();
        NavMeshAgentCompo = GetComponent<NavMeshAgent>();
        AnimatorCompo = GetComponent<EnemyAnimator>();
        EnemyStatistic = Instantiate(EnemyStatistic);

        EnemyStatistic.SetOwner(this);

        NavMeshAgentCompo.speed = EnemyStatistic.GetMoveSpeed();
        NavMeshAgentCompo.updateRotation = false;
    }

    public abstract void OnHit();

    public abstract void OnDie();
}
