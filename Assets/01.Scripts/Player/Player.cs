using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : PlayerController
{
    [Header("movement settings")]
    public float moveSpeed = 1f;
    public float rotationSpeed = 1f;
    public float dashDuration = 0.4f;
    public float dashSpeed = 20f;

    [Header("attack settings")]
    public float attackSpeed = 1f;
    public Vector3[] attackMovement;

    [Header("coolTime Settings")]
    public float dashCoolTime = 0f;
    private float dashPrevTime = 0f;

    [Header("Player HP")]
    public float playerCurrnetHP;

    [field: SerializeField] public InputReader PlayerInput { get; private set; }

    public PlayerStateMachine StateMachine { get; private set; }

    public PlayerStat PlayerStat { get; private set; }

    public bool IsAttack { get; set; }
    public bool IsDefense { get; set; }
    public bool IsDie { get; set; }

    protected override void Awake()
    {
        base.Awake();

        StateMachine = new PlayerStateMachine();
        PlayerStat = CharStat as PlayerStat;    
        playerCurrnetHP = PlayerStat.GetStatByType(StatType.maxHealth).GetValue();

        foreach (PlayerStateEnum stateEnum in Enum.GetValues(typeof(PlayerStateEnum)))
        {
            string typeName = stateEnum.ToString();
            Type t = Type.GetType($"Player{typeName}State");
            Debug.Log($"Player Get State : {t}");
            PlayerState newState = Activator.CreateInstance(t, this, StateMachine, typeName) as PlayerState;
            StateMachine.AddState(stateEnum, newState);
        }
    }

    protected void OnEnable()
    {
        PlayerInput.DashEvent += HandleDashEvent;
    }

    private void Start()
    {
        StateMachine.Initialize(PlayerStateEnum.Idle, this);
    }

    protected override void Update()
    {
        base.Update();

        StateMachine.CurrentState.UpdateState();

        if (playerCurrnetHP <= 0)
            StateMachine.ChangeState(PlayerStateEnum.Die);

        if (PlayerInput.isDefense)
        {
            var curState = StateMachine.CurrentState;

            if (curState == StateMachine.GetState(PlayerStateEnum.MeleeAttack)) return;
            if (curState == StateMachine.GetState(PlayerStateEnum.QSkill)) return;
            if (curState == StateMachine.GetState(PlayerStateEnum.ESkill)) return;
            if (curState == StateMachine.GetState(PlayerStateEnum.Dash)) return;

            if (IsGroundDetected())
                StateMachine.ChangeState(PlayerStateEnum.Defense);
        }

        // น๖วม
        //if (Keyboard.current.pKey.wasPressedThisFrame)
        //{
        //    PlayerStat.IncreaseStatBy(10, 4f, PlayerStat.GetStatByType(StatType.strength));
        //}
    }

    protected void OnDisable()
    {
        PlayerInput.DashEvent -= HandleDashEvent;
    }

    #region handling input
    private void HandleDashEvent()
    {
        if (dashCoolTime + dashPrevTime > Time.time) return;
        if (StateMachine.CurrentState._actionTriggerCalled) return;
        dashPrevTime = Time.time;
        
        StateMachine.ChangeState(PlayerStateEnum.Dash);
    }

    public void AnimationEndTrigger()
    {
        StateMachine.CurrentState.AnimationEndTrigger();
    }

    public void AnimationActionTrigger()
    {
        StateMachine.CurrentState.AnimationActionTrigger();
    }
    #endregion

    #region PlayerSetting
    public void TakeDamage(float meleeAttackDamage)
    {
        playerCurrnetHP -= meleeAttackDamage;
    }
    #endregion
}
