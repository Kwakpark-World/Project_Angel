using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    public string parameterName;
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

    [SerializeField]
    private Transform _weaponTransform;
    private Brain _owner;

    private Dictionary<string, AnimationTrigger> _animationTriggersByParameter = new Dictionary<string, AnimationTrigger>();
    private Dictionary<string, int> _parameterHashes = new Dictionary<string, int>();
    private Animator _animator;
    private string _currentState = "Idle";
    private string _previousState = "";

    private void Awake()
    {
        _animator = GetComponent<Animator>();

        foreach (AnimationTrigger animationTrigger in animationTriggers)
        {
            _animationTriggersByParameter.Add(animationTrigger.parameterName, animationTrigger);
        }
    }

    private void Start()
    {
        foreach (AnimatorControllerParameter parameter in _animator.parameters)
        {
            if (!_animationTriggersByParameter.ContainsKey(parameter.name))
            {
                _animationTriggersByParameter.Add(parameter.name, new AnimationTrigger());
            }

            _parameterHashes.Add(parameter.name, parameter.nameHash);
        }
    }

    public void SetOwner(Brain owner)
    {
        _owner = owner;
    }

    public void SetAnimationState(string stateName)
    {
        _animator.SetBool(_parameterHashes["is" + _currentState], false);

        _currentState = stateName;

        _animator.SetBool(_parameterHashes["is" + _currentState], true);
        Debug.Log(_parameterHashes["is" + _currentState]);
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

    public string GetCurrentAnimationState()
    {
        return _currentState;
    }

    public void OnAnimationBegin()
    {
        _animationTriggersByParameter["is" + _currentState].onAnimationBegin?.Invoke();
    }

    public void OnAnimationPlaying()
    {
        _animationTriggersByParameter["is" + _currentState].onAnimationPlaying?.Invoke();
    }

    public void OnAnimationEnd(string stateName)
    {
        _animationTriggersByParameter["is" + _currentState].onAnimationEnd?.Invoke();

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
    public void ArcherNormalAttack()
    {
        EnemyArrow arrow = PoolManager.Instance.Pop(PoolingType.EnemyArrow, _weaponTransform.position) as EnemyArrow;
        arrow.owner = _owner as EnemyBrain;
    }

    public void WitchNormalAttack()
    {
        PoolingType potionType = PoolingType.None;

        switch (UnityEngine.Random.Range(0, 3))
        {
            case 0:
                potionType = PoolingType.PoisonPotion;

                break;

            case 1:
                potionType = PoolingType.FreezePotion;

                break;

            case 2:
                potionType = PoolingType.KnockbackPotion;

                break;
        }

        DebuffPotion debuffPotion = PoolManager.Instance.Pop(potionType, _weaponTransform.position) as DebuffPotion;
        debuffPotion.owner = _owner as EnemyBrain;
    }
    #endregion

    #region Enemy Die Function
    public void EnemyDieProcess()
    {
        PoolManager.Instance.Push(_owner, (int)1.8);
    }
    #endregion
}
