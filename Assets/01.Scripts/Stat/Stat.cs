using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Stat
{
    [SerializeField]
    private float _baseValue;
    public List<float> modifiers;

    public float GetValue()
    {
        float finalValue = _baseValue;
        for (int i = 0; i < modifiers.Count; ++i)
        {
            finalValue += modifiers[i];
        }
        return finalValue;
    }



    public void AddModifier(float value)
    {
        if (value != 0)
        {
            modifiers.Add(value);
        }
    }

    public void RemoveModifier(float value)
    {
        if (value != 0)
        {
            modifiers.Remove(value);
        }
    }

    public void InitializeModifier()
    {
        modifiers.Clear();
    }

    public void SetDefalutValue(float value)
    {
        _baseValue = value;
    }

/*    public static explicit operator float(Stat v)
    {
        throw new NotImplementedException();
    }*/
}
