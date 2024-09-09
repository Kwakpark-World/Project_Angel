using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum BuffType
{
    None = 0,
    Scapegoat,
    Shield,
    Rune_Dash_Hermes = 100,
    Rune_Dash_Hermóðr,
    Rune_Dash_Horus,
    Rune_Dash_Gabriel,
    Rune_Charge_Ares = 200,
    Rune_Charge_Týr,
    Rune_Charge_Neith,
    Rune_Charge_Michael,
    Rune_Slam_Cronus = 300,
    Rune_Slam_Thor,
    Rune_Slam_Geb,
    Rune_Slam_Uriel,
    Rune_Whirlwind_Hades = 400,
    Rune_Whirlwind_Víðarr,
    Rune_Whirlwind_Anubis,
    Rune_Whirlwind_Sariel,
    Rune_Synergy_Dash = 500,
    Rune_Synergy_Charge,
    Rune_Synergy_Slam,
    Rune_Synergy_Whirlwind,
    Rune_Synergy_Jeus,
    Rune_Synergy_Odin,
    Rune_Synergy_Ra,
    Rune_Synergy_Jesus,
    Potion_Poison = 1000,
    Potion_Freeze,
    Potion_Paralysis
}

[Serializable]
public struct BuffTrigger
{
    [Space(10)]
    public BuffType buffType;
    [Space(10)]
    public UnityEvent onBuffBegin;
    [Space(10)]
    public UnityEvent onBuffPlaying;
    [Space(10)]
    public UnityEvent onBuffEnd;
}

public class Buff : MonoBehaviour
{
    [SerializeField]
    private List<BuffTrigger> buffTriggers;
    [field: SerializeField]
    public BuffStat BuffStatData { get; private set; }

    private Dictionary<BuffType, BuffTrigger> _buffTriggersByType = new Dictionary<BuffType, BuffTrigger>();
    private Dictionary<BuffType, object> _buffers = new Dictionary<BuffType, object>();
    private Dictionary<BuffType, bool> _buffStates = new Dictionary<BuffType, bool>();
    private Dictionary<BuffType, Coroutine> _coroutines = new Dictionary<BuffType, Coroutine>();

    private Player _ownerController;
    private Brain _ownerBrain;

    private float _poisonDelayTimer = -1f;
    private bool _isPlayer;

    private void Awake()
    {
        foreach (BuffTrigger buffTrigger in buffTriggers)
        {
            _buffTriggersByType.Add(buffTrigger.buffType, buffTrigger);
        }
    }

    private void Start()
    {
        foreach (BuffType buffType in Enum.GetValues(typeof(BuffType)))
        {
            if (!_buffTriggersByType.ContainsKey(buffType))
            {
                _buffTriggersByType.Add(buffType, new BuffTrigger());
            }
        }
    }

    private void OnEnable()
    {
        foreach (BuffType buffType in Enum.GetValues(typeof(BuffType)))
        {
            _buffers[buffType] = null;
            _buffStates[buffType] = false;
            _coroutines[buffType] = null;
        }
    }

    private void Update()
    {
        foreach (BuffType buffType in Enum.GetValues(typeof(BuffType)))
        {
            if (GetBuffState(buffType))
            {
                _buffTriggersByType[buffType].onBuffPlaying?.Invoke();
            }
        }
    }

    public void SetOwner(Player owner)
    {
        _ownerController = owner;
        _isPlayer = true;
    }

    public void SetOwner(Brain owner)
    {
        _ownerBrain = owner;
        _isPlayer = false;
    }

    public void PlayBuff(BuffType buffType)
    {
        if (_isPlayer && _ownerController.StateMachine.CurrentState == _ownerController.StateMachine.GetState(PlayerStateEnum.Die))
        {
            return;
        }

        if (_buffStates[buffType])
        {
            return;
        }

        _buffStates[buffType] = true;

        UIManager.Instance.PlayerHUDProperty?.StartBuffDuration(buffType);
        _buffTriggersByType[buffType].onBuffBegin?.Invoke();
    }

    public void PlayBuff(BuffType buffType, object attacker)
    {
        if (_isPlayer && _ownerController.StateMachine.CurrentState == _ownerController.StateMachine.GetState(PlayerStateEnum.Die))
        {
            return;
        }

        _buffers[buffType] = attacker;

        UIManager.Instance.PlayerHUDProperty?.StartBuffDuration(buffType);
        _buffTriggersByType[buffType].onBuffBegin?.Invoke();
    }

