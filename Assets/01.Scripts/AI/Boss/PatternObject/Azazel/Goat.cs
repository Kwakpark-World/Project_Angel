using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class Goat : Brain
{
    [HideInInspector]
    public Player player = null;

    private float _lifetime;
    private float _lifetimer;
    private float _moveSpeed;

    private float _attackPower;

    protected override void Update()
    {
        if (Time.time > _lifetimer + _lifetime)
        {
            OnDie();
        }

        if (player != null)
        {
            NavMeshAgentCompo.SetDestination(player.transform.position);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player.gameObject)
        {
            player.PlayerStat.Hit(_attackPower);

            // Give scapegoat effect.
        }

        OnDie();
    }

    public override void InitializePoolingItem()
    {
        base.InitializePoolingItem();

        _lifetimer = Time.time;
    }

    protected override void Initialize()
    {
        base.Initialize();

        _lifetime = EnemyStatistic.GetLifetime();
        _moveSpeed = EnemyStatistic.GetMoveSpeed();
        _attackPower = EnemyStatistic.GetAttackPower();
    }

    public override void OnHit()
    {
        // Do nothing.
    }

    public override void OnDie()
    {
        GameManager.Instance.pool.Push(this, true);
    }
}
