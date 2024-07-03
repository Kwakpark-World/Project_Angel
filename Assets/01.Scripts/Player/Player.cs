using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public enum PlayerControlEnum 
{
    Move,
    Wait,
    Stop
}


public class Player : PlayerController
{
    [Space(30f), Header("Attack Settings")]
    public GameObject weapon;

    public LayerMask enemyLayer;

    [Header("CoolTime Settings")]
    public float dashPrevTime = 0f;
    public float defensePrevTime = 0f;
    public float chargingPrevTime = 0f;
    public float slamPrevTime = 0f;
    public float dashLeftCooldown;
    public float defenseLeftCooldown;
    public float chargingLeftCooldown;
    public float slamLeftCooldown;

    public float defaultMoveSpeed = 0f;

    private float _currentAwakenGauge = 0f;
    public float CurrentShield = 10f;
    public float CurrentAwakenGauge
    {
        get
        {
            return _currentAwakenGauge;
        }

        set
        {
            _currentAwakenGauge = Mathf.Clamp(value, 0f, PlayerStatData.GetMaxAwakenGauge());

            UIManager.Instance.PlayerHUDProperty?.UpdateAwakenGauge();
        }
    }
    private float _currentChargingTime = 0f;
    public float CurrentChargingTime
    {
        get
        {
            return _currentChargingTime;
        }

        set
        {
            _currentChargingTime = Mathf.Clamp(value, 0f, PlayerStatData.GetMaxChargingTime());

            UIManager.Instance.PlayerHUDProperty?.UpdateChargingGauge();
        }
    }

    [field: Header("Asset Label")]
    [field: SerializeField] public AssetLabelReference normalStateLabel { get; private set; }
    [field: SerializeField] public AssetLabelReference awakenStateLabel { get; private set; }

    public List<Material> materials = new List<Material>();

    public const string weaponMatName = "PlayerWeaponMat";
    public const string hairMatName = "PlayerHairMat";
    public const string armorMatName = "PlayerArmorMat";

    [field: SerializeField] public InputReader PlayerInput { get; private set; }
    public PlayerStateMachine StateMachine { get; private set; }
    public PathFinder Navigation { get; private set; }
    public AnimationClip[] playerAnims;

    public Transform effectParent;

    public bool IsAttack;
    public bool IsDefense;
    public bool IsDie;
    public bool IsAwakening;
    public bool IsGroundState;
    public bool isShield;
    public PlayerControlEnum IsPlayerStop = PlayerControlEnum.Move;

    public Vector3 MousePosInWorld { get; private set; }

    public HashSet<Brain> enemyNormalHitDuplicateChecker = new HashSet<Brain>();
    public HashSet<Brain> enemyChainHitDuplicateChecker = new HashSet<Brain>();

    [Header("Debuff Render")]
    public Renderer[] renderers;

    private Volume _playerOnHitVolume;
    private Coroutine _playerOnHitVolumeCoroutine;

    protected override void Awake()
    {
        base.Awake();

        //MaterialCaching();

        BuffCompo.SetOwner(this);

        StateMachine = new PlayerStateMachine();
        Navigation = transform.Find("PathFinder").GetComponent<PathFinder>();

        foreach (PlayerStateEnum stateEnum in Enum.GetValues(typeof(PlayerStateEnum)))
        {
            string typeName = stateEnum.ToString();
            Type t = Type.GetType($"Player{typeName}State");
            PlayerState newState = Activator.CreateInstance(t, this, StateMachine, typeName) as PlayerState;
            StateMachine.AddState(stateEnum, newState);
        }

        weapon = GameObject.FindGameObjectWithTag("Weapon");
        effectParent = transform.Find("Effects");
        playerCenter = transform.Find("PlayerCenter");
        playerAnims = AnimatorCompo.runtimeAnimatorController.animationClips;

        ResetSkillCooldown();
    }

    protected void OnEnable()
    {
        PlayerInput.DashEvent += HandleDashEvent;
        PlayerInput.DefenseEvent += PlayerDefense;

    }

    protected override void Start()
    {
        base.Start();

        StateMachine.Initialize(PlayerStateEnum.Idle, this);
        PlayerStatData.InitializeAllModifiers();
        CurrentHealth = PlayerStatData.GetMaxHealth();

        defaultMoveSpeed = PlayerStatData.GetMoveSpeed();

        UIManager.Instance.PlayerHUDProperty?.UpdateHealth();
        UIManager.Instance.PlayerHUDProperty?.UpdateAwakenGauge();
        UIManager.Instance.PlayerHUDProperty?.UpdateChargingGauge();
    }

