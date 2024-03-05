using BTVisual;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Brain : MonoBehaviour
{
    public BehaviourTreeRunner treeRunner;

    // Control statistics here or child brain, using scriptable object and so on.
    #region Debug
    [Header("Debug statistics")]
    public float hitPoint;
    protected float currentHitPoint;
    public float CurrentHitPoint
    {
        get => currentHitPoint;
        set => currentHitPoint = value;
    }

    public float attackPower;

    public float normalAttackDelay = 3f;
    protected float normalAttackTimer;
    public float NormalAttackTimer
    {
        get => normalAttackTimer;
        set => normalAttackTimer = value;
    }
    #endregion

    protected virtual void Start()
    {
        Initialize();
    }

    protected abstract void Update();

    protected abstract void Initialize();

    public abstract void OnHit(float damage);

    public abstract void OnDie();
}
