using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Data/Rune")]
public class RuneDataSO : ScriptableObject
{
    #region Rune description
    public string runeDisplayedName;
    [Multiline(5)]
    public string runeDescription;
    #endregion

    #region Rune data
    public BuffType statSynergyType = BuffType.Rune_Synergy_Attack;
    public BuffType mythSynergyType = BuffType.Rune_Synergy_Zeus;
    public BuffType buffType = BuffType.Rune_Attack_Heracles;
    public GameObject runeObject;
    public Sprite runeSprite;
    public Color runeColor;
    #endregion
}
