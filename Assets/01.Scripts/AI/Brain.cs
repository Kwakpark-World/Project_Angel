using BTVisual;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Brain : PoolableMono
{
    public BehaviourTreeRunner treeRunner;

    #region Components
    public Animator AnimatorCompo { get; private set; }
    public Rigidbody RigidbodyCompo { get; private set; }
    public NavMeshAgent NavMeshAgentCompo { get; private set; }
    #endregion

    [field: SerializeField] public MonsterStat EnemyStatistic { get; private set; }
    public float NormalAttackTimer { get; set; }

    protected virtual void Start()
    {
        Initialize();
    }

    protected abstract void Update();

    public override void InitializePoolingItem()
    {
        EnemyStatistic.InitializeAllModifiers();
        NormalAttackTimer = Time.time;
    }

    protected virtual void Initialize()
    {
        AnimatorCompo = GetComponent<Animator>();
        RigidbodyCompo = GetComponent<Rigidbody>();
        NavMeshAgentCompo = GetComponent<NavMeshAgent>();
        EnemyStatistic = Instantiate(EnemyStatistic);

        EnemyStatistic.SetOwner(this);
    }

    public abstract void OnHit();

    public abstract void OnDie();
}
