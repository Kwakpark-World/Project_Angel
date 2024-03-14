using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goat : Brain
{
    private bool _isLiving;

    private float _lifetime;
    private float _lifetimer;

    private float _attackPower;

    protected override void Update()
    {
        if (_isLiving && (Time.time > _lifetimer + _lifetime))
        {
            OnDie();
        }

        if (_isLiving && !GameManager.Instance.player.IsDie)
        {
            NavMeshAgentCompo.SetDestination(GameManager.Instance.playerTransform.position);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == GameManager.Instance.player.gameObject)
        {
            GameManager.Instance.player.PlayerStat.Hit(_attackPower);

            // Give scapegoat debuff.

            OnDie();
        }
    }

    public override void InitializePoolingItem()
    {
        base.InitializePoolingItem();

        _isLiving = true;
        _lifetimer = Time.time;
    }

    protected override void Initialize()
    {
        base.Initialize();

        NavMeshAgentCompo.speed = EnemyStatistic.GetMoveSpeed();
        _lifetime = EnemyStatistic.GetLifetime();
        _attackPower = EnemyStatistic.GetAttackPower();
    }

    public override void OnHit()
    {
        // Do nothing.
    }

    public override void OnDie()
    {
        _isLiving = false;

        PoolManager.Instance.Push(this, true);
    }
}
