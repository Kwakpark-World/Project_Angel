using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PoolType
{
    None = 0,
    Rune,
    Enemy_Knight = 100,
    Enemy_Ranger,
    Enemy_Alchemist,
    Enemy_Shielder,
    Enemy_Archer,
    Enemy_Chemist,
    Enemy_Boss_Azazel = 200,
    Enemy_Goat = 300,
    Enemy_Shamshiel,
    Enemy_Sariel,
    Enemy_Satanael,
    Weapon_Arrow = 1000,
    Weapon_Potion_Poison = 1100,
    Weapon_Potion_Freeze,
    Weapon_Potion_Paralysis,
    Weapon_AreaPotion_Poison = 1200,
    Weapon_AreaPotion_Freeze,
    Weapon_AreaPotion_Paralysis,
    Weapon_BuffArea_Poison = 1300,
    Weapon_BuffArea_Freeze,
    Weapon_BuffArea_Paralysis,
    Effect_Shield = 2000,
    Effect_PlayerAttack_Slam_Normal = 2100,
    Effect_PlayerAttack_Slam_Awaken_0,
    Effect_PlayerAttack_Slam_Awaken_1,
    Effect_PlayerAttack_Slam_Awaken_2,
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
