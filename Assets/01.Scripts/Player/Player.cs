using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Player : PlayerController
{
    public EndingUI endingUI;

    [Space(30f), Header("Attack Settings")]
    public GameObject weapon;

    public LayerMask enemyLayer;

    [Header("Defense Settings")]
    public float defenseTime = 3f;

    [Header("CoolTime Settings")]
    private float _dashPrevTime = 0f;
    public float slamPrevTime = 0f;
    public float defensePrevTime = 0f;

    public float currentAwakenGauge = 0f;
    public float currentChargingTime = 0f;

    [field: Header("Asset Label")]
    [field: SerializeField] public AssetLabelReference normalStateLabel { get; private set; }
    [field: SerializeField] public AssetLabelReference awakenStateLabel { get; private set; }

    public Material[] materials { get; private set; } = new Material[6];

    public const string weaponMatName = "PlayerWeaponMat";
    public const string hairMatName = "PlayerHairMat";
    public const string armorMatName = "PlayerArmorMat";

    [field: SerializeField] public InputReader PlayerInput { get; private set; }
    public PlayerStateMachine StateMachine { get; private set; }
    public AnimationClip[] playerAnims;

    public Transform effectParent;

    public bool IsAttack;
    public bool IsDefense;
    public bool IsDie;
    public bool IsAwakening;
    public bool IsPlayerStop;
    public bool IsGroundState;

    public Vector3 MousePosInWorld { get; private set; }

    public HashSet<Brain> enemyNormalHitDuplicateChecker = new HashSet<Brain>();
    public HashSet<Brain> enemyChainHitDuplicateChecker = new HashSet<Brain>();

    [Header("Debuff Render")]
    public Renderer[] renderers;
    public Material freezeMaterial;
    private bool _isFreezing;

    private Volume _playerOnHitVolume;
    private Coroutine _playerOnHitVolumeCoroutine;

    protected override void Awake()
    {
        base.Awake();

        MaterialManager.Instance._cachingAction += MaterialCaching;

        BuffCompo.SetOwner(this);

        StateMachine = new PlayerStateMachine();

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
    }

    protected override void Update()
    {
        base.Update();

        StateMachine.CurrentState.UpdateState();


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
        //����ٰ�?�÷��̾� �ݻ� �� �ϱⷯ��

        if (BuffCompo.GetBuffState(BuffType.Rune_Defense_Uriel) && attacker)
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
            CurrentHealth -= Mathf.Max(incomingDamage - PlayerStatData.GetDefensivePower(), 0f);

            if (BuffCompo.GetBuffState(BuffType.Rune_Health_Demeter))
            {
                CurrentHealth = Mathf.Max(CurrentHealth, 1f);
            }
        }

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
    #endregion

    #region Player State Func
    private void OnDie()
    {
        StateMachine.ChangeState(PlayerStateEnum.Die);
        endingUI.OnDie();
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
        if (IsGroundDetected())
        {
            if (PlayerStatData.GetDefenseCooldown() + defensePrevTime > Time.time) return;
            StateMachine.ChangeState(PlayerStateEnum.Defense);
        }

    }
    #endregion

    #region handling input

    private void HandleDashEvent()
    {
        if (PlayerStatData.GetDashCooldown() + _dashPrevTime > Time.time) return;
        if (StateMachine.CurrentState._actionTriggerCalled) return;

        _dashPrevTime = Time.time;

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
