using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.Events;

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
    [Space(10)] 
    public UnityEvent onHitAnimation;
}

[RequireComponent(typeof(Animator))]
public class EnemyAnimator : MonoBehaviour
{
    private AnimationTrigger hitAnimationTrigger;
    public List<AnimationTrigger> animationTriggers = new List<AnimationTrigger>();

    [SerializeField]
    private Transform _weaponTransform;
    private Brain _owner;

    private Dictionary<string, AnimationTrigger> _animationTriggersByParameter = new Dictionary<string, AnimationTrigger>();
    private Dictionary<string, bool> _animationStates = new Dictionary<string, bool>();
    private Dictionary<string, int> _parameterHashes = new Dictionary<string, int>();
    private Animator _animator;
    private string _enabledParameter = "isIdle";

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

            _animationStates.Add(parameter.name, false);
            _parameterHashes.Add(parameter.name, parameter.nameHash);
        }
    }

    public void SetOwner(Brain owner)
    {
        _owner = owner;
    }

    public void SetParameterEnable(string parameterName = "isIdle")
    {
        if (parameterName == "isIdle")
        {
            SetParameterDisable();

            return;
        }

        _animator.SetBool(_parameterHashes[parameterName], true);

        _animationStates[parameterName] = true;

        if (_enabledParameter != "isIdle")
        {
            _animator.SetBool(_parameterHashes[_enabledParameter], false);

            _animationStates[_enabledParameter] = false;
        }

        _enabledParameter = parameterName;

        
    }

    public void SetParameterDisable()
    {
        if (_enabledParameter != "isIdle")
        {
            _animator.SetBool(_parameterHashes[_enabledParameter], false);

            _animationStates[_enabledParameter] = false;
            _enabledParameter = "isIdle";
            Debug.Log("3");
        }
    }

    public string GetEnabledParameter()
    {
        return _enabledParameter;
    }

    public bool GetParameterState(string parameterName)
    {
        return _animationStates[parameterName];
    }

    public void OnAnimationBegin()
    {
        _animationTriggersByParameter[_enabledParameter].onAnimationBegin?.Invoke();
    }

    public void OnAnimationPlaying()
    {
        _animationTriggersByParameter[_enabledParameter].onAnimationPlaying?.Invoke();
    }

    public void OnAnimationEnd(int isEndOfClip = 0)
    {
        _animationTriggersByParameter[_enabledParameter].onAnimationEnd?.Invoke();

        if (isEndOfClip == 1)
        {
            SetParameterDisable();
        }
    }

    #region Enemy Normal Attack Functions
    public void ArcherNormalAttack()
    {
        EnemyArrow arrow = PoolManager.Instance.Pop(PoolingType.Arrow, _weaponTransform.position) as EnemyArrow;
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
}
