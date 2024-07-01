using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Stat/Buff")]
public class BuffStat : ScriptableObject
{
    [Header("Shield")]
    [Tooltip("���� ���� �ð�")]
    public float shieldDuration = 3f;

    [Header("Poison")]
    [Tooltip("�ߵ� �����")]
    public float poisonDamage = 2f;
    [Tooltip("�ߵ� ����� ������")]
    public float poisonDelay = 1f;
    [Tooltip("�ߵ� ���� �ð�")]
    public float poisonDuration = 3f;

    [Header("Freeze")]
    [Tooltip("���� �̵� �ӵ� ���ҷ�")]
    public float freezeMoveSpeedModifier = -2f;
    [Tooltip("���� ���� �ð�")]
    public float freezeDuration = 3f;

    [Header("Paralysis")]
    [Tooltip("���� ���� �ð�")]
    public float paralysisDuration = 5f;
}
