using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Stat/Buff")]
public class BuffStat : ScriptableObject
{
    public float poisonDamage = 2f;
    public float poisonDelay = 1f;
    public float poisonDuration = 3f;
    public float freezeMoveSpeedModifier = -2f;
    public float freezeDuration = 3f;
    public float knockbackForce = 5f;
}
