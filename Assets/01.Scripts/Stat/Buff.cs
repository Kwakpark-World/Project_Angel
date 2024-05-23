using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum BuffType
{
    None = 0,
    Scapegoat = 1,
    Shield = 2,
    Rune_Acceleration_DefenseCooldown = 100,
    Rune_Acceleration_MoveSpeed = 101,
    Rune_Acceleration_SkillCooldown = 102,
    Rune_Acceleration_Synergy = 150,
    Rune_Attack_AttackPower = 200,
    Rune_Attack_AttackSpeed = 201,
    Rune_Attack_CriticalChance = 202,
    Rune_Attack_Synergy = 250,
    Rune_Debuff_Freeze = 300,
    Rune_Debuff_Paralysis = 301,
    Rune_Debuff_Poison = 302,
    Rune_Debuff_Synergy = 350,
    Rune_Defense_DefensivePower = 400,
    Rune_Defense_Reflection = 401,
    Rune_Defense_Shield = 402,
    Rune_Defense_Synergy = 450,
    Rune_Health_Absorb = 500,
    Rune_Health_MaxHealth = 501,
    Rune_Health_Recovery = 502,
    Rune_Health_Synergy = 550,
    Potion_Poison = 1001,
    Potion_Freeze = 1002,
    Potion_Paralysis = 1003,
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

        if (_buffStates[buffType])
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

        if (!_buffStates[buffType])
        {
            _attackers[buffType] = attacker;
            _buffStates[buffType] = true;

            _buffTriggersByType[buffType].onBuffBegin?.Invoke();
        }

        if (_coroutines[buffType] != null)
        {
            StopCoroutine(_coroutines[buffType]);
        }

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

    #region Poison Functions
    public void PoisonBegin()
    {
        if (_isPlayer)
        {
            _poisonDelayTimer = Time.time - (_attackers[BuffType.Potion_Poison] as Brain).BuffCompo.BuffStatData.poisonDelay;
            //Debug.Log(RuneManager.Instance.isArmor);
        }
        else
        {
            _poisonDelayTimer = Time.time - (_attackers[BuffType.Potion_Poison] as Player).BuffCompo.BuffStatData.poisonDelay;
        }
    }

    public void PoisonPlaying()
    {
        if (_isPlayer)
        {
            Brain attacker = _attackers[BuffType.Potion_Poison] as Brain;

            if (Time.time > _poisonDelayTimer + attacker.BuffCompo.BuffStatData.poisonDelay)
            {
                _ownerController.OnHit(attacker.BuffCompo.BuffStatData.poisonDamage);

                _poisonDelayTimer = Time.time;
            }
        }
        else
        {
            Player attacker = _attackers[BuffType.Potion_Poison] as Player;

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
            Brain attacker = _attackers[BuffType.Potion_Freeze] as Brain;

            _ownerController.PlayerStatData.moveSpeed.AddModifier(attacker.BuffCompo.BuffStatData.freezeMoveSpeedModifier);
        }
        else
        {
            PlayerController attacker = _attackers[BuffType.Potion_Freeze] as PlayerController;

            _ownerBrain.EnemyStatData.moveSpeed.AddModifier(attacker.BuffCompo.BuffStatData.freezeMoveSpeedModifier);
        }

        //RuneManager.Instance.isArmor = false;
    }

    public void FreezeEnd()
    {
        if (_isPlayer)
        {
            Brain attacker = _attackers[BuffType.Potion_Freeze] as Brain;

            _ownerController.PlayerStatData.moveSpeed.RemoveModifier(attacker.BuffCompo.BuffStatData.freezeMoveSpeedModifier);
        }
        else
        {
            PlayerController attacker = _attackers[BuffType.Potion_Freeze] as PlayerController;

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

            Brain attacker = _attackers[BuffType.Potion_Paralysis] as Brain;
            
            _ownerController.RigidbodyCompo.AddForce((transform.position - attacker.transform.position).normalized * attacker.BuffCompo.BuffStatData.paralysisDuration, ForceMode.Impulse);
        }
        else
        {
            PlayerController attacker = _attackers[BuffType.Potion_Paralysis] as PlayerController;

            _ownerBrain.RigidbodyCompo.AddForce((transform.position - attacker.transform.position).normalized * attacker.BuffCompo.BuffStatData.paralysisDuration, ForceMode.Impulse);
        }
    }
    #endregion

    #region Shield Functions
    public void ShieldBegin()
    {
        EffectManager.Instance.PlayEffect(PoolType.Effect_Shield, transform.position + transform.up * 1.5f, transform);
    }
    #endregion
}
