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
    public EnemyType enemyTypes;
    public BehaviourTreeRunner treeRunner;
    public ParticleSystem HitParticle;

    #region Components
    public Rigidbody RigidbodyCompo { get; private set; }
    public NavMeshAgent NavMeshAgentCompo { get; private set; }
    public Debuff DebuffCompo { get; private set; }
    public EnemyAnimator AnimatorCompo { get; private set; }
    #endregion

    [field: SerializeField]
    public MonsterStat EnemyStatData { get; private set; }
    [field: SerializeField]
    public float CurrentHealth { get; set; }
    public float NormalAttackTimer { get; set; }

    protected virtual void Start()
    {
        Initialize();
    }

    protected virtual void Update()
    {
        float damage = 10;

        if ((GameManager.Instance.playerTransform.position - transform.position).sqrMagnitude <= EnemyStatData.GetAttackRange() * EnemyStatData.GetAttackRange())
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(GameManager.Instance.playerTransform.position - transform.position), EnemyStatData.GetRotateSpeed() * Time.deltaTime);
        }
        else if (NavMeshAgentCompo.hasPath)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(NavMeshAgentCompo.steeringTarget - transform.position), EnemyStatData.GetRotateSpeed() * Time.deltaTime);
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            OnHit(damage);
        }
    }

    public override void InitializePoolingItem()
    {
        EnemyStatData.InitializeAllModifiers();

        CurrentHealth = EnemyStatData.GetMaxHealth();
        NormalAttackTimer = Time.time;
    }

    protected virtual void Initialize()
    {
        RigidbodyCompo = GetComponent<Rigidbody>();
        NavMeshAgentCompo = GetComponent<NavMeshAgent>();
        NavMeshAgentCompo.updateRotation = false;
        DebuffCompo = GetComponent<Debuff>();

        DebuffCompo.SetOwner(this);

        AnimatorCompo = GetComponent<EnemyAnimator>();

        AnimatorCompo.SetOwner(this);

        EnemyStatData = Instantiate(EnemyStatData);

        EnemyStatData.SetOwner(this);

        NavMeshAgentCompo.speed = EnemyStatData.GetMoveSpeed();
    }

    public virtual void OnHit(float incomingDamage)
    {
        CurrentHealth -= Mathf.Max(incomingDamage - EnemyStatData.GetDefensivePower(), 0f);
        HitParticle.Play();

        if (CurrentHealth <= 0f)
        {
            OnDie();
            
        }

    }

    public virtual void OnDie()
    {
        PoolManager.Instance.Push(this);
    }
}
