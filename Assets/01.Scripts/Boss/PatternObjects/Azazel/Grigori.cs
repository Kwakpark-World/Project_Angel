using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Grigori : MonoBehaviour
{
    [SerializeField]
    private float _hitPoint;
    [SerializeField]
    private float _attackDamage;
    [SerializeField]
    private float _attackDelay;
    [SerializeField]
    private float _attackRange;
    public Player player;
    private float _currentHitPoint;
    private float _attackTimer;

    protected virtual void OnEnable()
    {
        player = FindAnyObjectByType<Player>();
        _attackTimer = Time.time;
    }

    private void Update()
    {
        if (Vector3.Distance(player.transform.position, transform.position) <= _attackRange && Time.time > _attackTimer + _attackDelay)
        {
            Attack();
        }
    }

    public virtual void OnDie()
    {
        Debuff();
    }

    protected abstract void Attack();

    protected abstract void Debuff();

    public void OnHit(float damage)
    {
        _currentHitPoint -= damage;

        if (_currentHitPoint <= 0f)
        {
            _currentHitPoint = 0f;

            // Set dead to grigori.
            OnDie();
        }
    }
}
