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
    Rune_Attack_Heracles = 100,
    Rune_Attack_Thor,
    Rune_Attack_Michael,
    Rune_Defense_Athena = 200,
    Rune_Defense_Týr,
    Rune_Defense_Uriel,
    Rune_Acceleration_Hermes = 300,
    Rune_Acceleration_Heimdall,
    Rune_Acceleration_Gabriel,
    Rune_Health_Demeter = 400,
    Rune_Health_Freyja,
    Rune_Health_Raphael,
    Rune_Synergy_Attack = 500,
    Rune_Synergy_Defense,
    Rune_Synergy_Acceleration,
    Rune_Synergy_Health,
    Rune_Synergy_Zeus,
    Rune_Synergy_Odin,
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
    #region Hermes Functions
    public void BeginRuneHermes()
    {

    }
    #endregion

    #region Gabriel Functions
    public void BeginRuneGabriel()
    {
        _ownerController.PlayerStatData.maxAwakenGauge.AddModifier(-20f);

        _ownerController.CurrentAwakenGauge = _ownerController.CurrentAwakenGauge;

        UIManager.Instance.PlayerHUDProperty.UpdateAwakenGauge();
    }

    public void EndRuneGabriel()
    {
        _ownerController.PlayerStatData.maxAwakenGauge.RemoveModifier(-20f);
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
