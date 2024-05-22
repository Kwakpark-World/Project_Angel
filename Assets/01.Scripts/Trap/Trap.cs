using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Trap : MonoBehaviour
{
    protected float _trapDamage;

    protected bool _isOnTrap;
    protected bool _isPlayTrap { get; private set; }

    protected virtual void Start()
    {
        _isOnTrap = true;
        _isPlayTrap = false;
    }

    protected virtual void Update()
    {
        if (!_isOnTrap) return;

    }

    protected void OnTrap()
    {
        _isPlayTrap = true;

        PlayTrap();

        _isPlayTrap = false;
    }

    protected abstract void PlayTrap();

    protected void Attack(EnemyBrain[] enemies, Player player)
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

    protected void Attack(EnemyBrain enemy)
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
