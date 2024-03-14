using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyType
{
    knight,
    archer,
    witcher
}

public class EnemyBrain : Brain
{
    public EnemyType _enemyTypes;

    [Header("Range")]
    private float _detectRange;
    private float _attackRange;

    [Header("Movement")]
    private float _movementSpeed;

    [Header("Health")]
    private float _enemyMaxHealth;

    [Header("Damage")]
    private float _attackPower;

    [Header("Particle")]
    public ParticleSystem Attack1Particle;

    private Vector3 _originPos = default;
    private BehaviorTreeRunner _BTRunner = null;
    private Transform _detectedPlayer = null;

    public Collider weaponCollider;
    public GameObject weaponSpawn;

    private Vector3 _lastKnownPlayerPos = default;
    private Vector3 _deathPosition;
    private float _rotationSpeed = 10;
    private float _previousHealth;

    [Header("BoolValue")]
    private bool _isHit = false;
    private bool _isDie = false;
    private bool _isAttack = false;
    private bool _isMoving = false;
    private bool _isReload = false;

    //�ӽ�
    private bool _isReturningToOrigin = false;
    private float _elapsedTimeSinceReturn = 0f;

    private float _timer = 0;

    protected override void Start()
    {
        base.Start();

        Attack1Particle.Stop();
    }

