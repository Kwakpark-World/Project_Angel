using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : PlayerController
{
    [Space(30f), Header("Attack Settings")]
    public GameObject _weapon;

    public LayerMask _enemyLayer;

    public float ChargingGauge;

    [Header("Defense Settings")]
    public float defenseTime = 3f;

    [Header("CoolTime Settings")]
    private float dashPrevTime = 0f;

    public float defensePrevTime = 0f;

    public float awakenCurrentGauge = 0f;

    [field: SerializeField] public InputReader PlayerInput { get; private set; }
    public PlayerStateMachine StateMachine { get; private set; }

    public bool IsAttack { get; set; }
    public bool IsDefense { get; set; }
    public bool IsDie { get; set; }
    public bool IsStair { get; private set; }
    public bool IsAwakening { get; set; }
    public bool IsPlayerStop { get; set; }
    public bool IsGroundState { get; set; }

    public Vector3 MousePosInWorld { get; private set; }

    public HashSet<Brain> enemyNormalHitDuplicateChecker = new HashSet<Brain>();
    public HashSet<Brain> enemyChainHitDuplicateChecker = new HashSet<Brain>();

    [Header("Debuff Render")]
    public Renderer[] renderers;
    public Material freezeMaterial;
    private bool _isFreezing;

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

        _weapon = GameObject.FindGameObjectWithTag("Weapon");
    }

    protected void OnEnable()
    {
        PlayerInput.DashEvent += HandleDashEvent;
    }

    protected override void Start()
    {
        base.Start();

        StateMachine.Initialize(PlayerStateEnum.Idle, this);
        PlayerStatData.InitializeAllModifiers();
        PlayerStatInitialize();
    }
    
    protected override void Update()
    {
        base.Update();

        StateMachine.CurrentState.UpdateState();

        PlayerDefense();

        PlayerOnStair();

        SetMousePosInWorld();
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
        if (StateMachine.CurrentState == StateMachine.GetState(PlayerStateEnum.Awakening))
            return;
       
        if(RuneManager.Instance.isLastDance == true && CurrentHealth <= 1f)
        {
            CurrentHealth -= Mathf.Max(0, 0f);
        }
        else
        {
            CurrentHealth -= Mathf.Max(incomingDamage - PlayerStatData.GetDefensivePower(), 0f);
        }
        

        if (CurrentHealth <= 0f)
        {
            OnDie();
        }
    }

    #region Player Stat Func
    private void PlayerStatInitialize()
    {
        CurrentHealth = PlayerStatData.GetMaxHealth();

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
            if (curState == StateMachine.GetState(PlayerStateEnum.NormalSlam)) return;
            if (curState == StateMachine.GetState(PlayerStateEnum.Awakening)) return;
            if (curState == StateMachine.GetState(PlayerStateEnum.NormalDash)) return;
            if (curState == StateMachine.GetState(PlayerStateEnum.AwakenDash)) return;
            if (curState == StateMachine.GetState(PlayerStateEnum.Charging)) return;

            if (IsGroundDetected())
            {
                if (PlayerStatData.GetDefenseCooldown() + defensePrevTime > Time.time) return;
                StateMachine.ChangeState(PlayerStateEnum.Defense);
            }
        }
    }

    private void HandleDashEvent()
    {
        if (PlayerStatData.GetDefenseCooldown() + dashPrevTime > Time.time) return;
        if (StateMachine.CurrentState._actionTriggerCalled) return;

        dashPrevTime = Time.time;

        if (!IsAwakening)
        {
            if (!IsGroundDetected()) return;
            if (StateMachine.CurrentState == StateMachine.GetState(PlayerStateEnum.Awakening)) return;

            StateMachine.ChangeState(PlayerStateEnum.NormalDash);
        }
        else
        {
            if (!IsGroundDetected()) return;
            if (StateMachine.CurrentState == StateMachine.GetState(PlayerStateEnum.Awakening)) return;

            StateMachine.ChangeState(PlayerStateEnum.AwakenDash);
        }
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

    public void AnimationEffectTrigger()
    {
        StateMachine.CurrentState.AnimationEffectTrigger();
    }
    #endregion

    #region Mouse Control
    public void RotateToMousePos()
    {
        Vector3 dir = (MousePosInWorld - transform.position).normalized;
        dir.y = 0;

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
    #endregion

    #region Debuff Control
    public void AddFreezeMaterial()
    {
        if (_isFreezing)
        {
            return;
        }

        _isFreezing = true;

        foreach (Renderer renderer in renderers)
        {
            List<Material> rendererMaterials = new List<Material>();

            renderer.GetMaterials(rendererMaterials);
            rendererMaterials.Add(freezeMaterial); 
            renderer.SetMaterials(rendererMaterials);
        }
    }

    public void RemoveFreezeMaterial()
    {
        if (!_isFreezing)
        {
            return;
        }

        _isFreezing = false;

        foreach (Renderer renderer in renderers)
        {
            List<Material> rendererMaterials = new List<Material>();

            renderer.GetMaterials(rendererMaterials);
            rendererMaterials.Remove(rendererMaterials.Last());
            renderer.SetMaterials(rendererMaterials);
        }
    }
    #endregion
}
