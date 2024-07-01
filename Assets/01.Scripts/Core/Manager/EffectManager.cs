using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoSingleton<EffectManager>
{
    private Dictionary<string, PoolType> _effects = new Dictionary<string, PoolType>();

    public void RegisterEffect(PoolType type)
    {
        string effectName = type.ToString();
        if (!_effects.ContainsKey(effectName))
        {
            _effects.Add(effectName, type);
        }
        //else
            //Debug.Log($"This Object already contain effect. Object Pool Type : {type}");
    }

    public PoolType GetEffectType(string effectName)
    {
        if (!_effects.ContainsKey(effectName))
        {
            Debug.Log($"{effectName} is not Contain _effects");
            return PoolType.None;
        }

        return _effects[effectName];
    }

    public void PlayEffect(PoolType type, Vector3 pos)
    {
        string effectName = type.ToString();

        if (!_effects.ContainsKey(effectName))
        {
            Debug.LogError($"{effectName} is Not Contain _effects");
            return;
        }

        PoolManager.Instance.Pop(type, pos);
    }

    public void StopEffect(PoolType type, Vector3 pos)
    {
        string effectName = type.ToString();

        if (!_effects.ContainsKey(effectName))
        {
            Debug.LogError($"{effectName} is Not Contain _effects");
            return;
        }
    }

    public PoolableMono PlayAndGetEffect(PoolType type, Vector3 pos)
    {
        string effectName = type.ToString();

        if (!_effects.ContainsKey(effectName))
        {
            Debug.LogError($"{effectName} is Not Contain _effects");
            return null;
        }

        return PoolManager.Instance.Pop(type, pos);
    }

    public void PlayEffect(PoolType type, Vector3 pos, Transform parent)
    {
        string effectName = type.ToString();

        if (!_effects.ContainsKey(effectName))
        {
            Debug.LogError($"{effectName} is Not Contain _effects");
            return;
        }

        PoolManager.Instance.Pop(type, pos, parent);
    }
}
