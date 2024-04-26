using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PoolingType
{
    None,
    KnightEnemy,
    RangerEnemy,
    WitchEnemy,
    Azazel,
    Goat,
    Shamshiel,
    Sariel,
    Satanael,
    Arrow,
    Potion_Poison,
    Potion_Freeze,
    Potion_Knockback,
    Effect_PlayerAttack_Normal,
    Effect_PlayerAttack_Charging_Normal,
    Effect_PlayerAttack_Charged_Normal,
    Effect_PlayerAttack_Q_Normal,
    Effect_PlayerAwakening,
    Effect_PlayerAwakened,
    Effect_PlayerAttack_Awaken,
    Effect_PlayerAttack_Charging_Awaken,
    Effect_PlayerAttack_Charged_Awaken,
    Effect_PlayerAttack_Q_Awaken,
    Rune
}

[Serializable]
public class PoolingObject
{
    [ReadOnly]
    public string name;
    public PoolingType type;
    public PoolableMono prefab;
    public int itemAmount;
}

[CreateAssetMenu(menuName = "SO/Data/PoolingList")]
public class PoolingListSO : ScriptableObject
{
    public List<PoolingObject> poolingList;
}
