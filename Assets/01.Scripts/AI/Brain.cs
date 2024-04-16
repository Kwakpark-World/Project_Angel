using BTVisual;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody), typeof(NavMeshAgent))]
[RequireComponent(typeof(Debuff), typeof(EnemyAnimator))]
public abstract class Brain : PoolableMono
{
    public BehaviourTreeRunner treeRunner;

    #region Components
    public Rigidbody RigidbodyCompo { get; private set; }
    public NavMeshAgent NavMeshAgentCompo { get; private set; }
    public Debuff DebuffCompo { get; private set; }
    public EnemyAnimator AnimatorCompo { get; private set; }
    #endregion

    [field: SerializeField]
    public MonsterStat EnemyStatData { get; private set; }
    public float CurrentHealth { get; set; }
    public float NormalAttackTimer { get; set; }
    [HideInInspector]
    public EnemyMannequin enemySpawn;

    public List<Brain> nearbyEnemies = new List<Brain>();


    protected virtual void Start()
    {
        Initialize();
    }

    protected virtual void Update()
    {
        if (AnimatorCompo.GetCurrentAnimationState() == "Die")
        {
            return;
        }

        if ((GameManager.Instance.PlayerInstance.transform.position - transform.position).sqrMagnitude <= EnemyStatData.GetAttackRange() * EnemyStatData.GetAttackRange())
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(GameManager.Instance.PlayerInstance.transform.position - transform.position), EnemyStatData.GetRotateSpeed() * Time.deltaTime);
        }
        else if (NavMeshAgentCompo.hasPath)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(NavMeshAgentCompo.steeringTarget - transform.position), EnemyStatData.GetRotateSpeed() * Time.deltaTime);
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            OnHit(15f);
        }
    }

    public override void InitializePoolingItem()
    {
        if (NavMeshAgentCompo)
        {
            NavMeshAgentCompo.isStopped = false;
        }

        AnimatorCompo?.SetAnimationState();
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
        if (AnimatorCompo.GetCurrentAnimationState() == "Die")
        {
            return;
        }

        CurrentHealth -= Mathf.Max(incomingDamage - EnemyStatData.GetDefensivePower(), 0f);

        AnimatorCompo.SetAnimationState("Hit", AnimationStateMode.SavePreviousState);

        if (CurrentHealth <= 0f)
        {
            OnDie();
        }
    }

    public virtual void OnDie()
    {
        AnimatorCompo.SetAnimationState("Die");
    }

    public void FindNearbyEnemies()
    {
        nearbyEnemies.Clear(); 

        Brain[] allEnemies = FindObjectsOfType<Brain>();

        foreach (Brain enemy in allEnemies)
        {
            if (enemy != this && Vector3.Distance(transform.position, enemy.transform.position) < 7)
            {
                nearbyEnemies.Add(enemy);
            }
        }
    }

    public virtual void EnemyDistance(float minDistance)
    {
        List<Brain> enemiesCopy = new List<Brain>(nearbyEnemies); 

        foreach (Brain enemy in enemiesCopy)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < minDistance)
            {
                CurrentHealth -= Mathf.Max(1 - EnemyStatData.GetDefensivePower(), 0f);
                Debug.Log(CurrentHealth);
            }
        }
    }
}
