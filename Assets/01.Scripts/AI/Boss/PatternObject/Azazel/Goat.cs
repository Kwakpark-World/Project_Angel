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
        base.Update();

        if (_isLiving)
        {
            if (Time.time > _lifetimer + _lifetime)
            {
                OnDie();
            }

            if (!GameManager.Instance.player.IsDie)
            {
                NavMeshAgentCompo.SetDestination(GameManager.Instance.playerTransform.position);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == GameManager.Instance.player.gameObject)
        {
            GameManager.Instance.player.PlayerStatData.Hit(_attackPower);

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

        NavMeshAgentCompo.speed = EnemyStatData.GetMoveSpeed();
        _lifetime = EnemyStatData.GetLifetime();
        _attackPower = EnemyStatData.GetAttackPower();
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