    protected override void Update()
    {
        base.Update();

        StateMachine.CurrentState.UpdateState();

        GetSkillLeftCooldown();

        //debug DeveloperKey.
        #region Debug
        if (Input.GetKeyDown(KeyCode.K))
        {
            CurrentAwakenGauge += 100;
        }

#if UNITY_EDITOR
        if (Keyboard.current.bKey.wasPressedThisFrame)
        {
            PoolManager.Instance.Pop(PoolType.GuidedBullet, GameManager.Instance.PlayerInstance.playerCenter.position);
        }
#endif
        #endregion

        SetMousePosInWorld();

    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        StateMachine.CurrentState.FixedUpdateState();
    }

    protected void OnDisable()
    {
        PlayerInput.DashEvent -= HandleDashEvent;
        PlayerInput.DefenseEvent -= PlayerDefense;
    }

    public void OnHit(float incomingDamage, Brain attacker = null)
    {
        if (BuffCompo.GetBuffState(BuffType.Rune_Defense_Uriel) && attacker && !isShield)
        {
            attacker.OnHit(incomingDamage * 0.25f);
        }

        if (IsDefense)
        {
            CameraManager.Instance.ShakeCam(0.1f, 0.3f, 1.5f);
            return;
        }

        if (IsDie)
            return;
        if (StateMachine.CompareState(PlayerStateEnum.Awakening))
            return;

        PlayerOnHitVolume();

        if (CurrentHealth > 0f)
        {
            CurrentHealth -= Mathf.Max(Mathf.RoundToInt(incomingDamage - PlayerStatData.GetDefensivePower()), 0);

            if (BuffCompo.GetBuffState(BuffType.Rune_Health_Demeter))
            {
                CurrentHealth = Mathf.Max(CurrentHealth, 1f);
            }
        }

        UIManager.Instance.PlayerHUDProperty?.UpdateHealth();

        if (CurrentHealth <= 0f)
        {
            OnDie();
        }
    }



    #region Player Stat Func
    public void SetPlayerStat(PlayerStatType stat, float value)
    {
        PlayerStatData.GetStatByType(stat).AddModifier(value);
    }

    private void GetSkillLeftCooldown()
    {
        PlayerStat stat = GameManager.Instance.PlayerInstance.PlayerStatData;
        dashLeftCooldown = Mathf.Clamp01((dashPrevTime + stat.GetDashCooldown() - Time.time) / stat.GetDashCooldown());
        defenseLeftCooldown = Mathf.Clamp01((defensePrevTime + stat.GetDefenseCooldown() - Time.time) / stat.GetDefenseCooldown());
        chargingLeftCooldown = Mathf.Clamp01((chargingPrevTime + stat.GetChargingAttackCooldown() - Time.time) / stat.GetChargingAttackCooldown());
        slamLeftCooldown = Mathf.Clamp01((slamPrevTime + stat.GetSlamCooldown() - Time.time) / stat.GetSlamCooldown());

        UIManager.Instance.PlayerHUDProperty?.UpdateSkillCooldown(dashLeftCooldown, defenseLeftCooldown, slamLeftCooldown, chargingLeftCooldown);
    }
    #endregion

    #region Player State Func
    private void OnDie()
    {
        StateMachine.ChangeState(PlayerStateEnum.Die);
        UIManager.Instance.GameOverUIProperty?.OnGameOver();
    }

    public bool IsMovePressed()
    {
        float xInput = PlayerInput.XInput;
        float yInput = PlayerInput.YInput;

        if (Mathf.Abs(xInput) > 0.05f || Mathf.Abs(yInput) > 0.05f)
        {
            return true;
        }

        return false;
    }

    private void PlayerDefense()
    {
        if (IsPlayerStop) return;

        if (IsGroundDetected())
        {
            if (PlayerStatData.GetDefenseCooldown() + defensePrevTime > Time.time)
            {
                return;
            }

            StateMachine.ChangeState(PlayerStateEnum.Defense);
        }

    }

    private void ResetSkillCooldown()
    {
        dashPrevTime = Time.time - PlayerStatData.GetDashCooldown() + 1f;
        chargingPrevTime = Time.time - PlayerStatData.GetChargingAttackCooldown() + 1f;
        slamPrevTime = Time.time - PlayerStatData.GetSlamCooldown() + 1f;
        defensePrevTime = Time.time - PlayerStatData.GetDefenseCooldown() + 1f;
    }
    #endregion