    public void PlayBuff(BuffType buffType, float duration, object attacker)
    {
        if (_isPlayer && _ownerController.StateMachine.CurrentState == _ownerController.StateMachine.GetState(PlayerStateEnum.Die))
        {
            return;
        }

        if (!_buffStates[buffType])
        {
            _buffers[buffType] = attacker;
            _buffStates[buffType] = true;

            _buffTriggersByType[buffType].onBuffBegin?.Invoke();
        }

        if (_coroutines[buffType] != null)
        {
            StopCoroutine(_coroutines[buffType]);
        }

        UIManager.Instance.PlayerHUDProperty?.StartBuffDuration(buffType, duration);

        _coroutines[buffType] = StartCoroutine(BuffCoroutine(buffType, duration));
    }

    public void StopBuff(BuffType buffType)
    {
        _buffTriggersByType[buffType].onBuffEnd?.Invoke();

        _buffStates[buffType] = false;
        _coroutines[buffType] = null;

        if (_coroutines[buffType] != null)
        {
            StopCoroutine(_coroutines[buffType]);
        }
    }

    public bool GetBuffState(BuffType buffType)
    {
        return _buffStates[buffType];
    }

    private IEnumerator BuffCoroutine(BuffType buffType, float duration)
    {
        yield return new WaitForSeconds(duration);

        StopBuff(buffType);
    }

    #region Miscellaneous Buffs
    #region Shield Functions
    public void BeginShield()
    {
        EffectManager.Instance.PlayEffect(PoolType.Effect_Shield, transform.position + transform.up * 1.5f, transform);
    }
    #endregion
    #endregion

    #region Rune Buffs
    #region Dash Functions
    public void BeginDashHermes()
    {
        if (_isPlayer)
        {
            _ownerController.isRollKnockback = true;
        }
    }

    public void BeginDashHermóðr()
    {
        if (_isPlayer)
        {
            _ownerController.isRollAttack = true;
        }
    }

    public void BeginDashHorus()
    {
        if (_isPlayer)
        {
            _ownerController.isRollOnceMore = true;
        }
    }

    public void BeginDashGabriel()
    {
        if (_isPlayer)
        {
            UIManager.Instance.PlayerHUDProperty.ChangeSkillIcon(_ownerController.IsAwakened);

            _ownerController.isRollToDash = true;
        }
    }
    public void EndDashHermes()
    {
        if (_isPlayer)
        {
            _ownerController.isRollKnockback = false;
        }
    }

    public void EndDashHermóðr()
    {
        if (_isPlayer)
        {
            _ownerController.isRollAttack = false;
        }
    }

    public void EndDashHorus()
    {
        if (_isPlayer)
        {
            _ownerController.isRollOnceMore = false;
        }
    }

    public void EndDashGabriel()
    {
        if (_isPlayer)
        {
            UIManager.Instance.PlayerHUDProperty.ChangeSkillIcon(_ownerController.IsAwakened);

            _ownerController.isRollToDash = false;
        }
    }
    #endregion

    #region Charge Functions
    public void BeginChargeAres()
    {
        if (_isPlayer)
        {
            _ownerController.isChargingMultipleSting = true;
        }
    }

    public void BeginChargeTýr()
    {
        if (_isPlayer)
        {
            _ownerController.isChargingSlashOnceMore = true;
        }
    }

    public void BeginChargeNeith()
    {
        if (_isPlayer)
        {
            _ownerController.isChargingSwordAura = true;
        }
    }

    public void BeginChargeMichael()
    {
        if (_isPlayer)
        {
            _ownerController.isChargingTripleSting = true;
        }
    }
    public void EndChargeAres()
    {
        if (_isPlayer)
        {
            _ownerController.isChargingMultipleSting = false;
        }
    }

    public void EndChargeTýr()
    {
        if (_isPlayer)
        {
            _ownerController.isChargingSlashOnceMore = false;
        }
    }

    public void EndChargeNeith()
    {
        if (_isPlayer)
        {
            _ownerController.isChargingSwordAura = false;
        }
    }

    public void EndChargeMichael()
    {
        if (_isPlayer)
        {
            _ownerController.isChargingTripleSting = false;
        }
    }
    #endregion

