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

    [Header("Cooldown Settings")]
    [HideInInspector]public float dashPrevTime = 0f;
    [HideInInspector]public float chargePrevTime = 0f;
    [HideInInspector]public float slamPrevTime = 0f;
    [HideInInspector]public float whirlwindPrevTime = 0f;

    [HideInInspector]public float awakenTime = 0f;
    
    [HideInInspector]public float dashLeftCooldown;
    [HideInInspector]public float chargeLeftCooldown;
    [HideInInspector]public float slamLeftCooldown;
    [HideInInspector] public float whirlwindLeftCooldown;

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
    private float _currentChargeTime = 0f;
    public float CurrentChargeTime
    {
        get
        {
            return _currentChargeTime;
        }

        set
        {
            _currentChargeTime = Mathf.Clamp(value, 0f, PlayerStatData.GetMaxChargeTime());

            UIManager.Instance.PlayerHUDProperty?.UpdateChargeGauge();
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
    public bool IsAwakened;
    public bool IsGroundState;
    public bool isShield;
    public PlayerControlEnum IsPlayerStop = PlayerControlEnum.Move;

    public Vector3 MousePosInWorld { get; private set; }

    public HashSet<Brain> enemyNormalHitDuplicateChecker = new HashSet<Brain>();
    public HashSet<Brain> enemyChainHitDuplicateChecker = new HashSet<Brain>();

    public HashSet<Brain> enemyDashHitDuplicateChecker = new HashSet<Brain>();
    public HashSet<Brain> enemyKnockBackDuplicateChecker = new HashSet<Brain>();


    [Header("Debuff Render")]
    public Renderer[] renderers;

    private Volume _playerOnHitVolume;
    private Coroutine _playerOnHitVolumeCoroutine;

    [Header("Rune Params"), Header("Charging")]
    public bool isChargingTripleSting;
    public bool isChargingMultipleSting;
    public bool isChargingSlashOnceMore;
    public bool isChargingSwordAura;
    [Header("Slam")]
    public bool isSlamSixTimeSlam;
    [Header("Whirlwind")]
    public bool isWhirlwindShockWave;
    public bool isWhirlwindMoveAble;
    public bool isWhirlwindPullEnemies;
    public bool isWhirlwindRangeUp;
    [Header("Dash")]
    public bool isRollToDash;
    public bool isRollAttack;
    public bool isRollKnockback;
    public bool isRollOnceMore;

    [HideInInspector] public bool isOnRollOnceMore;
    [HideInInspector] public bool isOnChargingSlashOnceMore;

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
        UIManager.Instance.PlayerHUDProperty?.UpdateChargeGauge();
    }

    protected override void Update()
    {
        base.Update();

        StateMachine.CurrentState.UpdateState();

        GetSkillLeftCooldown();

        // Debug(Developer Key)
        #region Debug
        if (Input.GetKeyDown(KeyCode.K))
        {
            CurrentAwakenGauge += 100;
        }
        #endregion

        SetMousePosInWorld();
        //Skill_Synergy(enemy);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        StateMachine.CurrentState.FixedUpdateState();
    }

    protected void OnDisable()
    {
        PlayerInput.DashEvent -= HandleDashEvent;
    }

    public void OnHit(float incomingDamage, Brain attacker = null)
    {
        CameraManager.Instance.ShakeCam(0.1f, 0.3f, 1f);
        //EarthQuake(attacker);
        if (attacker && !isShield)
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
        chargeLeftCooldown = Mathf.Clamp01((chargePrevTime + stat.GetChargeAttackCooldown() - Time.time) / stat.GetChargeAttackCooldown());
        slamLeftCooldown = Mathf.Clamp01((slamPrevTime + stat.GetSlamCooldown() - Time.time) / stat.GetSlamCooldown());
        whirlwindLeftCooldown = Mathf.Clamp01((whirlwindPrevTime + stat.GetWhirlwindCooldown() - Time.time) / stat.GetWhirlwindCooldown());

        UIManager.Instance.PlayerHUDProperty?.UpdateSkillCooldown(dashLeftCooldown, chargeLeftCooldown, slamLeftCooldown, whirlwindLeftCooldown);
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

    private void ResetSkillCooldown()
    {
        dashPrevTime = Time.time - PlayerStatData.GetDashCooldown() + 1f;
        chargePrevTime = Time.time - PlayerStatData.GetChargeAttackCooldown() + 1f;
        slamPrevTime = Time.time - PlayerStatData.GetSlamCooldown() + 1f;
        whirlwindPrevTime = Time.time - PlayerStatData.GetWhirlwindCooldown() + 1f;
    }
    #endregion

    #region handling input

    private void HandleDashEvent()
    {
        if (IsPlayerStop == PlayerControlEnum.Stop) return;

        if (PlayerStatData.GetDashCooldown() + dashPrevTime > Time.time) return;

        isOnRollOnceMore = false;
        dashPrevTime = Time.time;
        awakenTime = 0;

        if (!isRollToDash)

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

    public void AnimationPlayerSoundTrigger()
    {
        StateMachine.CurrentState.AnimationPlayerSoundTrigger();
    }

    public void AnimationPlayerAttackImpactTrigger()
    {
        StateMachine.CurrentState.AnimationPlayerAttackImpactTrigger();
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

    public void Skill_Synergy()
    {
        PlayerStat stat = GameManager.Instance.PlayerInstance.PlayerStatData;

        //ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½
        if (Keyboard.current.qKey.wasPressedThisFrame)
        {
            // ?„ë????´ê±° ?„ìž¬ ì¿¨í???4ì´?ê°ì†Œ ?„ë‹ˆê³?ìµœë? ì¿¨í???4ì´?ê°ì†Œ?? ?½ê°„ ê³ ì¹˜ê¸??ˆëŠ”???Œì•„???˜ì •(Modifier ???“ì´ê²?
            stat.slamCooldown.AddModifier(-4f);
            Debug.Log(stat.GetSlamCooldown());
        }

        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            CurrentHealth += 1;
        }

        if (Keyboard.current.shiftKey.wasPressedThisFrame)
        {
            StartCoroutine(IAS(3f));
        }
    }

    public void EarthQuake(Brain enemy)
    {
        //?´ìŠ¤?˜ì´??
        enemy.BuffCompo.PlayBuff(BuffType.Potion_Paralysis);
        if (Keyboard.current.pKey.wasPressedThisFrame)
        {
            
            
        }
    }

    private IEnumerator IAS(float duration)
    {
        if (duration >= 3f)
        {
            yield return new WaitForSeconds(3f);
        }
        else
        {
            yield return new WaitForSeconds(duration);
        }

        BuffCompo.StopBuff(BuffType.Potion_Freeze);
        BuffCompo.StopBuff(BuffType.Potion_Paralysis);
        BuffCompo.StopBuff(BuffType.Potion_Poison);
    }
}
