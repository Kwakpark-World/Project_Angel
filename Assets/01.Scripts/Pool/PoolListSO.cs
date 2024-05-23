using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PoolType
{
    None = 0,
    Rune = 1,
    Enemy_Knight = 100,
    Enemy_Ranger = 101,
    Enemy_Alchemist = 102,
    Enemy_Shielder = 103,
    Enemy_Archer = 104,
    Enemy_Chemist = 105,
    Enemy_Boss_Azazel = 200,
    Enemy_Goat = 300,
    Enemy_Shamshiel = 301,
    Enemy_Sariel = 302,
    Enemy_Satanael = 303,
    Weapon_Arrow = 1000,
    Weapon_Potion_Poison = 1100,
    Weapon_Potion_Freeze = 1101,
    Weapon_Potion_Paralysis = 1102,
    Effect_Shield = 2000,
    Effect_PlayerAttack_Slam_Normal = 2100,
    Effect_PlayerAttack_Slam_Awaken_0 = 2101,
    Effect_PlayerAttack_Slam_Awaken_1 = 2102,
    Effect_PlayerAttack_Slam_Awaken_2 = 2203,
    Dynamite = 3000,
}

[Serializable]
public class PoolObject
{
    [ReadOnly]
    public string name;
    public PoolType type;
    public PoolableMono prefab;
    public int itemAmount;
}

[CreateAssetMenu(menuName = "SO/Data/PoolList")]
public class PoolListSO : ScriptableObject
{
    public List<PoolObject> poolList;
}
