using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Stat/Buff")]
public class BuffStat : ScriptableObject
{
    [Header("Shield")]
    [Tooltip("방패 지속 시간")]
    public float shieldDuration = 3f;

    [Header("Poison")]
    [Tooltip("중독 대미지")]
    public float poisonDamage = 2f;
    [Tooltip("중독 대미지 딜레이")]
    public float poisonDelay = 1f;
    [Tooltip("중독 지속 시간")]
    public float poisonDuration = 3f;

    [Header("Freeze")]
    [Tooltip("빙결 이동 속도 감소량")]
    public float freezeMoveSpeedModifier = -2f;
    [Tooltip("빙결 지속 시간")]
    public float freezeDuration = 3f;

    [Header("Paralysis")]
    [Tooltip("마비 지속 시간")]
    public float paralysisDuration = 5f;
}
