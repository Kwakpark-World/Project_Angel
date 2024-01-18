using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EnemyAI : MonoBehaviour
{
    [Header("Range")]
    [SerializeField]
    float _detectRange = 10f;
    [SerializeField]
    float _meleeAttackRange = 5f;

    [Header("Movement")]
    [SerializeField]
    float _movementSpeed = 10f;

    [Header("EnemyHP")]
    [SerializeField]
    public float Enemy_MaxHp;
    [SerializeField]
    public float Enemy_CurrentHp;

    [Header("stride")]
    float patrolTimer = 0f;
    float patrolDuration = 2f;
    bool isMovingRight = true;

    [Header("Damage")]
    public float meleeAttackDamage = 10;

    [Header("Idle")]
    float idleTimer = 0f;
    float idleDuration = 2f;

    Vector3 _originPos = default;
    BehaviorTreeRunner _BTRunner = null;
    Transform _detectedPlayer = null;
    Animator _animator = null;

    //Player player;
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

    private void Awake()
    {
        _animator = GetComponent<Animator>();

        _BTRunner = new BehaviorTreeRunner(SettingBT());

        _originPos = transform.position;

        //player = GetComponent<Player>();

        _previousHealth = Enemy_CurrentHp;
    }

    private void Update()
    {
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
                }
            ),
            new ActionNode(MoveToOriginPosition),
            }
        );
    }

    INode.ENodeState CheckPatrol()
    {
        // 플레이어가 감지되지 않았는지 확인
        if (_detectedPlayer == null && isDie == false)
        {
            // IDLE 타이머 갱신
            if (idleTimer > 0f)
            {
                idleTimer -= Time.deltaTime;

                // IDLE 타이머가 종료되면 다시 움직임
                if (idleTimer <= 0f)
                {
                    OnMoveTrue();
                    return INode.ENodeState.ENS_Running;
                }

                // 아직 IDLE 상태 유지 중
                OnIdleTrue();
                return INode.ENodeState.ENS_IDLE;
            }

            patrolTimer += Time.deltaTime;

            // 정찰 지속 시간 확인
            if (patrolTimer >= patrolDuration)
            {
                // 랜덤한 확률로 IDLE 상태로 전환
                if (Random.Range(0f, 1f) < idleProbability)
                {
                    OnIdleTrue();  // IDLE 상태로 전환
                    idleTimer = idleDuration;  // IDLE 타이머 설정
                    return INode.ENodeState.ENS_IDLE;
                }

                // 방향 전환 및 타이머 초기화
                isMovingRight = !isMovingRight;
                patrolTimer = 0f;

                // 이동 방향에 맞춰 캐릭터 회전
                if (isMovingRight)
                {
                    // 오른쪽으로 회전
                    transform.rotation = Quaternion.Euler(0f, 450, 0);
                }
                else
                {
                    // 왼쪽으로 회전
                    transform.rotation = Quaternion.Euler(0f, 270, 0);
                }
            }



            // 현재 방향으로 이동
            Vector3 patrolDirection = isMovingRight ? Vector3.right : Vector3.left;
            //patrolDirection.z = 0;  // Z축 고정

            Vector3 randomPatrolPos = transform.position + patrolDirection.normalized * 10f;
            OnMoveTrue();

            if (Vector3.SqrMagnitude(randomPatrolPos - transform.position) < float.Epsilon * float.Epsilon)
            {
                // 이미 무작위 정찰 위치에 도달함
                return INode.ENodeState.ENS_Success;
            }
            else
            {
                OnDie();
                transform.position = Vector3.MoveTowards(transform.position, randomPatrolPos, Time.deltaTime * _movementSpeed);
                return INode.ENodeState.ENS_Running;
            }
        }

        // 플레이어가 감지됐으므로 이 부분의 동작은 실패로 반환
        return INode.ENodeState.ENS_Failure;
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

                    RotateTowardsPlayer();
                    return INode.ENodeState.ENS_Success;
                }
            }
        }

        // Player가 감지되지 않았을 경우
        _detectedPlayer = null;
        return INode.ENodeState.ENS_Failure;
    }


    private void RotateTowardsPlayer()
    {
        if (_detectedPlayer != null)
        {
            Vector3 directionToPlayer = _detectedPlayer.position - transform.position;

            if (directionToPlayer.magnitude > float.Epsilon)
            {
                Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
                targetRotation.x = 0;
                targetRotation.z = 0;
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * _rotationSpeed);
            }
        }
    }

    INode.ENodeState MoveToDetectEnemy()
    {
        if (_detectedPlayer != null)
        {
            if (Vector3.SqrMagnitude(_detectedPlayer.position - transform.position) < (_meleeAttackRange * _meleeAttackRange))
            {
                return INode.ENodeState.ENS_Success;
            }

            // 움직일 때 SetBool 호출
            OnAttackFalse();

            transform.position = Vector3.MoveTowards(transform.position, _detectedPlayer.position, Time.deltaTime * _movementSpeed);

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
        if (Vector3.SqrMagnitude(_originPos - transform.position) < float.Epsilon * float.Epsilon)
        {
            return INode.ENodeState.ENS_Success;
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, _originPos, Time.deltaTime * _movementSpeed);
            return INode.ENodeState.ENS_Failure;
        }
    }

    INode.ENodeState MoveToLastKnownPlayerPos()
    {
        if (Vector3.SqrMagnitude(_lastKnownPlayerPos - transform.position) < float.Epsilon * float.Epsilon)
        {
            return INode.ENodeState.ENS_Success;
        }
        else
        {
            OnIdleTrue();
            transform.position = Vector3.MoveTowards(transform.position, _lastKnownPlayerPos, Time.deltaTime * _movementSpeed);
            return INode.ENodeState.ENS_Failure;
        }
    }
    #endregion

    private void OnDie()
    {
        if (Enemy_CurrentHp <= 0)
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
            SetAnimatorBools(true, false, false, false, false);
    }

    public void OnAttackTrue()
    {
        if (isHit == false)
        {
            SetAnimatorBools(false, true, false, false, false);
            _isMoving = false;
        }
    }

    private void OnMoveTrue()
    {
        SetAnimatorBools(false, false, true, false, false);
    }

    private void OnAttackFalse()
    {
        SetAnimatorBools(false, false, true, false, false);
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
        if (Enemy_CurrentHp <= 0)
        {
            SetAnimatorBools(false, false, false, false, true);
        }
    }
    #endregion

    public void OnDamage(float damage)
    {
        Enemy_CurrentHp -= damage;
    }
}
