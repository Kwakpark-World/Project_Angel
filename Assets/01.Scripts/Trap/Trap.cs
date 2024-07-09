using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Trap : PoolableMono
{
    protected float _trapDamage;

    protected bool _isOnTrap;
    protected bool _isPlayTrap { get; private set; }

    protected virtual void Awake()
    {
        _isOnTrap = false;
        _isPlayTrap = false;
    }

    public override void InitializePoolItem()
    {
        base.InitializePoolItem();

        _isOnTrap = true;
        _isPlayTrap = false;

    }

    protected virtual void Update()
    {
        if (!_isOnTrap) return;

    }

    protected void OnTrap()
    {
        if (_isPlayTrap) return;

        StartTrap();
        _isPlayTrap = true;
    }

    protected virtual void StartTrap()
    {
        PlayTrap();
    }
    protected virtual void PlayTrap()
    {
        EndTrap();
    }
    protected virtual void EndTrap()
    {
        _isPlayTrap = false;
    }

    protected void Attack(Brain[] enemies, Player player)
    {
        if (enemies.Length > 0)
        {
            foreach (var enemy in enemies)
            {
                enemy.OnHit(_trapDamage);
            }
        }

        if (player != null)
            player.OnHit(_trapDamage);
    }

    protected void Attack(Brain enemy)
    {
        if (enemy != null)
            enemy.OnHit(_trapDamage);
    }

    protected void Attack(Player player)
    {
        if (player != null)
            player.OnHit(_trapDamage);
    }
}
