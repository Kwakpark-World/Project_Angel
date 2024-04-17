using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Data/RuneList")]
public class RuneListSO : ScriptableObject
{
    public List<RuneDataSO> list;
}