using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator))]
public class EnemyAI : Brain
{
    public EnemyType _enemyTypes;
    [Header("Range")]
    float _detectRange;
    float _meleeAttackRange;

    [Header("Movement")]
    float _movementSpeed;

    [Header("EnemyHP")]
    public float Enemy_MaxHp;
    public float Enemy_CurrentHp;
    
    public MonsterStat _enemyStat;

    [Header("Damage")]
    public float meleeAttackDamage;
    [Header("Partical")]
    public ParticleSystem Attack1Particle;

    Vector3 _originPos = default;
    BehaviorTreeRunner _BTRunner = null;
    Transform _detectedPlayer = null;

    public Collider weaponCollider;
    public GameObject WeaponSpawn;

    Vector3 _lastKnownPlayerPos = default;
    private float _rotationSpeed = 10;
    private Vector3 deathPosition;

    [Header("BoolValue")]
    private float _previousHealth;
    private bool isHit = false;
    private bool isDie = false;
    private bool _isMoving = false;

    //임시
    private bool isReturningToOrigin = false;
    private float elapsedTimeSinceReturn = 0f;
    NavMeshAgent _navMeshAgent;
    CharacterController _characterController;

    private float timer = 0;

    public enum EnemyType
    { 
        knight,
        archer
    }

    protected override void Start()
    {
        base.Start();

        Attack1Particle.Stop();
    }

    protected override void Update()
    {
        timer += Time.deltaTime;

        if (Enemy_CurrentHp <= 0)
        {
            _BTRunner.Operate();
            OnDieTrue();
            isDie = true;
        }

        if (Enemy_CurrentHp < _previousHealth)
        {
            OnHitTrue();
            OnAttackFalse();
            isHit = true;
            _isMoving = false;
        }

        _BTRunner.Operate();

        _previousHealth = Enemy_CurrentHp;
    }

    INode SettingBT()
    {
        return new SelectorNode
        (
            new List<INode>()
            {
            new SequenceNode
            (
                new List<INode>()
                {
                    new ActionNode(CheckEnemyWithinMeleeAttackRange),
                    new ActionNode(DoMeleeAttack),
                }
            ),
            new SequenceNode
            (
                new List<INode>()
                {
                    new ActionNode(CheckDetectEnemy),
                    new ActionNode(MoveToDetectEnemy),
                }
            ),
            new SelectorNode
            (
                new List<INode>()
                {
                    new ActionNode(CheckPatrol),
                    new ActionNode(MoveToLastKnownPlayerPos),
                    new ActionNode(MoveToOriginPosition),
                }
            ),

            }
        );
    }

    INode.ENodeState CheckPatrol()
    {
        // 플레이어가 감지되지 않았는지 확인
        if (_detectedPlayer == null && isDie == false)
        {
            // 플레이어를 감지한 후 3초 동안 감지되지 않으면 원래 위치로 돌아가기 시작
            if (isReturningToOrigin)
            {
                elapsedTimeSinceReturn += Time.deltaTime;

                if (elapsedTimeSinceReturn >= 3)
                {
                    isReturningToOrigin = false;

                    // 원래 위치로 돌아가는 로직 추가
                    //OnMoveTrue();
                    return MoveToOriginPosition();

                }

            }

            Vector3 patrolDirection = Vector3.zero;
            Vector3 randomPatrolPos = transform.position + patrolDirection.normalized * 10f;
            //OnIdleTrue();

            if (Vector3.SqrMagnitude(randomPatrolPos - transform.position) < float.Epsilon * float.Epsilon)
            {
                OnIdleTrue();
                return INode.ENodeState.ENS_Success;
            }
            else
            {
                OnDie();
                transform.position = Vector3.MoveTowards(transform.position, randomPatrolPos, Time.deltaTime * _movementSpeed);
                return INode.ENodeState.ENS_Running;
            }
        }
        else
        {
            // 플레이어가 감지됐으므로 이 부분의 동작은 실패로 반환
            elapsedTimeSinceReturn = 0f;
            isReturningToOrigin = false;
            return INode.ENodeState.ENS_Failure;
        }
    }

    #region Attack Node

    INode.ENodeState CheckEnemyWithinMeleeAttackRange()
    {
        if (_detectedPlayer != null)
        {
            if (Vector3.SqrMagnitude(_detectedPlayer.position - transform.position) < (_meleeAttackRange * _meleeAttackRange))
            {
                
                return INode.ENodeState.ENS_Success;
            }
        }

        return INode.ENodeState.ENS_Failure;
    }

    INode.ENodeState DoMeleeAttack()
    {
        if (_detectedPlayer != null)
        {
            
            if (isHit == false)
            {
   
                OnAttackTrue();
                if (_enemyTypes == EnemyType.archer && timer > 1f)
                {
                    GameObject EnemyArrow = GameManager.Instance.pool.GetEnemyArrow(0);

                    EnemyArrow.transform.position = WeaponSpawn.transform.position;
                    EnemyArrow.transform.rotation = WeaponSpawn.transform.rotation;
                    timer = 0;
                }
            }
            else
            {
                OnHitTrue();
                isHit = false;
            }

            OnDie();


            return INode.ENodeState.ENS_Success;
        }

        return INode.ENodeState.ENS_Failure;
    }
    #endregion

