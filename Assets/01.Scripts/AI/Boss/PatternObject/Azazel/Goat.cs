using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goat : Brain
{
    private bool _isLiving;

    private float _lifetime;
    private float _lifetimer;

    protected override void Update()
    {
        base.Update();

        if (_isLiving)
        {
            if (Time.time > _lifetimer + _lifetime)
            {
                OnDie();
            }

            if (!GameManager.Instance.PlayerInstance.IsDie)
            {
                NavMeshAgentCompo.SetDestination(GameManager.Instance.PlayerInstance.transform.position);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == GameManager.Instance.PlayerInstance.gameObject)
        {
            GameManager.Instance.PlayerInstance.OnHit(EnemyStatData.GetAttackPower());

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
    }

    public override void OnHit(float incomingDamage)
    {
        // Do nothing.
    }

    public override void OnDie()
    {
        _isLiving = false;

        PoolManager.Instance.Push(this, true);
    }
}
