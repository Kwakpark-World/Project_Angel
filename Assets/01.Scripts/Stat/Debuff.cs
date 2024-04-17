using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum DebuffType
{
    Poison,
    Freeze,
    Knockback,
    Scapegoat,
    // Fill here.
}

[Serializable]
public struct DebuffTrigger
{
    [Space(10)]
    public DebuffType debuffType;
    [Space(10)]
    public UnityEvent onDebuffBegin;
    [Space(10)]
    public UnityEvent onDebuffPlaying;
    [Space(10)]
    public UnityEvent onDebuffEnd;
}

public class Debuff : MonoBehaviour
{
    [SerializeField]
    private List<DebuffTrigger> debuffTriggers;
    [field: SerializeField]
    public DebuffStat DebuffStatData { get; private set; }

    private Dictionary<DebuffType, DebuffTrigger> _debuffTriggersByType = new Dictionary<DebuffType, DebuffTrigger>();
    private Dictionary<DebuffType, object> _attackers = new Dictionary<DebuffType, object>();
    private Dictionary<DebuffType, bool> _debuffStates = new Dictionary<DebuffType, bool>();
    private Dictionary<DebuffType, Coroutine> _coroutines = new Dictionary<DebuffType, Coroutine>();

    private Player _ownerController;
    private Brain _ownerBrain;

    private float _poisonDelayTimer = -1f;

    private void Awake()
    {
        foreach (DebuffTrigger debuffTrigger in debuffTriggers)
        {
            _debuffTriggersByType.Add(debuffTrigger.debuffType, debuffTrigger);
        }
    }

    private void Start()
    {
        foreach (DebuffType debuffType in Enum.GetValues(typeof(DebuffType)))
        {
            if (!_debuffTriggersByType.ContainsKey(debuffType))
            {
                _debuffTriggersByType.Add(debuffType, new DebuffTrigger());
            }
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
                _debuffTriggersByType[debuffType].onDebuffPlaying?.Invoke();
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
        if (_ownerController.StateMachine.CurrentState == _ownerController.StateMachine.GetState(PlayerStateEnum.Die))
        {
            return;
        }

        _attackers[debuffType] = attacker;

        _debuffTriggersByType[debuffType].onDebuffBegin?.Invoke();
    }

    public void SetDebuff(DebuffType debuffType, float duration, object attacker)
    {
        if (_ownerController.StateMachine.CurrentState == _ownerController.StateMachine.GetState(PlayerStateEnum.Die))
        {
            return;
        }

        _attackers[debuffType] = attacker;

        if (_coroutines[debuffType] != null)
        {
            StopCoroutine(_coroutines[debuffType]);
        }

        _coroutines[debuffType] = StartCoroutine(DebuffCoroutine(debuffType, duration));
        RuneManager.Instance.isArmor = false;
    }

    public bool GetDebuff(DebuffType debuffType)
    {
        return _debuffStates[debuffType];
    }

    private IEnumerator DebuffCoroutine(DebuffType debuffType, float duration)
    {
        _debuffStates[debuffType] = true;

        _debuffTriggersByType[debuffType].onDebuffBegin?.Invoke();

        yield return new WaitForSeconds(duration);

        _debuffTriggersByType[debuffType].onDebuffEnd?.Invoke();

        _debuffStates[debuffType] = false;
        _coroutines[debuffType] = null;
    }

    #region Poison Functions
    public void PoisonBegin()
    {
        if (_ownerController)
        {
            _poisonDelayTimer = Time.time - (_attackers[DebuffType.Poison] as Brain).DebuffCompo.DebuffStatData.poisonDelay;
            Debug.Log(RuneManager.Instance.isArmor);
        }
        else
        {
            _poisonDelayTimer = Time.time - (_attackers[DebuffType.Poison] as Player).DebuffCompo.DebuffStatData.poisonDelay;
        }
    }

    public void PoisonPlaying()
    {
        if (_ownerController)
        {
            Brain attacker = _attackers[DebuffType.Poison] as Brain;

            if (Time.time > _poisonDelayTimer + attacker.DebuffCompo.DebuffStatData.poisonDelay)
            {
                _ownerController.OnHit(attacker.DebuffCompo.DebuffStatData.poisonDamage);

                _poisonDelayTimer = Time.time;
            }
        }
        else
        {
            Player attacker = _attackers[DebuffType.Poison] as Player;

            if (Time.time > _poisonDelayTimer + attacker.DebuffCompo.DebuffStatData.poisonDelay)
            {
                _ownerBrain.OnHit(attacker.DebuffCompo.DebuffStatData.poisonDamage);

                _poisonDelayTimer = Time.time;
            }
        }
    }
    #endregion

    #region Freeze Functions
    public void FreezeBegin()
    {
        if (_ownerController)
        {
            Brain attacker = _attackers[DebuffType.Freeze] as Brain;

            _ownerController.PlayerStatData.moveSpeed.AddModifier(attacker.DebuffCompo.DebuffStatData.freezeMoveSpeedModifier);
        }
        else
        {
            PlayerController attacker = _attackers[DebuffType.Freeze] as PlayerController;

            _ownerBrain.EnemyStatData.moveSpeed.AddModifier(attacker.DebuffCompo.DebuffStatData.freezeMoveSpeedModifier);
        }

        RuneManager.Instance.isArmor = false;
    }

    public void FreezeEnd()
    {
        if (_ownerController)
        {
            Brain attacker = _attackers[DebuffType.Freeze] as Brain;

            _ownerController.PlayerStatData.moveSpeed.RemoveModifier(attacker.DebuffCompo.DebuffStatData.freezeMoveSpeedModifier);
        }
        else
        {
            PlayerController attacker = _attackers[DebuffType.Freeze] as PlayerController;

            _ownerBrain.EnemyStatData.moveSpeed.RemoveModifier(attacker.DebuffCompo.DebuffStatData.freezeMoveSpeedModifier);
        }
    }
    #endregion

    #region Knockback Functions
    public void KnockbackBegin()
    {
        if (_ownerController)
        {
            if (_ownerController.StateMachine.CurrentState == _ownerController.StateMachine.GetState(PlayerStateEnum.QSkill) || _ownerController.StateMachine.CurrentState == _ownerController.StateMachine.GetState(PlayerStateEnum.ESkill))
            {
                return;
            }

            Brain attacker = _attackers[DebuffType.Knockback] as Brain;

            _ownerController.RigidbodyCompo.AddForce((transform.position - attacker.transform.position).normalized * attacker.DebuffCompo.DebuffStatData.knockbackForce, ForceMode.Impulse);
        }
        else
        {
            PlayerController attacker = _attackers[DebuffType.Knockback] as PlayerController;

            _ownerBrain.RigidbodyCompo.AddForce((transform.position - attacker.transform.position).normalized * attacker.DebuffCompo.DebuffStatData.knockbackForce, ForceMode.Impulse);
        }
    }
    #endregion
}