    #region handling input

    private void HandleDashEvent()
    {
        if (IsPlayerStop) return;

        if (PlayerStatData.GetDashCooldown() + dashPrevTime > Time.time) return;
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
    #endregion

    #region AnimationTrigger

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

    public void AnimationEffectEndTrigger()
    {
        StateMachine.CurrentState.AnimationEffectEndTrigger();
    }

    public void AnimationTickCheckTrigger()
    {
        StateMachine.CurrentState.AnimationTickCheckTrigger();
    }

    public void AnimationMoveFreezeToggleTrigger()
    {
        StateMachine.CurrentState.AnimationMoveFreezeToggleTrigger();
    }
    #endregion

    #region Mouse Control
    public void RotateToMousePos()
    {
        Vector3 dir = (MousePosInWorld - transform.position).normalized;
        dir.y = 0;

        transform.rotation = Quaternion.LookRotation(dir);
    }

    private void SetMousePosInWorld()
    {
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(PlayerInput.MousePos);

        RaycastHit hit;
        if (Physics.Raycast(worldPos, Camera.main.transform.forward, out hit, 3000f, whatIsGround))
        {
            MousePosInWorld = hit.point;
        }


        Debug.DrawRay(worldPos, Camera.main.transform.forward * 3000f, Color.red);
    }
    #endregion

    #region caching
    private void MaterialCaching()
    {
        string normalLabel = normalStateLabel.labelString;
        string awakenLabel = awakenStateLabel.labelString;

        materials[(int)PlayerMaterialIndex.Weapon_Normal] = MaterialManager.Instance.GetMaterial(normalLabel, weaponMatName);
        materials[(int)PlayerMaterialIndex.Hair_Normal] = MaterialManager.Instance.GetMaterial(normalLabel, hairMatName);
        materials[(int)PlayerMaterialIndex.Armor_Normal] = MaterialManager.Instance.GetMaterial(normalLabel, armorMatName);
        materials[(int)PlayerMaterialIndex.Weapon_Awaken] = MaterialManager.Instance.GetMaterial(awakenLabel, weaponMatName);
        materials[(int)PlayerMaterialIndex.Hair_Awaken] = MaterialManager.Instance.GetMaterial(awakenLabel, hairMatName);
        materials[(int)PlayerMaterialIndex.Armor_Awaken] = MaterialManager.Instance.GetMaterial(awakenLabel, armorMatName);
    }
    #endregion

    #region Impact
    private void PlayerOnHitVolume()
    {
        if (_playerOnHitVolume == null)
        {
            _playerOnHitVolume = new GameObject().AddComponent<Volume>();
            _playerOnHitVolume.name = "PlayerOnHitVolume";
        }

        if (!_playerOnHitVolume.profile.TryGet<Vignette>(out var vignette))
            _playerOnHitVolume.profile.Add<Vignette>();

        if (_playerOnHitVolumeCoroutine != null)
        {
            StopCoroutine(_playerOnHitVolumeCoroutine);
            _playerOnHitVolumeCoroutine = null;
        }

        if (_playerOnHitVolume.profile.TryGet<Vignette>(out var volume))
        {
            volume.color.overrideState = true;
            volume.intensity.overrideState = true;
            volume.rounded.overrideState = true;
        }

        // 0 ~ 1
        float valueAdd = (PlayerStatData.GetMaxHealth() - CurrentHealth) / PlayerStatData.GetMaxHealth();
        valueAdd *= 0.3f; // 0 ~ 0.3

        volume.color.value = Color.red;
        volume.intensity.value = 0.3f + valueAdd;
        volume.rounded.value = true;

        if (volume.intensity.value > 0.5f)
        {
            CameraManager.Instance.ShakeCam(0.1f, 0.3f, 1f + valueAdd);
        }

        _playerOnHitVolumeCoroutine = StartCoroutine(PlayerOnHitVolumeFade());
    }

    private IEnumerator PlayerOnHitVolumeFade()
    {
        if (_playerOnHitVolume.profile.TryGet<Vignette>(out var volume))
        {
            while (volume.intensity.value > 0f)
            {
                volume.intensity.value -= 0.01f;
                yield return null;
            }

            if (volume.intensity.value < 0f)
                volume.intensity.value = 0f;
        }
        yield return null;
    }

    #endregion
}
