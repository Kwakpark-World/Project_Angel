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

    [SerializeField]
    private Transform _weaponTransform;
    private Brain _owner;

    private Dictionary<string, AnimationTrigger> _animationTriggersByState = new Dictionary<string, AnimationTrigger>();
    private Dictionary<string, int> _parameterHashes = new Dictionary<string, int>();
    public Animator _animator;
    private string _currentState = "Idle";
    private string _previousState = "";

    private void Awake()
    {
        _animator = GetComponent<Animator>();

        foreach (AnimationTrigger animationTrigger in animationTriggers)
        {
            _animationTriggersByState.Add(animationTrigger.stateName, animationTrigger);
        }
    }

    private void Start()
    {
        foreach (AnimatorControllerParameter parameter in _animator.parameters)
        {
            string state = parameter.name.Replace("is", "");

            if (!_animationTriggersByState.ContainsKey(state))
            {
                _animationTriggersByState.Add(state, new AnimationTrigger());
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
        _animationTriggersByState[_currentState].onAnimationBegin?.Invoke();
    }

    public void OnAnimationPlaying()
    {
        _animationTriggersByState[_currentState].onAnimationPlaying?.Invoke();
    }

    public void OnAnimationEnd(string stateName)
    {
        _animationTriggersByState[_currentState].onAnimationEnd?.Invoke();

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
        EnemyArrow arrow = PoolManager.Instance.Pop(PoolingType.Weapon_Arrow, _weaponTransform.position) as EnemyArrow;
        arrow.owner = _owner as EnemyBrain;
    }

    public void WitchNormalAttack()
    {
        PoolingType potionType = PoolingType.None;

        switch (UnityEngine.Random.Range(0, 3))
        {
            case 0:
                potionType = PoolingType.Weapon_Potion_Poison;

                break;

            case 1:
                potionType = PoolingType.Weapon_Potion_Freeze;

                break;

            case 2:
                potionType = PoolingType.Weapon_Potion_Knockback;

                break;
        }

        EnemyPotion debuffPotion = PoolManager.Instance.Pop(potionType, _weaponTransform.position) as EnemyPotion;
        debuffPotion.owner = _owner as EnemyBrain;
    }

    public void ArcherNormalAttack()
    {
        int arrowCount = 3;
        float angleRange = 45;
        float angleIncrement = angleRange / (arrowCount - 1);

        for (int i = 0; i < arrowCount; i++)
        {
            float angle = -angleIncrement + angleIncrement * i;
            Vector3 direction = Quaternion.Euler(0, angle, 0) * -transform.forward;

            EnemyArrow arrow = PoolManager.Instance.Pop(PoolingType.Weapon_Arrow, _weaponTransform.position) as EnemyArrow;
            arrow.transform.forward = direction;
            arrow.owner = _owner as EnemyBrain;
            //arrowObject.transform.forward = direction;
        }

    }

    /*public void ArcherSkillAttack()
    {
        //StartCoroutine(ArcherSkill(3));
    }*/

    IEnumerator ArcherSkill(int duration)
    {
        for(int i =0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                int arrowCount = 5;
                float angleRange = 45;
                float angleIncrement = angleRange / (arrowCount - 1);

                float angle = -angleIncrement + angleIncrement * i;
                Vector3 direction = Quaternion.Euler(0, angle, 0) * -transform.forward;

                EnemyArrow arrow = PoolManager.Instance.Pop(PoolingType.Weapon_Arrow, _weaponTransform.position) as EnemyArrow;
                arrow.transform.forward = direction;
                arrow.owner = _owner as EnemyBrain;
                //arrowObject.transform.forward = direction;
            }
        }
        yield return new WaitForSeconds(duration);
    }
    #endregion

    #region Enemy Die Function
    public void EnemyDieProcess()
    {
        PoolManager.Instance.Push(_owner, 1.8f);
    }
    #endregion
}
