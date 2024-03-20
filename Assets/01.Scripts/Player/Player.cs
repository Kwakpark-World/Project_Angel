using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class Player : PlayerController
{
    [Header("Movement Settings")]
    public float moveSpeed = 1f;
    public float rotationSpeed = 1f;
    public float dashDuration = 0.4f;
    public float dashSpeed = 20f;

    [Header("Attack Settings")]
    public float attackSpeed = 1f;
    public Vector3[] attackMovement;

    [Header("Defense Settings")]
    public float defenseTime = 3f;

    [Header("CoolTime Settings")]
    public float dashCoolTime = 0f;
    private float dashPrevTime = 0f;

    public float defenseCoolTime = 1f;
    public float defensePrevTime = 0f;

    [field: SerializeField] public InputReader PlayerInput { get; private set; }
    public PlayerStateMachine StateMachine { get; private set; }
    public PlayerStat PlayerStat { get; private set; }

    public Dictionary<DebuffType, MethodInfo> methodInfos = new Dictionary<DebuffType, MethodInfo>();

    public bool IsAttack { get; set; }
    public bool IsDefense { get; set; }
    public bool IsDie { get; set; }
    public bool IsStair { get; private set; }
    public bool IsAwakening { get; set; }
    public bool IsPlayerStop { get; set; }

    private ParticleSystem _poisonParticle;

    protected override void Awake()
    {
        base.Awake();

        StateMachine = new PlayerStateMachine();

        PlayerStat = CharStat as PlayerStat;

        foreach (PlayerStateEnum stateEnum in Enum.GetValues(typeof(PlayerStateEnum)))
        {
            string typeName = stateEnum.ToString();
            Type t = Type.GetType($"Player{typeName}State");
            PlayerState newState = Activator.CreateInstance(t, this, StateMachine, typeName) as PlayerState;
            StateMachine.AddState(stateEnum, newState);
        }

        foreach (DebuffType type in Enum.GetValues(typeof(DebuffType)))
        {
            methodInfos.Add(type, PlayerStat.GetType().GetMethod(type.ToString()));
        }

        _poisonParticle = transform.Find("PoisonParticle").GetComponent<ParticleSystem>();
    }

    protected void OnEnable()
    {
        PlayerInput.DashEvent += HandleDashEvent;
    }

    protected override void Start()
    {
        base.Start();

        StateMachine.Initialize(PlayerStateEnum.Idle, this);
        PlayerStat.InitializeAllModifiers();
    }


    protected override void Update()
    {
        base.Update();

        moveSpeed = PlayerStat.GetMoveSpeed();

        StateMachine.CurrentState.UpdateState();

        PlayerDie();

        PlayerDefense();

        PlayerOnStair();

        PlayerDebuff();
        // ����
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

    private void PlayerDie()
    {
        if (PlayerStat.GetCurrentHealth() <= 0)
            StateMachine.ChangeState(PlayerStateEnum.Die);
    }

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

    private void PlayerOnStair()
    {
        if (IsStair)
            if (IsGroundDetected())
                IsStair = false;
    }

    private void PlayerDebuff()
    {
        foreach (DebuffType type in Enum.GetValues(typeof(DebuffType)))
        {
            if (PlayerStat.GetDebuff(type))
            {
                if (type == DebuffType.Poison)
                {
                    _poisonParticle.Play();
                }

                methodInfos[type].Invoke(PlayerStat, null);
            }
            else
            {
                if (type == DebuffType.Poison)
                {
                    _poisonParticle.Stop();
                }
            }
        }
    }


    #region handling input
    private void HandleDashEvent()
    {
        if (dashCoolTime + dashPrevTime > Time.time) return;
        if (StateMachine.CurrentState._actionTriggerCalled) return;
        dashPrevTime = Time.time;

        if (!IsAwakening)
            StateMachine.ChangeState(PlayerStateEnum.Dash);
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
    #endregion

    public void SetPlayerStat(PlayerStatType stat, float value)
    {
        PlayerStat.GetStatByType(stat).AddModifier(value);
    }
}
