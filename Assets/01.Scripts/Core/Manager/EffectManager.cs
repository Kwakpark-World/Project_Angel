using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoSingleton<EffectManager>
{
    private Dictionary<string, PoolingType> _effects = new Dictionary<string, PoolingType>();

    public void RegisterEffect(PoolingType type)
    {
        string effectName = type.ToString();
        if (!_effects.ContainsKey(effectName))
        {
            _effects.Add(effectName, type);
        }
        //else
            //Debug.Log($"This Object already contain effect. Object Pooling Type : {type}");
    }

    public PoolingType GetEffectType(string effectName)
    {
        if (!_effects.ContainsKey(effectName))
        {
            Debug.Log($"{effectName} is not Contain _effects");
            return PoolingType.None;
        }

        return _effects[effectName];
    }

    public void PlayEffect(PoolingType type, Vector3 pos)
    {
        string effectName = type.ToString();

        if (!_effects.ContainsKey(effectName))
        {
            Debug.LogError($"{effectName} is Not Contain _effects");
            return;
        }

        PoolManager.Instance.Pop(type, pos);
    }
}
