using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum AnimationStateMode
{
    None,
    SavePreviousState,
    LoadPreviousState
}

[Serializable]
public struct AnimationTrigger
{
    [Space(10)]
    public string stateName;
    [Space(10)]
    public UnityEvent onAnimationBegin;
    [Space(10)]
    public UnityEvent onAnimationPlaying;
    [Space(10)]
    public UnityEvent onAnimationEnd;
}

[RequireComponent(typeof(Animator))]
public class EnemyAnimator : MonoBehaviour
{
    public List<AnimationTrigger> animationTriggers = new List<AnimationTrigger>();
    public Dictionary<string, AnimationTrigger> animationTriggersByState = new Dictionary<string, AnimationTrigger>();

    [SerializeField]
    private Transform _weaponTransform;
    private Brain _owner;

    private Dictionary<string, int> _parameterHashes = new Dictionary<string, int>();
    public Animator _animator;
    private string _currentState = "Idle";
    private string _previousState = "";

    private void Awake()
    {
        _animator = GetComponent<Animator>();

        foreach (AnimationTrigger animationTrigger in animationTriggers)
        {
            animationTriggersByState.Add(animationTrigger.stateName, animationTrigger);
        }
    }

    private void Start()
    {
        foreach (AnimatorControllerParameter parameter in _animator.parameters)
        {
            string state = parameter.name.Replace("is", "");

            if (!animationTriggersByState.ContainsKey(state))
            {
                animationTriggersByState.Add(state, new AnimationTrigger());
            }

            _parameterHashes.Add(parameter.name, parameter.nameHash);
        }
    }

    public void SetOwner(Brain owner)
    {
        _owner = owner;
    }

    public void SetAnimationState(string stateName = "Idle", AnimationStateMode stateMode = AnimationStateMode.None)
    {
        _animator.SetBool(_parameterHashes["is" + _currentState], false);

        switch (stateMode)
        {
            case AnimationStateMode.SavePreviousState:
                _previousState = _currentState;
                _currentState = stateName;

                break;

            case AnimationStateMode.LoadPreviousState:
                _currentState = _previousState;
                _previousState = "";

                break;

            default:
                _currentState = stateName;

                break;
        }

        _animator.SetBool(_parameterHashes["is" + _currentState], true);
    }

    public bool GetCurrentAnimationState(string stateName)
    {
        return _currentState == stateName;
    }

    public string GetCurrentAnimationState()
    {
        return _currentState;
    }

    public void OnAnimationBegin()
    {
        animationTriggersByState[_currentState].onAnimationBegin?.Invoke();
    }

    public void OnAnimationPlaying()
    {
        animationTriggersByState[_currentState].onAnimationPlaying?.Invoke();
    }

    public void OnAnimationEnd(string stateName)
    {
        animationTriggersByState[_currentState].onAnimationEnd?.Invoke();

        if (stateName != "")
        {
            SetAnimationState(stateName);
        }
        else if (_previousState != "")
        {
            SetAnimationState(stateMode: AnimationStateMode.LoadPreviousState);
        }
        else
        {
            SetAnimationState();
        }
    }

    #region Enemy Normal Attack Functions
    public void RangerNormalAttack()
    {
        EnemyArrow arrow = PoolManager.Instance.Pop(PoolType.Weapon_Arrow, _weaponTransform.position) as EnemyArrow;
        arrow.owner = _owner as EnemyBrain;
    }

    public void AlchemistNormalAttack()
    {
        PoolType potionType = PoolType.None;

        switch (UnityEngine.Random.Range(0, 3))
        {
            case 0:
                potionType = PoolType.Weapon_Potion_Poison;

                break;

            case 1:
                potionType = PoolType.Weapon_Potion_Freeze;

                break;

            case 2:
                potionType = PoolType.Weapon_Potion_Paralysis;

                break;
        }

        EnemyPotion debuffPotion = PoolManager.Instance.Pop(potionType, _weaponTransform.position) as EnemyPotion;
        debuffPotion.owner = _owner as EnemyBrain;
    }
    public void ShieldNormalAttack()
    {
        _owner.FindNearbyEnemies(5, 5f);
    }

