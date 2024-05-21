using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PoolingType
{
    None,
    Enemy_Knight,
    Enemy_Ranger,
    Enemy_Alchemist,
    Enemy_Shielder,
    Enemy_Archer,
    Enemy_Wizard,
    Enemy_Boss_Azazel,
    Enemy_Goat,
    Enemy_Shamshiel,
    Enemy_Sariel,
    Enemy_Satanael,
    Weapon_Arrow,
    Weapon_Potion_Poison,
    Weapon_Potion_Freeze,
    Weapon_Potion_Paralysis,
    Effect_PlayerAttack_Slam_Normal,
    Effect_PlayerAttack_Slam_Awaken_0,
    Effect_PlayerAttack_Slam_Awaken_1,
    Effect_PlayerAttack_Slam_Awaken_2,
    Effect_Shield,
    Rune
}
//
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
