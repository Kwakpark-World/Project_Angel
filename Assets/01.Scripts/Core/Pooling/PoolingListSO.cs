using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PoolingType
{
    Arrow,
    Goat,
    Shamshiel,
    Sariel,
    Satanael,
}

[Serializable]
public struct PoolingObject
{
    public PoolingType type;
    public PoolableMono prefab;
    public int itemAmount;
}

[CreateAssetMenu(menuName = "SO/PoolingList")]
public class PoolingListSO : ScriptableObject
{
    public List<PoolingObject> poolingList;
}