    public void ArcherNormalAttack()
    {
        int arrowCount = 3;
        float angleRange = 45;
        float angleIncrement = angleRange / (arrowCount - 1);

        for (int i = 0; i < arrowCount; i++)
        {
            float angle = -angleIncrement * (arrowCount * 0.5f - 0.5f) + angleIncrement * i;

            EnemyArrow arrow = PoolManager.Instance.Pop(PoolType.Weapon_Arrow, _weaponTransform.position) as EnemyArrow;
            Vector3 direction = Quaternion.Euler(0f, angle, 0f) * arrow.transform.forward;
            arrow.transform.forward = direction;
            arrow.owner = _owner as EnemyBrain;
            //arrowObject.transform.forward = direction;
        }

    }

    public void ArcherSkillAttack()
    {
        int arrowCount = 5;
        float angleRange = 45;
        float angleIncrement = angleRange / (arrowCount - 1);

        for (int i = 0; i < arrowCount; i++)
        {
            float angle = -angleIncrement * (arrowCount * 0.5f - 0.5f) + angleIncrement * i;

            EnemyArrow arrow = PoolManager.Instance.Pop(PoolType.Weapon_Arrow, _weaponTransform.position) as EnemyArrow;
            Vector3 direction = Quaternion.Euler(0f, angle, 0f) * arrow.transform.forward;
            arrow.transform.forward = direction;
            arrow.owner = _owner as EnemyBrain;
            //Debug.Log(arrow);
            //arrowObject.transform.forward = direction;
        }
    }

    public void ChemistNormalAttack()
    {
        PoolType potionType = PoolType.None;

        switch (UnityEngine.Random.Range(0, 3))
        {
            case 0:
                potionType = PoolType.Weapon_AreaPotion_Poison;

                break;

            case 1:
                potionType = PoolType.Weapon_AreaPotion_Freeze;

                break;

            case 2:
                potionType = PoolType.Weapon_AreaPotion_Paralysis;

                break;
        }

        EnemyAreaPotion debuffPotion = PoolManager.Instance.Pop(potionType, _weaponTransform.position) as EnemyAreaPotion;
        debuffPotion.owner = _owner as EnemyBrain;
        debuffPotion.direction = new Vector3(GameManager.Instance.PlayerInstance.playerCenter.position.x - debuffPotion.transform.position.x, 0f, GameManager.Instance.PlayerInstance.playerCenter.position.z - debuffPotion.transform.position.z).normalized;

        debuffPotion.SetDefaultSpeed();
    }

    public void ChemistSkillAttack()
    {
        int randomPotion = UnityEngine.Random.Range(5, 10);

        for (int i = 0; i < randomPotion; i++)
        {
            Vector2 randomPos = UnityEngine.Random.insideUnitCircle * 10f;
            Vector3 spawnPos = transform.position + new Vector3(randomPos.x, 10, randomPos.y);

            PoolType potionType = PoolType.None;

            switch (UnityEngine.Random.Range(0, 3))
            {
                case 0:
                    potionType = PoolType.Weapon_AreaPotion_Poison;

                    break;

                case 1:
                    potionType = PoolType.Weapon_AreaPotion_Freeze;

                    break;

                case 2:
                    potionType = PoolType.Weapon_AreaPotion_Paralysis;

                    break;
            }

            EnemyAreaPotion debuffPotion = PoolManager.Instance.Pop(potionType, spawnPos) as EnemyAreaPotion;
            debuffPotion.owner = _owner as EnemyBrain;
            debuffPotion.direction = Vector3.down;
            debuffPotion.speed = 9.8f;
        }
    }
    #endregion

    #region Enemy Die Function
    public void EnemyDieProcess()
    {
        PoolManager.Instance.Push(_owner, 1.8f);
    }
    #endregion
}
