using System;
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
    float _enemyMaxHealth;

    [Header("Damage")]
    float _meleeAttackDamage;
    [Header("Particle")]
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
    private bool isReload = false;

    //�ӽ�
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
        float currentHP = EnemyStatistic.GetCurrentHealth();
        timer += Time.deltaTime;

        _BTRunner.Operate();

        _previousHealth = currentHP;

        if (_enemyTypes == EnemyType.archer && timer > 1f)
        {
            if (isHit == false)
            {
                OnReload();
               
            }
        }

        // Debug
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //OnHit(10f);
        }


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
        if (_detectedPlayer == null && isDie == false)
        {
            // �÷��̾ ������ �� 3�� ���� �������� ������ ���� ��ġ�� ���ư��� ����
            if (isReturningToOrigin)
            {
                elapsedTimeSinceReturn += Time.deltaTime;

                if (elapsedTimeSinceReturn >= 3)
                {
                    isReturningToOrigin = false;

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
                if(_enemyTypes == EnemyType.knight)
                {
                    OnAttackTrue();
                }
                
                else if (_enemyTypes == EnemyType.archer && timer > 1f)
                {
                    OnAttackTrue();

                    PoolableMono EnemyArrow = PoolManager.Instance.Pop(PoolingType.Arrow);
                    EnemyArrow.transform.position = WeaponSpawn.transform.position;
                    EnemyArrow.transform.rotation = WeaponSpawn.transform.rotation;
                    timer = 0;
                }
            }
            else
            {
                //OnHitTrue();
                isHit = false;
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
        if (overlapColliders != null && overlapColliders.Length > 0 && !isDie)
        {
            foreach (var collider in overlapColliders)
            {
                if (collider.gameObject.layer == LayerMask.NameToLayer("Player"))
                {
                    _detectedPlayer = collider.transform;

                    _lastKnownPlayerPos = _detectedPlayer.position;

                    // ���� ��ġ�� ���ư��� Ÿ�̸� �� ���� �ʱ�ȭ
                    elapsedTimeSinceReturn = 0f;
                    isReturningToOrigin = false;

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
            _navMeshAgent.SetDestination(_detectedPlayer.position);

            if (Vector3.SqrMagnitude(_detectedPlayer.position - transform.position) < (_meleeAttackRange * _meleeAttackRange))
            {
               /*Debug.Log("55");
                OnAttackTrue();
                if(_enemyTypes == EnemyType.knight)
                {
                    OnAttackTrue();
                    Debug.Log("2");
                }
                else
                {
                    OnReload();
                    Debug.Log("2.5");
                }*/

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
        if (isDie == false)
            SetAnimatorBools(false, false, false, false, false, false);
    }

    private bool isSoundPlayed = false;
    private void OnAttackTrue()
    {
        
        if (isHit == false )
        {
            SetAnimatorBools(false, true, false, false, false, false);

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
            isReload = false;
        }
    }

    private void ResetSoundPlayed()
    {
        isSoundPlayed = false;
    }

    private void OnReload()
    {
        if (isHit == false && isReload == false)
        {
            SetAnimatorBools(false, false, false, false, false, true); // �̵� �ִϸ��̼� ���

            _navMeshAgent.isStopped = true; // ���� �߿� ������ ����

            isSoundPlayed = false;
            timer = 0;
            isReload = true;
        }

        //isReload = false;

        if (isReload == true)
        {
            // Reload �ִϸ��̼��� ���̸� �˰� �ִٰ� �����մϴ�. ���� �ִϸ��̼� ���̸� �𸣸� �ٸ� ����� ����ؾ� �մϴ�.
            float reloadAnimationLength = 2f; // ������ ������ �����մϴ�. ���� �ִϸ��̼��� ���̸� ����ϼ���.

            // Reload �ִϸ��̼��� ���� ���Ŀ� Attack �� Reload �޼��带 ȣ���մϴ�.
            StartCoroutine(WaitAndAttack(reloadAnimationLength));
            isReload = false;
            
        }
    }

    private IEnumerator WaitAndAttack(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        // ���� ����
        OnAttackTrue();
        Debug.Log(isReload);
        // ���� �� ������
        yield return new WaitForSeconds(3); // ���� �ִϸ��̼� ���̸� �����Ͽ� �����ϼ���.
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
            timer = 0;
        }
        SetAnimatorBools(false, false, true, false, false, false);

        // �̵��� �ٽ� �����ϵ��� ����
        if (_navMeshAgent != null)
        {
            _navMeshAgent.isStopped = false;
        }

        _isMoving = true;
    }

    private void OnMoveTrue()
    {
        SetAnimatorBools(false, false, true, false, false, false);
    }

    private void OnHitTrue()
    {
        if (isHit == true)
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
        _navMeshAgent = GetComponent<NavMeshAgent>();

        _enemyMaxHealth = EnemyStatistic.GetMaxHealthValue();
        _detectRange = EnemyStatistic.GetDetectRange();
        _meleeAttackRange = EnemyStatistic.GetAttackRange();
        _meleeAttackDamage = EnemyStatistic.GetAttackPower();
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
        deathPosition = transform.position;

        OnDieTrue();
    }
}
