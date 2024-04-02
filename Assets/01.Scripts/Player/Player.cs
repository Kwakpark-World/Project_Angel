using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.Collections;
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

    public ParticleSystem[] _weaponSlashParticles;
    public ParticleSystem _currentSlashParticle;

    public LayerMask _enemyLayer;

    public float attackPower;
    public float attackSpeed = 1f;
    public Vector3[] attackMovement;

    public float ChargingGage;

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

    public float qSkillCoolTime = 10f;
    public float qPrevTime = 0f;

    public float awakenMaxGage = 100f;
    public float awakenCurrentGage = 0f;

    [field: SerializeField] public InputReader PlayerInput { get; private set; }
    public PlayerStateMachine StateMachine { get; private set; }

    public bool IsAttack { get; set; }
    public bool IsDefense { get; set; }
    public bool IsDie { get; set; }
    public bool IsStair { get; private set; }
    public bool IsAwakening { get; set; }
    public bool IsPlayerStop { get; set; }

    public Vector3 MousePosInWorld { get; private set; }

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

        _weaponSlashParticles[0].Stop();
        _weaponSlashParticles[1].Stop();
    }

    protected override void Update()
    {
        base.Update();

        moveSpeed = PlayerStatData.GetMoveSpeed();

        StateMachine.CurrentState.UpdateState();

        PlayerDefense();

        PlayerOnStair();

        SetMousePosInWorld();

        // Debug
        if (CurrentHealth <= 0f)
        {
            OnDie();
        }

        Debug.Log(StateMachine.CurrentState);
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
        if (StateMachine.CurrentState == StateMachine.GetState(PlayerStateEnum.ESkill))
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
        qSkillCoolTime = PlayerStatData.GetQSkillCooldown();
        awakenMaxGage = PlayerStatData.GetAwakenMaxGage();
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
            if (StateMachine.CurrentState == StateMachine.GetState(PlayerStateEnum.ESkill)) return;

            StateMachine.ChangeState(PlayerStateEnum.Dash);
        }
        else
        {
            if (!IsGroundDetected()) return;
            if (StateMachine.CurrentState == StateMachine.GetState(PlayerStateEnum.ESkill)) return;

            StateMachine.ChangeState(PlayerStateEnum.EDash);
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

    public void SetPlayerModelAndAnim()
    {
        _defaultVisual.SetActive(!IsAwakening);
        _awakenVisual.SetActive(IsAwakening);


        if (!IsAwakening)
        {
            UsingAnimatorCompo = DefaultAnimatorCompo;
            _currentWeapon = _weapons[1];
            _currentSlashParticle = _weaponSlashParticles[1];
        }
        else
        {
            UsingAnimatorCompo = AwakenAnimatorCompo;
            _currentWeapon = _weapons[0];
            _currentSlashParticle = _weaponSlashParticles[0];
            
            StateMachine.ChangeState(PlayerStateEnum.Idle);
        }

        _currentSlashParticle.Stop();
    }

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
}
