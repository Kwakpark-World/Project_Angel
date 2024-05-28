using BTVisual;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody), typeof(NavMeshAgent))]
[RequireComponent(typeof(Buff), typeof(EnemyAnimator))]
public abstract class Brain : PoolableMono
{
    static private List<Brain> enemyChain = new List<Brain>();

    public BehaviourTreeRunner treeRunner;
    public Transform enemyCenter;

    #region Components
    public Rigidbody RigidbodyCompo { get; private set; }
    public NavMeshAgent NavMeshAgentCompo { get; private set; }
    public Buff BuffCompo { get; private set; }
    public EnemyAnimator AnimatorCompo { get; private set; }
    #endregion

    [field: SerializeField]
    public EnemyStat EnemyStatData { get; private set; }
    public float CurrentHealth { get; set; }
    public float NormalAttackTimer { get; set; }
    public float SkillAttackTimer { get; set; }
    [HideInInspector]
    public EnemyMannequin enemySpawn;

    protected virtual void Start()
    {
        Initialize();
    }

    protected virtual void Update()
    {
        if (AnimatorCompo.GetCurrentAnimationState("Die"))
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
            Debug.Log("5");
        }
        #endregion
    }

    public override void InitializePoolItem()
    {
        base.InitializePoolItem();

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

    public virtual void OnHit(float incomingDamage, bool isHitPhysically = false, float knockbackPower = 0f)
    {
        if (BuffCompo.GetBuffState(BuffType.Shield))
        {
            return;
        }

        if (AnimatorCompo.GetCurrentAnimationState("Die"))
        {
            return;
        }

        CurrentHealth -= Mathf.Max(incomingDamage - EnemyStatData.GetDefensivePower(), 0f);

        AnimatorCompo.SetAnimationState("Hit", AnimatorCompo.GetCurrentAnimationState("Hit") ? AnimationStateMode.None : AnimationStateMode.SavePreviousState);

        if (isHitPhysically)
        {
            StartCoroutine(Knockback(knockbackPower));
        }

        if (CurrentHealth <= 0f)
        {
            OnDie();
        }
    }

    public virtual void OnDie()
    {
        AnimatorCompo.SetAnimationState("Die");
    }

    public IEnumerator Knockback(float knockbackPower)
    {
        Vector3 knockbackDirection = (transform.position - GameManager.Instance.PlayerInstance.playerCenter.position).normalized;
        knockbackDirection.y = 0f;
        float timer = 0f;
        float knockbackDuration = GameManager.Instance.PlayerInstance.PlayerStatData.GetKnockbackDuration();

        while (timer <= knockbackDuration)
        {
            transform.position = Vector3.Lerp(transform.position, transform.position + knockbackDirection * knockbackPower, timer);
            timer += Time.deltaTime;

            yield return null;
        }
    }

    public List<Brain> FindNearbyEnemies(int maxEnemyAmount, float nearbyRange)
    {
        if (maxEnemyAmount < 1 || nearbyRange <= 0f)
        {
            return null;
        }

        List<Brain> enemies = FindObjectsOfType<Brain>().OrderBy(enemy => (transform.position - enemy.transform.position).sqrMagnitude).ToList();

        if (enemies.Count < 2)
        {
            return null;
        }

        enemies.RemoveAt(0);

        int maxEnemy = enemies.FindIndex(enemy => (transform.position - enemy.transform.position).sqrMagnitude > (nearbyRange * nearbyRange));

        return enemies.GetRange(0, Mathf.Min(maxEnemyAmount, maxEnemy == -1 ? enemies.Count : maxEnemy));
    }

    public List<Brain> FindAllNearbyEnemies(float nearbyRange)
    {
        if (nearbyRange <= 0f)
        {
            return null;
        }

        List<Brain> enemies = FindObjectsOfType<Brain>().OrderBy(enemy => (transform.position - enemy.transform.position).sqrMagnitude).ToList();

        if (enemies.Count < 2)
        {
            return null;
        }

        enemies.RemoveAt(0);

        int maxEnemy = enemies.FindIndex(enemy => (transform.position - enemy.transform.position).sqrMagnitude > (nearbyRange * nearbyRange));

        return maxEnemy == -1 ? enemies : enemies.GetRange(0, maxEnemy);
    }

    public List<Brain> ChainNearbyEnemies(int maxEnemyAmount, float nearbyRange, int enemyCount = 1)
    {
        if (enemyCount == 1)
        {
            enemyChain.Clear();
        }

        enemyChain.Add(this);

        if (enemyCount == maxEnemyAmount)
        {
            return enemyChain;
        }

        List<Brain> nearbyEnemies = FindAllNearbyEnemies(nearbyRange);

        if (nearbyEnemies != null && nearbyEnemies.Count > 0)
        {
            foreach (Brain nearbyEnemy in nearbyEnemies)
            {
                if (!enemyChain.Contains(nearbyEnemy))
                {
                    return nearbyEnemy.ChainNearbyEnemies(maxEnemyAmount, nearbyRange, enemyCount + 1);
                }
            }
        }

        return enemyChain;
    }
}
