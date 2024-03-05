using System;
using UnityEngine;

[Serializable]
public class Blackboard
{
    public LayerMask enemyLayer;
    public Vector3 destination;
    public GameObject target;
    public int selectedPattern;
    public float nextPatternCooldown;
}
