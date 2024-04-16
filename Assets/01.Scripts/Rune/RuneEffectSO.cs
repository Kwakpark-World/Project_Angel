using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RuneEffectSO : ScriptableObject
{
    public RuneType runeType;
    public GameObject runeObj;
    public Color lightColor;

    public abstract void UseEffect();
    public abstract void KillEffect();
}