    protected override void Update()
    {
        _previousHealth = EnemyStatistic.GetCurrentHealth();
        _timer += Time.deltaTime;

        _BTRunner.Operate();
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
        // �÷��̾ �������� �ʾҴ��� Ȯ��
        if (_detectedPlayer == null && _isDie == false)
        {
            // �÷��̾ ������ �� 3�� ���� �������� ������ ���� ��ġ�� ���ư��� ����
            if (_isReturningToOrigin)
            {
                _elapsedTimeSinceReturn += Time.deltaTime;

                if (_elapsedTimeSinceReturn >= 3)
                {
                    _isReturningToOrigin = false;

                    // ���� ��ġ�� ���ư��� ���� �߰�
                    //OnMoveTrue();
                    return MoveToOriginPosition();

                }

            }

            Vector3 patrolDirection = Vector3.zero;
            Vector3 randomPatrolPos = transform.position + patrolDirection.normalized * 10f;
            OnIdleTrue();

            if (Vector3.SqrMagnitude(randomPatrolPos - transform.position) < float.Epsilon * float.Epsilon)
            {
                OnIdleTrue();
                return INode.ENodeState.ENS_Success;
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, randomPatrolPos, Time.deltaTime * _movementSpeed);
                return INode.ENodeState.ENS_Running;
            }
        }
        else
        {
            // �÷��̾ ���������Ƿ� �� �κ��� ������ ���з� ��ȯ
            _elapsedTimeSinceReturn = 0f;
            _isReturningToOrigin = false;
            return INode.ENodeState.ENS_Failure;
        }
    }

    #region Attack Node

    INode.ENodeState CheckEnemyWithinMeleeAttackRange()
    {
        if (_detectedPlayer != null)
        {
            if (Vector3.SqrMagnitude(_detectedPlayer.position - transform.position) < (_attackRange * _attackRange))
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
            
            if (_isHit == false)
            {
                if(_enemyTypes == EnemyType.knight || _enemyTypes == EnemyType.witcher)
                {
                    OnAttackTrue();
                }

                else if (_enemyTypes == EnemyType.archer && _timer > 1f)
                {
                    // 장전 후에 공격 상태로 전환
                    OnReload();
                }
            }
            else
            {
                //OnHitTrue();
                _isHit = false;
            }


            return INode.ENodeState.ENS_Success;
        }

        return INode.ENodeState.ENS_Failure;
    }
    #endregion

    #region Detect & Move Node
    INode.ENodeState CheckDetectEnemy()
    {
        var overlapColliders = Physics.OverlapSphere(transform.position, _detectRange, LayerMask.GetMask("Player"));
        if (overlapColliders != null && overlapColliders.Length > 0 && !_isDie)
        {
            foreach (var collider in overlapColliders)
            {
                if (collider.gameObject.layer == LayerMask.NameToLayer("Player"))
                {
                    _detectedPlayer = collider.transform;

                    _lastKnownPlayerPos = _detectedPlayer.position;

                    // ���� ��ġ�� ���ư��� Ÿ�̸� �� ���� �ʱ�ȭ
                    _elapsedTimeSinceReturn = 0f;
                    _isReturningToOrigin = false;

                    RotateTowardsPlayer();
                    return INode.ENodeState.ENS_Success;
                }
            }
        }

        // Player�� �������� �ʾ��� ���
        OnIdleTrue();
        _detectedPlayer = null;

        // �÷��̾ �������� �ʾ��� ��, ���� ��ġ�� ���ư��� ���� ����
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
            base.NavMeshAgentCompo.SetDestination(_detectedPlayer.position);

            if (Vector3.SqrMagnitude(_detectedPlayer.position - transform.position) < (_attackRange * _attackRange))
            {

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
        base.NavMeshAgentCompo.SetDestination(_originPos);

        if (base.NavMeshAgentCompo.remainingDistance <= 0.1f && _isMoving)
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
        NavMeshAgentCompo.SetDestination(_lastKnownPlayerPos);

        if (NavMeshAgentCompo.remainingDistance < NavMeshAgentCompo.stoppingDistance)
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
        Gizmos.DrawWireSphere(this.transform.position, _attackRange);
    }
#endif

    #region Anmation
    private void SetAnimatorBools(bool isIdle, bool isAttack, bool isMove, bool isHit, bool isDie, bool isLoad)
    {
        AnimatorCompo.SetBool("Idle", isIdle);
        AnimatorCompo.SetBool("Attack", isAttack);
        AnimatorCompo.SetBool("Move", isMove);
        AnimatorCompo.SetBool("Hit", isHit);
        AnimatorCompo.SetBool("Die", isDie);
        AnimatorCompo.SetBool("load", isLoad);
    }

    private void OnIdleTrue()
    {
        if (_isDie == false)
            SetAnimatorBools(false, false, false, false, false, false);
        NavMeshAgentCompo.isStopped = true;
    }

    private bool isSoundPlayed = false;
    private void OnAttackTrue()
    {
        
        if (_isHit == false)
        {
            SetAnimatorBools(false, true, false, false, false, false);

            if (!isSoundPlayed)
            {
                
                _timer = 0;

                if (_enemyTypes == EnemyType.knight)
                {
                    SoundManager.Instance.PlayAttackSound("Attack1");
                }
                else if (_enemyTypes == EnemyType.archer)
                {
                    SoundManager.Instance.PlayAttackSound("Attack2");

                    // 공격 전에 화살 생성
                    EnemyArrow EnemyArrow = PoolManager.instance.Pop(PoolingType.Arrow) as EnemyArrow;
                    EnemyArrow.enemyAI = this;
                    EnemyArrow.transform.position = WeaponSpawn.transform.position;
                    EnemyArrow.transform.rotation = WeaponSpawn.transform.rotation;

                }

                else if(_enemyTypes == EnemyType.witcher)
                {
                    SoundManager.Instance.PlayAttackSound("Attack3");

                    // 공격 전에 화살 생성
                    PoolableMono EnemyArrow = PoolManager.instance.Pop(PoolingType.Porison);
                    EnemyArrow.transform.position = WeaponSpawn.transform.position;
                    EnemyArrow.transform.rotation = WeaponSpawn.transform.rotation;
                }
                Attack1Particle.Play();
                isSoundPlayed = true;
                Invoke("ResetSoundPlayed", 1.3f); 
            }

            NavMeshAgentCompo.isStopped = true;
            _isMoving = false;
        }
    }

    private void ResetSoundPlayed()
    {
        isSoundPlayed = false;
    }


    private void OnReload()
    {
        if (_isHit == false && _isReload == false)
        {
            SetAnimatorBools(false, false, false, false, false, true); // 이동 애니메이션 제어
            NavMeshAgentCompo.isStopped = true; // 이동 중지 상태로 변경

            isReload = true;
            _timer = 0;
        }

        if (_isReload == true)
        {
            // 장전이 완료되면 Attack 메서드가 호출되어야 함
            float reloadAnimationLength = 0.8f;

            StartCoroutine(WaitAndAttack(reloadAnimationLength));
            _isReload = false;
        }
    }

    private IEnumerator WaitAndAttack(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        // 장전 완료 후 바로 공격 상태로 전환
        OnAttackTrue();

        // 공격 후 재장전
        yield return new WaitForSeconds(0.65f); // 재장전 애니메이션 시간동안 기다림
        OnReload();
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
            _timer = 0;
        }

        else if (_enemyTypes == EnemyType.witcher)
        {
            SoundManager.Instance.StopAttackSound("Attack3");
            timer = 0;
        }
        SetAnimatorBools(false, false, true, false, false, false);

        // �̵��� �ٽ� �����ϵ��� ����
        NavMeshAgentCompo.isStopped = false;
        _isMoving = true;
    }

    private void OnMoveTrue()
    {
        SetAnimatorBools(false, false, true, false, false, false);
    }

    private void OnHitTrue()
    {
        if (_isHit == true)
        {
            SetAnimatorBools(false, false, false, true, false, false);
            _isMoving = false;
        }
    }

    public void OnDieTrue()
    {
        SetAnimatorBools(false, false, false, false, true, false);
    }
    #endregion

    public override void InitializePoolingItem()
    {
        base.InitializePoolingItem();

        _originPos = transform.position;
    }

    protected override void Initialize()
    {
        base.Initialize();

        _BTRunner = new BehaviorTreeRunner(SettingBT());

        _enemyMaxHealth = EnemyStatistic.GetMaxHealthValue();
        _detectRange = EnemyStatistic.GetDetectRange();
        _attackRange = EnemyStatistic.GetAttackRange();
        _attackPower = EnemyStatistic.GetAttackPower();
        _movementSpeed = EnemyStatistic.GetMoveSpeed();
    }

    public override void OnHit()
    {
        if (EnemyStatistic.GetCurrentHealth() <= 0f)
        {
            OnDie();
        }
    }

    public override void OnDie()
    {
        _deathPosition = transform.position;

        OnDieTrue();
    }
}
