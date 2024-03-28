using System;
using System.Collections.Generic;
using UnityEngine;

public class Player : PlayerController
{
    [Header("Movement Settings")]
    public float moveSpeed = 1f;
    public float rotationSpeed = 1f;
    public float dashDuration = 0.4f;
    public float dashSpeed = 20f;

    [Header("Attack Settings")]
    private GameObject[] _weapons;
    public GameObject _currentWeapon;

    public LayerMask _enemyLayer;

    public float attackPower;
    public float attackSpeed = 1f;
    public Vector3[] attackMovement;

    [Header("Critical Settings")]
    public float criticalChance;
    public float criticalMultiplier;

    [Header("Defense Settings")]
    public float defensivePower;
    public float defenseTime = 3f;

    [Header("CoolTime Settings")]
    public float dashCoolTime = 0f;
    private float dashPrevTime = 0f;

    public float defenseCoolTime = 1f;
    public float defensePrevTime = 0f;

    [field: SerializeField] public InputReader PlayerInput { get; private set; }
    public PlayerStateMachine StateMachine { get; private set; }

    public bool IsAttack { get; set; }
    public bool IsDefense { get; set; }
    public bool IsDie { get; set; }
    public bool IsStair { get; private set; }
    public bool IsAwakening { get; set; }
    public bool IsPlayerStop { get; set; }

    public Vector3 MousePosInWorld { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        DebuffCompo.SetOwner(this);

        StateMachine = new PlayerStateMachine();

        foreach (PlayerStateEnum stateEnum in Enum.GetValues(typeof(PlayerStateEnum)))
        {
            string typeName = stateEnum.ToString();
            Type t = Type.GetType($"Player{typeName}State");
            PlayerState newState = Activator.CreateInstance(t, this, StateMachine, typeName) as PlayerState;
            StateMachine.AddState(stateEnum, newState);
        }

        _weapons = GameObject.FindGameObjectsWithTag("Weapon");
    }

    protected void OnEnable()
    {
        PlayerInput.DashEvent += HandleDashEvent;
    }

    protected override void Start()
    {
        base.Start();
        SetPlayerModelAndAnim();

        StateMachine.Initialize(PlayerStateEnum.Idle, this);
        PlayerStatData.InitializeAllModifiers();
        PlayerStatInitialize();


        
    }


    protected override void Update()
    {
        base.Update();

        moveSpeed = PlayerStatData.GetMoveSpeed();

        StateMachine.CurrentState.UpdateState();

        PlayerDefense();

        PlayerOnStair();

        SetMousePosInWorld();

        // ¹Ù´Ú¿¡ ´¯´Â°Å ¹æÁöÀÓ;; ´¯´Â ÀÌÀ¯ ¾Ë¸é µð¿¥Á»..
        if (transform.rotation.x > 0.707 && transform.rotation.x < 0.709)
        {
            transform.rotation = Quaternion.LookRotation(Vector3.zero);
        }

        // ï¿½ï¿½ï¿½ï¿½
        //if (Keyboard.current.pKey.wasPressedThisFrame)
        //{
        //    PlayerStat.IncreaseStatBy(10, 4f, PlayerStat.GetStatByType(StatType.strength));
        //}

    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        IsClimbStair();
    }

    protected void OnDisable()
    {
        PlayerInput.DashEvent -= HandleDashEvent;
    }

    private void IsClimbStair()
    {
        if (CheckStair(Vector3.forward))
            IsStair = true;
        if (CheckStair(new Vector3(1.5f, 0, 1)))
            IsStair = true;
        if (CheckStair(new Vector3(-1.5f, 0, 1)))
            IsStair = true;
    }

    public void OnHit(float incomingDamage)
    {
        if (IsDefense || IsDie)
            return;

        CurrentHealth -= Mathf.Max(incomingDamage - defensivePower, 0f);

        if (CurrentHealth <= 0f)
        {
            OnDie();
        }
    }

