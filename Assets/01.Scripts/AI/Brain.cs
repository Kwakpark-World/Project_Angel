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
    Sorcerer,
    Azazel
}

[RequireComponent(typeof(Rigidbody), typeof(NavMeshAgent))]
[RequireComponent(typeof(Debuff), typeof(EnemyAnimator))]
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
    public Debuff DebuffCompo { get; private set; }
    public EnemyAnimator AnimatorCompo { get; private set; }
    #endregion

    [field: SerializeField]
    public MonsterStat EnemyStatData { get; private set; }

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
        EnemyStatData.InitializeAllModifiers();

        normalAttackTimer = Time.time;
    }

    protected virtual void Initialize()
    {
        RigidbodyCompo = GetComponent<Rigidbody>();
        NavMeshAgentCompo = GetComponent<NavMeshAgent>();
        NavMeshAgentCompo.updateRotation = false;
        DebuffCompo =  GetComponent<Debuff>();

        DebuffCompo.SetOwner(this);

        AnimatorCompo = GetComponent<EnemyAnimator>();

        AnimatorCompo.SetOwner(this);

        EnemyStatData = Instantiate(EnemyStatData);

        EnemyStatData.SetOwner(this);

        NavMeshAgentCompo.speed = EnemyStatData.GetMoveSpeed();
    }

    public abstract void OnHit();

    public abstract void OnDie();
}
