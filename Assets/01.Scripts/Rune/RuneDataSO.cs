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
    public BuffType skillSynergyType = BuffType.Rune_Synergy_Dash;
    public BuffType mythSynergyType = BuffType.Rune_Synergy_Jeus;
    public BuffType buffType = BuffType.Rune_Dash_Hermes;
    public GameObject runeObject;
    public Sprite runeSprite;
    public Color runeColor;
    #endregion
}
