using System;
using System.Collections;
using System.Collections.Generic;
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
}

[RequireComponent(typeof(Animator))]
public class EnemyAnimator : MonoBehaviour
{
    public List<AnimationTrigger> animationTriggers = new List<AnimationTrigger>();

    [SerializeField]
    private Transform _weaponTransform;
    [HideInInspector]
    private Brain _owner;

    private Dictionary<string, AnimationTrigger> _animationTriggersByParameter = new Dictionary<string, AnimationTrigger>();
    private Dictionary<string, int> _parameterHashes = new Dictionary<string, int>();
    private Animator _animator;
    private string _previousParameter = "isIdle";

    private void Awake()
    {
        _animator = GetComponent<Animator>();

        foreach (AnimationTrigger animationTrigger in animationTriggers)
        {
            _animationTriggersByParameter.Add(animationTrigger.parameterName, animationTrigger);
        }

        foreach (AnimatorControllerParameter parameter in _animator.parameters)
        {
            _parameterHashes.Add(parameter.name, parameter.nameHash);
        }
    }

    public void SetOwner(Brain owner)
    {
        _owner = owner;
    }

    public void SetBoolEnable(string parameterName)
    {
        if (parameterName == "isIdle")
        {
            SetBoolDisable();

            return;
        }

        _animator.SetBool(_parameterHashes[parameterName], true);

        if (_previousParameter != "isIdle")
        {
            _animator.SetBool(_parameterHashes[_previousParameter], false);
        }

        _previousParameter = parameterName;
    }

    public void SetBoolDisable()
    {
        if (_previousParameter != "isIdle")
        {
            _animator.SetBool(_parameterHashes[_previousParameter], false);

            _previousParameter = "isIdle";
        }
    }

    public void OnAnimationBegin()
    {
        _animationTriggersByParameter[_previousParameter].onAnimationBegin?.Invoke();
    }

    public void OnAnimationPlaying()
    {
        _animationTriggersByParameter[_previousParameter].onAnimationPlaying?.Invoke();
    }

    public void OnAnimationEnd()
    {
        _animationTriggersByParameter[_previousParameter].onAnimationEnd?.Invoke();
    }

    #region Enemy Normal Attack Functions
    public void ArcherNormalAttack()
    {
        EnemyArrow arrow = PoolManager.Instance.Pop(PoolingType.Arrow) as EnemyArrow;
        arrow.owner = _owner as EnemyBrain;
        arrow.transform.position = _weaponTransform.position;
        arrow.transform.rotation = _weaponTransform.rotation;
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

        DebuffPotion potion = PoolManager.Instance.Pop(potionType) as DebuffPotion;
        potion.owner = _owner as EnemyBrain;
        potion.transform.position = _weaponTransform.position;
        potion.transform.rotation = _weaponTransform.rotation;
    }
    #endregion
}
