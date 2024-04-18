using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Data/Rune")]
public class RuneDataSO : ScriptableObject
{
    public RuneType runeType;
    public GameObject runeObj;
    public Sprite runeSprite;
    public Color lightColor;
    public BuffType buffType;
}
