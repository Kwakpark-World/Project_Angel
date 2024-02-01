using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator))]
public class EnemyAI : MonoBehaviour
{
    [Header("Range")]
    [SerializeField]
    float _detectRange = 10f;
    [SerializeField]
    float _meleeAttackRange = 5f;

    [Header("Enemy Stats")]
    public EnemyStats enemyStats;

    private float timer;

    Vector3 _originPos = default;
    BehaviorTreeRunner _BTRunner = null;
    Transform _detectedPlayer = null;
    Animator _animator;

    FakePlayer player;
    public Collider weaponCollider;

    private bool Isstride;
    Vector3 _lastKnownPlayerPos = default;
    private float _rotationSpeed = 10;
    float idleProbability = 0.1f;
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

    private void Awake()
    {
        _animator = GetComponent<Animator>();

        _BTRunner = new BehaviorTreeRunner(SettingBT());

        _originPos = transform.position;

        player = GetComponent<FakePlayer>();

        _navMeshAgent = GetComponent<NavMeshAgent>();

    }

    private void Start()
    {
        timer = Time.deltaTime;
    }

    private void Update()
    {
        if (enemyStats._currentHp <= 0)
        {
            _BTRunner.Operate();
            OnDieTrue();
            isDie = true;
        }

        if (enemyStats._currentHp < _previousHealth)
        {
            OnHitTrue();
            OnAttackFalse();
            isHit = true;
            _isMoving = false;
        }

        _BTRunner.Operate();

        _previousHealth = enemyStats._currentHp;
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
                transform.position = Vector3.MoveTowards(transform.position, randomPatrolPos, Time.deltaTime * enemyStats._moveSpeed);
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
            //이거

            if (isHit == false)
            {
                OnAttackTrue();
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

        if (_navMeshAgent.remainingDistance <= 0.1f)
        {
            OnIdleTrue();
            return INode.ENodeState.ENS_Success;
        }
        OnMoveTrue();
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

    private void OnDie()
    {
        if (enemyStats._currentHp <= 0)
        {
            OnDieTrue();
            deathPosition = transform.position;
        }
    }

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
        _animator.SetBool("Idle", isIdle);
        _animator.SetBool("Attack", isAttack);
        _animator.SetBool("Move", isMove);
        _animator.SetBool("Hit", isHit);
        _animator.SetBool("Die", isDie);
    }

    private void OnIdleTrue()
    {
        if (isDie == false)
            SetAnimatorBools(false, false, false, false, false);
    }

    private void OnAttackTrue()
    {
        if (isHit == false)
        {
            // 이동을 멈추도록 설정
            if (_navMeshAgent != null)
            {
                _navMeshAgent.isStopped = true;
            }

            _isMoving = false;

            SetAnimatorBools(false, true, false, false, false);
            
        }
    }

    private void OnMoveTrue()
    {
        SetAnimatorBools(false, false, true, false, false);
    }

    private void OnAttackFalse()
    {
        SetAnimatorBools(false, false, true, false, false);
        if (_navMeshAgent != null)
        {
            _navMeshAgent.isStopped = false;
        }

        _isMoving = true;
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
        if (enemyStats._currentHp <= 0)
        {
            SetAnimatorBools(false, false, false, false, true);
        }
        timer += Time.deltaTime;
        if (timer > 2.5f)
        {
            gameObject.SetActive(false);
        }
    }
    #endregion

    public void OnDamage(float damage)
    {
        damage = enemyStats._damage;
        enemyStats._currentHp -= damage;
    }
}
