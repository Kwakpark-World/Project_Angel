using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum BuffType
{
    None = 0,
    Poison,
    Freeze,
    Knockback,
    Scapegoat,
    Rune_Acceleration_DefenseCooldown,
    Rune_Acceleration_MoveSpeed,
    Rune_Acceleration_SkillCooldown,
    Rune_Acceleration_Synergy,
    Rune_Attack_AttackPower,
    Rune_Attack_AttackSpeed,
    Rune_Attack_CriticalChance,
    Rune_Attack_Synergy,
    Rune_Debuff_Freeze,
    Rune_Debuff_Knockback,
    Rune_Debuff_Poison,
    Rune_Debuff_Synergy,
    Rune_Defense_DefensivePower,
    Rune_Defense_Reflection,
    Rune_Defense_Shield,
    Rune_Defense_Synergy,
    Rune_Health_Absorb,
    Rune_Health_MaxHealth,
    Rune_Health_Recovery,
    Rune_Health_Synergy,
    Shield,
    // Fill here.
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
    private Dictionary<BuffType, object> _attackers = new Dictionary<BuffType, object>();
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
            _attackers[buffType] = null;
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

        _buffStates[buffType] = true;

        _buffTriggersByType[buffType].onBuffBegin?.Invoke();
    }

    public void PlayBuff(BuffType buffType, object attacker)
    {
        if (_isPlayer && _ownerController.StateMachine.CurrentState == _ownerController.StateMachine.GetState(PlayerStateEnum.Die))
        {
            return;
        }

        _attackers[buffType] = attacker;

        _buffTriggersByType[buffType].onBuffBegin?.Invoke();
    }

    public void PlayBuff(BuffType buffType, float duration, object attacker)
    {
        if (_isPlayer && _ownerController.StateMachine.CurrentState == _ownerController.StateMachine.GetState(PlayerStateEnum.Die))
        {
            return;
        }

        _attackers[buffType] = attacker;

        if (_coroutines[buffType] != null)
        {
            StopCoroutine(_coroutines[buffType]);
        }

        _coroutines[buffType] = StartCoroutine(BuffCoroutine(buffType, duration));
    }

    public void StopBuff(BuffType buffType)
    {
        if (_coroutines[buffType] != null)
        {
            StopCoroutine(_coroutines[buffType]);
        }

        _buffTriggersByType[buffType].onBuffEnd?.Invoke();

        _buffStates[buffType] = false;
        _coroutines[buffType] = null;
    }

    public bool GetBuffState(BuffType buffType)
    {
        return _buffStates[buffType];
    }

    private IEnumerator BuffCoroutine(BuffType buffType, float duration)
    {
        _buffStates[buffType] = true;

        _buffTriggersByType[buffType].onBuffBegin?.Invoke();

        yield return new WaitForSeconds(duration);

        _buffTriggersByType[buffType].onBuffEnd?.Invoke();

        _buffStates[buffType] = false;
        _coroutines[buffType] = null;
    }

    #region Poison Functions
    public void PoisonBegin()
    {
        if (_isPlayer)
        {
            _poisonDelayTimer = Time.time - (_attackers[BuffType.Poison] as Brain).BuffCompo.BuffStatData.poisonDelay;
            //Debug.Log(RuneManager.Instance.isArmor);
        }
        else
        {
            _poisonDelayTimer = Time.time - (_attackers[BuffType.Poison] as Player).BuffCompo.BuffStatData.poisonDelay;
        }
    }

    public void PoisonPlaying()
    {
        if (_isPlayer)
        {
            Brain attacker = _attackers[BuffType.Poison] as Brain;

            if (Time.time > _poisonDelayTimer + attacker.BuffCompo.BuffStatData.poisonDelay)
            {
                _ownerController.OnHit(attacker.BuffCompo.BuffStatData.poisonDamage);

                _poisonDelayTimer = Time.time;
            }
        }
        else
        {
            Player attacker = _attackers[BuffType.Poison] as Player;

            if (Time.time > _poisonDelayTimer + attacker.BuffCompo.BuffStatData.poisonDelay)
            {
                _ownerBrain.OnHit(attacker.BuffCompo.BuffStatData.poisonDamage);

                _poisonDelayTimer = Time.time;
            }
        }
    }
    #endregion

    #region Freeze Functions
    public void FreezeBegin()
    {
        if (_isPlayer)
        {
            Brain attacker = _attackers[BuffType.Freeze] as Brain;

            _ownerController.PlayerStatData.moveSpeed.AddModifier(attacker.BuffCompo.BuffStatData.freezeMoveSpeedModifier);
        }
        else
        {
            PlayerController attacker = _attackers[BuffType.Freeze] as PlayerController;

            _ownerBrain.EnemyStatData.moveSpeed.AddModifier(attacker.BuffCompo.BuffStatData.freezeMoveSpeedModifier);
        }

        //RuneManager.Instance.isArmor = false;
    }

    public void FreezeEnd()
    {
        if (_isPlayer)
        {
            Brain attacker = _attackers[BuffType.Freeze] as Brain;

            _ownerController.PlayerStatData.moveSpeed.RemoveModifier(attacker.BuffCompo.BuffStatData.freezeMoveSpeedModifier);
        }
        else
        {
            PlayerController attacker = _attackers[BuffType.Freeze] as PlayerController;

            _ownerBrain.EnemyStatData.moveSpeed.RemoveModifier(attacker.BuffCompo.BuffStatData.freezeMoveSpeedModifier);
        }
    }
    #endregion

    #region Knockback Functions
    public void KnockbackBegin()
    {
        if (_isPlayer)
        {
            if (_ownerController.StateMachine.CurrentState == _ownerController.StateMachine.GetState(PlayerStateEnum.NormalSlam) || _ownerController.StateMachine.CurrentState == _ownerController.StateMachine.GetState(PlayerStateEnum.AwakenSlam) || _ownerController.StateMachine.CurrentState == _ownerController.StateMachine.GetState(PlayerStateEnum.Awakening))
            {
                return;
            }

            Brain attacker = _attackers[BuffType.Knockback] as Brain;
            
            _ownerController.RigidbodyCompo.AddForce((transform.position - attacker.transform.position).normalized * attacker.BuffCompo.BuffStatData.knockbackForce, ForceMode.Impulse);
        }
        else
        {
            PlayerController attacker = _attackers[BuffType.Knockback] as PlayerController;

            _ownerBrain.RigidbodyCompo.AddForce((transform.position - attacker.transform.position).normalized * attacker.BuffCompo.BuffStatData.knockbackForce, ForceMode.Impulse);
        }
    }
    #endregion

    #region Shield Functions
    public void ShieldBegin()
    {
        EffectManager.Instance.PlayEffect(PoolingType.Effect_Shield, transform.position + transform.up * 1.5f, transform);
    }
    #endregion
}
