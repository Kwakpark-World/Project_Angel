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
    public string runeName;
    public string runeexplanation;

    public string GetNameAndExplanation(Dictionary<BuffType, string> selectItems)
    {
        if (selectItems.ContainsKey(buffType))
        {
            return selectItems[buffType];
        }
        else
        {
            return "BuffType에 해당하는 값이 없습니다.";
        }
    }
}