    #region Player Stat Func
    private void PlayerStatInitialize()
    {
        CurrentHealth = PlayerStatData.GetMaxHealth();
        defensivePower = PlayerStatData.GetDefensivePower();
        defenseCoolTime = PlayerStatData.GetDefenseCooldown();
        attackPower = PlayerStatData.GetAttackPower();
        attackSpeed = PlayerStatData.GetAttackSpeed();
        criticalChance = PlayerStatData.GetCriticalChance();
        criticalMultiplier = PlayerStatData.GetCriticalMultiplier();
        moveSpeed = PlayerStatData.GetMoveSpeed();
        rotationSpeed = PlayerStatData.GetRotateSpeed();
        dashSpeed = PlayerStatData.GetDashSpeed();
        dashDuration = PlayerStatData.GetDashDuration();
        dashCoolTime = PlayerStatData.GetDashCooldown();
    }

    public void SetPlayerStat(PlayerStatType stat, float value)
    {
        PlayerStatData.GetStatByType(stat).AddModifier(value);
    }
    #endregion

    #region Player State Func
    private void OnDie()
    {
        StateMachine.ChangeState(PlayerStateEnum.Die);
    }

    private void PlayerOnStair()
    {
        if (IsStair)
            if (IsGroundDetected())
                IsStair = false;
    }
#endregion

    #region handling input
    private void PlayerDefense()
    {
        if (PlayerInput.isDefense)
        {
            var curState = StateMachine.CurrentState;

            if (curState == StateMachine.GetState(PlayerStateEnum.MeleeAttack)) return;
            if (curState == StateMachine.GetState(PlayerStateEnum.QSkill)) return;
            if (curState == StateMachine.GetState(PlayerStateEnum.ESkill)) return;
            if (curState == StateMachine.GetState(PlayerStateEnum.Dash)) return;
            if (curState == StateMachine.GetState(PlayerStateEnum.EDash)) return;
            if (curState == StateMachine.GetState(PlayerStateEnum.Charge)) return;

            if (IsGroundDetected())
            {
                if (defenseCoolTime + defensePrevTime > Time.time) return;
                StateMachine.ChangeState(PlayerStateEnum.Defense);
            }
        }
    }

    private void HandleDashEvent()
    {
        if (dashCoolTime + dashPrevTime > Time.time) return;
        if (StateMachine.CurrentState._actionTriggerCalled) return;

        dashPrevTime = Time.time;

        if (!IsAwakening)
        {
            if (!IsGroundDetected()) return;
            StateMachine.ChangeState(PlayerStateEnum.Dash);
        }
        else
            StateMachine.ChangeState(PlayerStateEnum.EDash);
    }

    public void AnimationEndTrigger()
    {
        StateMachine.CurrentState.AnimationEndTrigger();
    }

    public void AnimationActionTrigger()
    {
        StateMachine.CurrentState.AnimationActionTrigger();
    }

    public void AnimationHitAbleTrigger()
    {
        StateMachine.CurrentState.AnimationHitAbleTrigger();
    }
    #endregion


    public void SetPlayerModelAndAnim()
    {
        _defaultVisual.SetActive(!IsAwakening);
        _awakenVisual.SetActive(IsAwakening);

        //StateMachine.ChangeState(PlayerStateEnum.Idle);

        if (!IsAwakening)
        {
            UsingAnimatorCompo = DefaultAnimatorCompo;
            _currentWeapon = _weapons[1];
        }
        else
        {
            UsingAnimatorCompo = AwakenAnimatorCompo;
            _currentWeapon = _weapons[0];
        }
    }

    public void RotateToMousePos()
    {
        Vector3 dir = (MousePosInWorld - transform.position).normalized;

        transform.transform.rotation = Quaternion.LookRotation(dir);
    }

    private void SetMousePosInWorld()
    {
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(PlayerInput.MousePos);

        RaycastHit hit;
        if (Physics.Raycast(worldPos, Camera.main.transform.forward, out hit, 3000f, _whatIsGround))
        {
            MousePosInWorld = hit.point;
        }

        Debug.DrawRay(worldPos, Camera.main.transform.forward * 3000f, Color.red);
    }

}
