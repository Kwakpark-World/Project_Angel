using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Grigori : Brain
{
    [HideInInspector]
    public BossBrain owner = null;
    [HideInInspector]
    public Player player = null;

    private float _attackRange;
    private float _attackDelay;
    private float _attackTimer;

    protected override void Update()
    {
        if (Vector3.Distance(player.transform.position, transform.position) <= _attackRange && Time.time > _attackTimer + _attackDelay)
        {
            Attack();
        }
    }

    protected abstract void Attack();

    protected abstract void Debuff();

    public override void InitializePoolingItem()
    {
        base.InitializePoolingItem();

        _attackTimer = Time.time;
    }

    protected override void Initialize()
    {
        base.Initialize();

        _attackRange = EnemyStatistic.GetAttackRange();
        _attackDelay = EnemyStatistic.GetAttackDelay();
    }

    public override void OnHit()
    {
        if (EnemyStatistic.GetCurrentHealth() <= 0f)
        {
            OnDie();
        }
    }

    public override void OnDie()
    {
        Debuff();
        GameManager.Instance.pool.Push(this);
    }
}
