using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Rune/RuneList")]
public class RuneListSO : ScriptableObject
{
    public List<RuneEffectSO> list;
	public RuneEffectSO this[int index]
	{
		get => list[index];
	}

    public void Remove(RuneEffectSO runeData)
    {
        list.Remove(runeData);
    }
}