    #region Slam Functions
    public void BeginSlamCronus()
    {
        if (_isPlayer)
        {
            _ownerController.isSlamEarthquake = true;
        }
    }

    public void BeginSlamThor()
    {
        if (_isPlayer)
        {
            _ownerController.isSlamStatic = true;
        }
    }

    public void BeginSlamGeb()
    {
        if (_isPlayer)
        {
            _ownerController.isSlamFloorEnd = true;
        }
    }

    public void BeginSlamUriel()
    {
        if (_isPlayer)
        {
            _ownerController.isSlamSixTimeSlam = true;
        }
    }
    public void EndSlamCronus()
    {
        if (_isPlayer)
        {
            _ownerController.isSlamEarthquake = false;
        }
    }

    public void EndSlamThor()
    {
        if (_isPlayer)
        {
            _ownerController.isSlamStatic = false;
        }
    }

    public void EndSlamGeb()
    {
        if (_isPlayer)
        {
            _ownerController.isSlamFloorEnd = false;
        }
    }

    public void EndSlamUriel()
    {
        if (_isPlayer)
        {
            _ownerController.isSlamSixTimeSlam = false;
        }
    }
    #endregion

    #region Whirlwind Functions
    public void BeginWhirlwindHades()
    {
        if (_isPlayer)
        {
            _ownerController.isWhirlwindPullEnemies = true;
        }
    }

    public void BeginWhirlwindVíðarr()
    {
        if (_isPlayer)
        {
            _ownerController.isWhirlwindShockWave = true;
        }
    }

    public void BeginWhirlwindAnubis()
    {
        if (_isPlayer)
        {
            _ownerController.isWhirlwindMoveAble = true;
        }
    }

    public void BeginWhirlwindSariel()
    {
        if (_isPlayer)
        {
            _ownerController.isWhirlwindRangeUp = true;
        }
    }
    public void EndWhirlwindHades()
    {
        if (_isPlayer)
        {
            _ownerController.isWhirlwindPullEnemies = false;
        }
    }

    public void EndWhirlwindVíðarr()
    {
        if (_isPlayer)
        {
            _ownerController.isWhirlwindShockWave = false;
        }
    }

    public void EndWhirlwindAnubis()
    {
        if (_isPlayer)
        {
            _ownerController.isWhirlwindMoveAble = false;
        }
    }

    public void EndWhirlwindSariel()
    {
        if (_isPlayer)
        {
            _ownerController.isWhirlwindRangeUp = false;
        }
    }
    #endregion
    #endregion

    #region Potion Buffs
    #region Poison Functions
    public void BeginPotionPoison()
    {
        _poisonDelayTimer = Time.time - (_buffers[BuffType.Potion_Poison] as Brain).BuffCompo.BuffStatData.poisonDelay;
    }

    public void PlayingPotionPoison()
    {
        Brain buffer = _buffers[BuffType.Potion_Poison] as Brain;

        if (Time.time > _poisonDelayTimer + buffer.BuffCompo.BuffStatData.poisonDelay)
        {
            _ownerController.OnHit(buffer.BuffCompo.BuffStatData.poisonDamage);

            _poisonDelayTimer = Time.time;
        }
    }
    #endregion

    #region Freeze Functions
    public void BeginPotionFreeze()
    {
        Brain buffer = _buffers[BuffType.Potion_Freeze] as Brain;

        _ownerController.PlayerStatData.moveSpeed.AddModifier(buffer.BuffCompo.BuffStatData.freezeMoveSpeedModifier);
    }

    public void EndPotionFreeze()
    {
        Brain buffer = _buffers[BuffType.Potion_Freeze] as Brain;

        _ownerController.PlayerStatData.moveSpeed.RemoveModifier(buffer.BuffCompo.BuffStatData.freezeMoveSpeedModifier);
    }
    #endregion

    #region Paralysis Functions
    public void BeginPotionParalysis()
    {
        GameManager.Instance.PlayerInstance.StopImmediately(true);

        GameManager.Instance.PlayerInstance.IsPlayerStop = PlayerControlEnum.Wait;
        CameraManager.Instance._currentCam.IsCamRotateStop = true;
    }

    public void EndPotionParalysis()
    {
        GameManager.Instance.PlayerInstance.IsPlayerStop = PlayerControlEnum.Move;
        CameraManager.Instance._currentCam.IsCamRotateStop = false;
    }
    #endregion
    #endregion
}
