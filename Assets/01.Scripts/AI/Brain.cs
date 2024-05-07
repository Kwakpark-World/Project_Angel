using BTVisual;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody), typeof(NavMeshAgent))]
[RequireComponent(typeof(Buff), typeof(EnemyAnimator))]
public abstract class Brain : PoolableMono
{
    public BehaviourTreeRunner treeRunner;

    #region Components
    public Rigidbody RigidbodyCompo { get; private set; }
    public NavMeshAgent NavMeshAgentCompo { get; private set; }
    public Buff BuffCompo { get; private set; }
    public EnemyAnimator AnimatorCompo { get; private set; }
    #endregion

    [field: SerializeField]
    public MonsterStat EnemyStatData { get; private set; }
    public float CurrentHealth { get; set; }
    public float NormalAttackTimer { get; set; }
    public float SkillAttackTimer { get; set; }
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

        // Debug
        #region Debug
        if (Keyboard.current.lKey.wasPressedThisFrame)
        {
            OnHit(15f);
        }

        if (Keyboard.current.mKey.wasPressedThisFrame)
        {
            BuffCompo.PlayBuff(BuffType.Shield, 3f, this);
        }
        #endregion
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
        SkillAttackTimer = Time.time;
    }

    protected virtual void Initialize()
    {
        RigidbodyCompo = GetComponent<Rigidbody>();
        NavMeshAgentCompo = GetComponent<NavMeshAgent>();
        NavMeshAgentCompo.updateRotation = false;
        BuffCompo = GetComponent<Buff>();

        CurrentHealth = EnemyStatData.GetMaxHealth();

        BuffCompo.SetOwner(this);

        AnimatorCompo = GetComponent<EnemyAnimator>();

        AnimatorCompo.SetOwner(this);

        EnemyStatData = Instantiate(EnemyStatData);

        EnemyStatData.SetOwner(this);

        NavMeshAgentCompo.speed = EnemyStatData.GetMoveSpeed();
    }

    public virtual void OnHit(float incomingDamage)
    {
        if(BuffCompo.GetBuffState(BuffType.Shield))
        {
            return;
        }

        if (AnimatorCompo.GetCurrentAnimationState() == "Die")
        {
            return;
        }

        CurrentHealth -= Mathf.Max(incomingDamage - EnemyStatData.GetDefensivePower(), 0f);

        AnimatorCompo.SetAnimationState("Hit", AnimationStateMode.SavePreviousState);

        if (GameManager.Instance.PlayerInstance.BuffCompo.GetBuffState(BuffType.Rune_Debuff_Synergy))
        {
            float previousSpeed = AnimatorCompo._animator.speed;

            AnimatorCompo._animator.speed = 0;

            StartCoroutine(RestoreAnimationSpeedAfterDelay(previousSpeed, 2f));
        }

        if (CurrentHealth <= 0f)
        {
            OnDie();
        }
    }

    IEnumerator RestoreAnimationSpeedAfterDelay(float previousSpeed, float delay)
    {
        yield return new WaitForSeconds(delay);

        AnimatorCompo._animator.speed = previousSpeed;
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
            }
        }
    }
}
