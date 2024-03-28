using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public enum DebuffType
{
    Poison,
    Freeze,
    Knockback,
    Scapegoat,
    // Fill here.
}

public class Debuff : MonoBehaviour
{
    [field: SerializeField]
    public DebuffStat DebuffStatData { get; private set; }

    private Dictionary<DebuffType, MethodInfo> _methodInfos = new Dictionary<DebuffType, MethodInfo>();
    private Dictionary<DebuffType, object> _attackers = new Dictionary<DebuffType, object>();
    private Dictionary<DebuffType, bool> _debuffStates = new Dictionary<DebuffType, bool>();
    private Dictionary<DebuffType, Coroutine> _coroutines = new Dictionary<DebuffType, Coroutine>();

    private Player _ownerController;
    private Brain _ownerBrain;

    private float _poisonDelayTimer = -1f;
    private float _freezeDurationTimer = -1f;

    private void Awake()
    {
        foreach (DebuffType debuffType in Enum.GetValues(typeof(DebuffType)))
        {
            _methodInfos.Add(debuffType, GetType().GetMethod(debuffType.ToString()));
            _attackers.Add(debuffType, null);
            _debuffStates.Add(debuffType, false);
            _coroutines.Add(debuffType, null);
        }
    }

    private void OnEnable()
    {
        foreach (DebuffType debuffType in Enum.GetValues(typeof(DebuffType)))
        {
            _attackers[debuffType] = null;
            _debuffStates[debuffType] = false;
            _coroutines[debuffType] = null;
        }
    }

    private void Update()
    {
        foreach (DebuffType debuffType in Enum.GetValues(typeof(DebuffType)))
        {
            if (GetDebuff(debuffType))
            {
                _methodInfos[debuffType].Invoke(this, null);
            }
        }
    }

    public void SetOwner(Player owner)
    {
        _ownerController = owner;
    }

    public void SetOwner(Brain owner)
    {
        _ownerBrain = owner;
    }

    public void SetDebuff(DebuffType debuffType, object attacker)
    {
        _attackers[debuffType] = attacker;

        _methodInfos[debuffType].Invoke(this, null);
    }

    public void SetDebuff(DebuffType debuffType, float duration, object attacker)
    {
        _attackers[debuffType] = attacker;

        if (_coroutines[debuffType] != null)
        {
            StopCoroutine(_coroutines[debuffType]);
        }

        _coroutines[debuffType] = StartCoroutine(DebuffCoroutine(debuffType, duration));
    }

    public bool GetDebuff(DebuffType debuffType)
    {
        return _debuffStates[debuffType];
    }

    private IEnumerator DebuffCoroutine(DebuffType debuffType, float duration)
    {
        _debuffStates[debuffType] = true;

        yield return new WaitForSeconds(duration);

        _debuffStates[debuffType] = false;
        _coroutines[debuffType] = null;
    }

    public void Poison()
    {
        if (_ownerController)
        {
            Brain attacker = _attackers[DebuffType.Poison] as Brain;

            _poisonDelayTimer = Time.time - attacker.DebuffCompo.DebuffStatData.poisonDelay;

            if (Time.time > _poisonDelayTimer + attacker.DebuffCompo.DebuffStatData.poisonDelay)
            {
                _ownerController.OnHit(attacker.DebuffCompo.DebuffStatData.poisonDamage);

                _poisonDelayTimer = Time.time;
            }
        }
        else
        {
            PlayerController attacker = _attackers[DebuffType.Poison] as PlayerController;

            _poisonDelayTimer = Time.time - attacker.DebuffCompo.DebuffStatData.poisonDelay;

            if (Time.time > _poisonDelayTimer + attacker.DebuffCompo.DebuffStatData.poisonDelay)
            {
                _ownerBrain.OnHit(attacker.DebuffCompo.DebuffStatData.poisonDamage);

                _poisonDelayTimer = Time.time;
            }
        }
    }

    public void Freeze()
    {
        if (_ownerController)
        {
            Brain attacker = _attackers[DebuffType.Freeze] as Brain;

            if (_freezeDurationTimer <= 0f)
            {
                _freezeDurationTimer = Time.time;

                _ownerController.PlayerStatData.moveSpeed.AddModifier(attacker.DebuffCompo.DebuffStatData.freezeMoveSpeedModifier);
            }

            if (Time.time > _freezeDurationTimer + attacker.DebuffCompo.DebuffStatData.freezeDuration)
            {
                _ownerController.PlayerStatData.moveSpeed.RemoveModifier(attacker.DebuffCompo.DebuffStatData.freezeMoveSpeedModifier);

                _freezeDurationTimer = -1f;
            }
        }
        else
        {
            PlayerController attacker = _attackers[DebuffType.Freeze] as PlayerController;

            if (_freezeDurationTimer <= 0f)
            {
                _freezeDurationTimer = Time.time;

                _ownerBrain.EnemyStatData.moveSpeed.AddModifier(attacker.DebuffCompo.DebuffStatData.freezeMoveSpeedModifier);
            }

            if (Time.time > _freezeDurationTimer + attacker.DebuffCompo.DebuffStatData.freezeDuration)
            {
                _ownerBrain.EnemyStatData.moveSpeed.RemoveModifier(attacker.DebuffCompo.DebuffStatData.freezeMoveSpeedModifier);

                _freezeDurationTimer = -1f;
            }
        }
    }

    public void Knockback()
    {
        if (_ownerController)
        {
            Brain attacker = _attackers[DebuffType.Knockback] as Brain;

            _ownerController.RigidbodyCompo.AddForce((transform.position - attacker.transform.position).normalized * attacker.DebuffCompo.DebuffStatData.knockbackForce, ForceMode.Impulse);
        }
        else
        {
            PlayerController attacker = _attackers[DebuffType.Knockback] as PlayerController;

            _ownerBrain.RigidbodyCompo.AddForce((transform.position - attacker.transform.position).normalized * attacker.DebuffCompo.DebuffStatData.knockbackForce, ForceMode.Impulse);
        }
    }
}
