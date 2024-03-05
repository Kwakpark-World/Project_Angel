using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Entity
{
    [Header("movement settings")]
    public float moveSpeed = 1f;
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

    protected override void Awake()
    {
        base.Awake();

        StateMachine = new PlayerStateMachine();
        PlayerStat = CharStat as PlayerStat;
        playerCurrnetHP = PlayerStat.GetStatByType(PlayerStatType.maxHealth).GetValue();

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

        // ����
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

    public void GetFocusEnemy()
    {
        
    }
}
