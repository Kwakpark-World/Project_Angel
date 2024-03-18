using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EnemyAnimator : MonoBehaviour
{
    private Dictionary<string, int> _parameterHashes = new Dictionary<string, int>();
    private Animator _animator;
    private string _previousParameter = "";

    private void Awake()
    {
        _animator = GetComponent<Animator>();

        foreach (AnimatorControllerParameter parameter in _animator.parameters)
        {
            _parameterHashes.Add(parameter.name, parameter.nameHash);
        }
    }

    public void SetBoolEnable(string parameterName)
    {
        if (parameterName == "isIdle")
        {
            SetBoolDisable();

            return;
        }

        _animator.SetBool(_parameterHashes[parameterName], true);

        if (_previousParameter != "")
        {
            _animator.SetBool(_parameterHashes[_previousParameter], false);
        }

        _previousParameter = parameterName;
    }

    public void SetBoolDisable()
    {
        _animator.SetBool(_parameterHashes[_previousParameter], false);

        _previousParameter = "";
    }
}