    #region Detect & Move Node
    INode.ENodeState CheckDetectEnemy()
    {
        var overlapColliders = Physics.OverlapSphere(transform.position, _detectRange, LayerMask.GetMask("Player"));
        if (overlapColliders != null && overlapColliders.Length > 0 && !isDie)
        {
            foreach (var collider in overlapColliders)
            {
                if (collider.gameObject.layer == LayerMask.NameToLayer("Player"))
                {
                    _detectedPlayer = collider.transform;

                    _lastKnownPlayerPos = _detectedPlayer.position;

                    // 원래 위치로 돌아가는 타이머 및 상태 초기화
                    elapsedTimeSinceReturn = 0f;
                    isReturningToOrigin = false;

                    RotateTowardsPlayer();
                    return INode.ENodeState.ENS_Success;
                }
            }
        }

        // Player가 감지되지 않았을 경우
        OnIdleTrue();
        _detectedPlayer = null;

        // 플레이어를 감지하지 않았을 때, 원래 위치로 돌아가는 상태 설정
        return MoveToOriginPosition();
    }


    private void RotateTowardsPlayer()
    {
        if (_detectedPlayer != null)
        {
            Vector3 directionToPlayer = _detectedPlayer.position - transform.position;

            if (directionToPlayer.magnitude > float.Epsilon)
            {
                Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);

                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * _rotationSpeed);
            }
        }
    }

    INode.ENodeState MoveToDetectEnemy()
    {
        if (_detectedPlayer != null)
        {
            _navMeshAgent.SetDestination(_detectedPlayer.position);

           
            if (Vector3.SqrMagnitude(_detectedPlayer.position - transform.position) < (_meleeAttackRange * _meleeAttackRange))
            {
                OnAttackTrue();
               
                return INode.ENodeState.ENS_Success;
            }

            OnAttackFalse();
            return INode.ENodeState.ENS_Running;
        }
        else
        {
            return INode.ENodeState.ENS_Failure;
        }
    }
    #endregion

    #region  Move Origin Pos Node
    INode.ENodeState MoveToOriginPosition()
    {
        _navMeshAgent.SetDestination(_originPos);

        if (_navMeshAgent.remainingDistance <= 0.1f && _isMoving)
        {
            OnIdleTrue();
            return INode.ENodeState.ENS_Success;
        }

        if (_isMoving)
        {
            OnMoveTrue();
        }

        return INode.ENodeState.ENS_Running;
    }



    INode.ENodeState MoveToLastKnownPlayerPos()
    {
        _navMeshAgent.SetDestination(_lastKnownPlayerPos);

        if (_navMeshAgent.remainingDistance < _navMeshAgent.stoppingDistance)
        {
            return INode.ENodeState.ENS_Success;
        }

        return INode.ENodeState.ENS_Failure;
    }
    #endregion

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(this.transform.position, _detectRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(this.transform.position, _meleeAttackRange);
    }
#endif

    #region Anmation

    private void SetAnimatorBools(bool isIdle, bool isAttack, bool isMove, bool isHit, bool isDie)
    {
        AnimatorCompo.SetBool("Idle", isIdle);
        AnimatorCompo.SetBool("Attack", isAttack);
        AnimatorCompo.SetBool("Move", isMove);
        AnimatorCompo.SetBool("Hit", isHit);
        AnimatorCompo.SetBool("Die", isDie);
    }

    private void OnIdleTrue()
    {
        if (isDie == false)
            SetAnimatorBools(false, false, false, false, false);
    }

    private bool isSoundPlayed = false;
    private void OnAttackTrue()
    {
        if (isHit == false)
        {
            SetAnimatorBools(false, true, false, false, false);

            if (!isSoundPlayed)
            {
                
                timer = 0;

                if (_enemyTypes == EnemyType.knight)
                {
                    SoundManager.Instance.PlayAttackSound("Attack1");
                }
                else if (_enemyTypes == EnemyType.archer)
                {
                    SoundManager.Instance.PlayAttackSound("Attack2");
                }
                Attack1Particle.Play();
                isSoundPlayed = true;
                Invoke("ResetSoundPlayed", 1.3f); 
            }

            if (_navMeshAgent != null)
            {
                _navMeshAgent.isStopped = true;
            }

            _isMoving = false;
        }
    }

    private void ResetSoundPlayed()
    {
        isSoundPlayed = false;
    }

    private void OnAttackFalse()
    {
        if (_enemyTypes == EnemyType.knight)
        {
            SoundManager.Instance.StopAttackSound("Attack1");
        }
        else if (_enemyTypes == EnemyType.archer )
        {
            SoundManager.Instance.StopAttackSound("Attack2");
            timer = 0;
        }
        SetAnimatorBools(false, false, true, false, false);

        // 이동을 다시 시작하도록 설정
        if (_navMeshAgent != null)
        {
            _navMeshAgent.isStopped = false;
        }

        _isMoving = true;
    }

    private void OnMoveTrue()
    {
        SetAnimatorBools(false, false, true, false, false);
    }

    private void OnHitTrue()
    {
        if (isHit == true)
        {
            SetAnimatorBools(false, false, false, true, false);
            _isMoving = false;
        }
    }

    public void OnDieTrue()
    {
        if (Enemy_CurrentHp <= 0)
        {
            SetAnimatorBools(false, false, false, false, true);
        }
    }
    #endregion

    protected override void Initialize()
    {
        base.Initialize();

        _BTRunner = new BehaviorTreeRunner(SettingBT());

        _originPos = transform.position;

        _navMeshAgent = GetComponent<NavMeshAgent>();

        _enemyStat = _enemyStat as MonsterStat;

        Enemy_CurrentHp = _enemyStat.GetMaxHealthValue();
        _detectRange = _enemyStat.GetDetectRange();
        _meleeAttackRange = _enemyStat.GetAttackRange();
        meleeAttackDamage = _enemyStat.GetDamage();
        _movementSpeed = _enemyStat.GetMoveSpeed();
    }

    public override void OnHit(float damage)
    {
        Enemy_CurrentHp -= damage;
    }

    public override void OnDie()
    {
        if (Enemy_CurrentHp <= 0)
        {
            OnDieTrue();
            deathPosition = transform.position;
        }
    }
}
