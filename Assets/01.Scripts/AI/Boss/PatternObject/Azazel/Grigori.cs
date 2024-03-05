using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Grigori : MonoBehaviour
{
    [HideInInspector]
    public Player player = null;

    #region Debug
    [Header("Debug statistics")]
    [SerializeField]
    private float _hitPoint;
    private float _currentHitPoint;

    [SerializeField]
    private float _attackRange;
    [SerializeField]
    private float _attackPower;
    [SerializeField]
    private float _attackDelay;
    private float _attackTimer;
    #endregion

    protected virtual void OnEnable()
    {
        _attackTimer = Time.time;
    }

    private void Update()
    {
        if (Vector3.Distance(player.transform.position, transform.position) <= _attackRange && Time.time > _attackTimer + _attackDelay)
        {
            Attack();
        }
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

    public virtual void OnDie()
    {
        Debuff();
    }
}
